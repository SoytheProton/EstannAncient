using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace EstannAncient.EstannAncientCode.Potions;

[Pool(typeof(EventPotionPool))]
public class FakeBufferPotion : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Event;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>( 1M), new PowerVar<FrailPower>(3M)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BufferPower>(), HoverTipFactory.FromPower<FrailPower>()];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature, DynamicVars.Power<BufferPower>().BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<FrailPower>(choiceContext, Owner.Creature, DynamicVars.Power<FrailPower>().BaseValue, Owner.Creature, null);
    }
}