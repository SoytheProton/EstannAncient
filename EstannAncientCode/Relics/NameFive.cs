using BaseLib.Utils;
using EstannAncient.EstannAncientCode.Potions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace EstannAncient.EstannAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class NameFive : EstannAncientRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power", 2M), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<FrailPower>()];

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner || Owner.PlayerCombatState?.TurnNumber > 1 ? count : count + DynamicVars.Cards.BaseValue;
    }
    
    // Technically both of these could be in the same argument, but I'm trying to be consistent with Vanilla's timings on relics.
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        await PowerCmd.Apply<DoubleDamagePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
    }
    
    public override async Task AfterBlockCleared(Creature creature)
    {
        if (creature != Owner.Creature || Owner.PlayerCombatState?.TurnNumber != 2)
            return;
        Flash();
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<FrailPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, null);
    }
}