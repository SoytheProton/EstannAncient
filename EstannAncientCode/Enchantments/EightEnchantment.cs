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
    
    public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(
        CardModel card,
        bool isAutoPlay,
        ResourceInfo resources,
        PileType pileType,
        CardPilePosition position)
    {
        if (card != Card || pileType != PileType.Discard)
            return (pileType, position);
        return (PileType.Hand, position);
    }
    
    public override Task OnPlay(PlayerChoiceContext context, CardPlay? cardPlay)
    {
        Card.EnergyCost.AddThisTurn(1);
        return Task.CompletedTask;
    }
    
    protected override void OnEnchant() => CardCmd.ApplyKeyword(Card, CardKeyword.Retain);
}