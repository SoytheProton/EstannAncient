using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace EstannAncient.EstannAncientCode.Enchantments;

public class TwoEnchantment : EstannAncientEnchantment
{
    public override bool HasExtraCardText => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    public override bool CanEnchant(CardModel card)
    {
        return base.CanEnchant(card) && card.Type is CardType.Attack or CardType.Skill;
    }
    
    public override async Task OnPlay(PlayerChoiceContext context, CardPlay? cardPlay)
    {
        await CardPileCmd.Draw(context, DynamicVars.Cards.IntValue, Card.Owner);
    }
    
    protected override void OnEnchant() => CardCmd.ApplyKeyword(Card, CardKeyword.Exhaust);
}