using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace EstannAncient.EstannAncientCode.Enchantments;

public class EightEnchantment : EstannAncientEnchantment
{
    public override bool HasExtraCardText => true;
    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && card.Type is CardType.Attack or CardType.Skill && !card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust);
    }
    
    public override CardLocation ModifyCardPlayResultLocation(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        CardLocation location)
    {
        if (card != Card || location.pileType != PileType.Discard)
            return location;
        location.pileType = PileType.Hand;
        return location;
    }
    
    public override Task OnPlay(PlayerChoiceContext context, CardPlay? cardPlay)
    {
        Card.EnergyCost.AddThisTurn(1);
        return Task.CompletedTask;
    }
    
    protected override void OnEnchant() => CardCmd.ApplyKeyword(Card, CardKeyword.Retain);
}