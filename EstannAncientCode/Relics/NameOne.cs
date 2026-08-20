using BaseLib.Utils;
using EstannAncient.EstannAncientCode.Potions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace EstannAncient.EstannAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class NameOne : EstannAncientRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("PotionSlots", 2M), new ("Potions", 2M), new StringVar("PotionName", ModelDb.Potion<FakeBufferPotion>().Title.GetFormattedText())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<FakeBufferPotion>()];

    public override async Task AfterObtained()
    {
        await PlayerCmd.GainMaxPotionCount(DynamicVars["PotionSlots"].IntValue, Owner);
    }
    
    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not RestSiteRoom)
            return;
        Flash();
        for (var i = 0; i < DynamicVars["Potions"].IntValue; i++)
        {
            await PotionCmd.TryToProcure(ModelDb.Potion<FakeBufferPotion>().ToMutable(), Owner);
        }
    }
}