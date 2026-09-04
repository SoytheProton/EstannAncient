using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace EstannAncient.EstannAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class NameSeven : EstannAncientRelic
{
    private const string BlockRemoval = "BlockRemoval";
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new (BlockRemoval,4), new PowerVar<StrengthPower>(1M)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<StrengthPower>()];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if(cardPlay.Card.Owner != Owner || cardPlay.Card.Type != CardType.Attack || Owner.Creature.Block > 0)
            return;
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars[BlockRemoval].BaseValue, ValueProp.Unpowered, Owner.Creature, null);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, null, null);
    }
}