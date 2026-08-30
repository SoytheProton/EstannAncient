using BaseLib.Utils;
using EstannAncient.EstannAncientCode.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;

namespace EstannAncient.EstannAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class NameTwo : EstannAncientRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool _wasUsed;

    public override bool HasUponPickupEffect => true;

    public bool WasUsed
    {
        get => _wasUsed;
        set
        {
            AssertMutable();
            _wasUsed = value;
            Status = value ? RelicStatus.Normal : RelicStatus.Active;
            InvokeDisplayAmountChanged();
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("EnchantmentName", ModelDb.Enchantment<TwoEnchantment>().Title.GetFormattedText()), new CardsVar(5), new ("Power", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..HoverTipFactory.FromEnchantment<TwoEnchantment>(), HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<DexterityPower>()];

    public override async Task AfterObtained()
    {
        var enchantment = ModelDb.Enchantment<TwoEnchantment>();
        var list = PileType.Deck.GetPile(Owner).Cards.Where(enchantment.CanEnchant).ToList();
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, DynamicVars.Cards.IntValue);
        foreach (var card in await CardSelectCmd.FromDeckForEnchantment(list.UnstableShuffle(Owner.RunState.Rng.Niche).ToList(), enchantment, 1, prefs))
        {
            CardCmd.Enchant<TwoEnchantment>(card, 1M);
            var child = NCardEnchantVfx.Create(card);
            if (child == null)
                continue;
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(child);
        }
    }
    
    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        if(card.Owner != Owner || Owner.PlayerCombatState == null)
            return;
        if (Owner.PlayerCombatState.AllCards.Any(c => c is { Pile.Type: not PileType.Exhaust, Enchantment: TwoEnchantment }))
            return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["Power"].IntValue, null, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars["Power"].IntValue, null, null);
        WasUsed = true;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        WasUsed = false;
        return Task.CompletedTask;
    }

    
    protected override bool RelicAllowedToSpawn(Player owner)
    {
        return PileType.Deck.GetPile(owner).Cards.Any(ModelDb.Enchantment<TwoEnchantment>().CanEnchant);
    }
}