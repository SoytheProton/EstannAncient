using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace EstannAncient.EstannAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class NameSix : EstannAncientRelic
{
    private bool _hasFreePurchase;
    
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    private bool HasFreePurchase
    {
        get => _hasFreePurchase;
        set
        {
            AssertMutable();
            _hasFreePurchase = value;
            Status = _hasFreePurchase ? RelicStatus.Normal : RelicStatus.Active;
        }
    }

    public override bool HasUponPickupEffect => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Purchase", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    
    // map code courtesy of The Brute because I was not gonna bother figuring out how to do this myself.
    public override Task AfterObtained()
    {
        var runState = Owner.RunState;
        if (runState.Map == null)
            return Task.FromResult(Task.CompletedTask);

        var count = runState.Players.Count(p => p.GetRelic<NameSix>() != null);
        runState.Map = new NameSixMap(count, runState.Map);
        NMapScreen.Instance?.SetMap(runState.Map, runState.Rng.Seed, false);
        Flash();
        return Task.FromResult(Task.CompletedTask);
    }
    
    public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
    {
        var relicCount = runState.Players.Count(player => player.GetRelic<NameSix>() != null);
        if (relicCount <= 0)
            return map;
        
        Flash();
        return new NameSixMap(relicCount, map);
    }
    
    public override decimal ModifyMerchantPrice(
        Player player,
        MerchantEntry entry,
        decimal originalPrice)
    {
        return player != Owner || !HasFreePurchase ? originalPrice : 0;
    }
    
    public override Task AfterItemPurchased(
        Player player,
        MerchantEntry itemPurchased,
        int goldSpent)
    {
        if (player != Owner || !HasFreePurchase)
            return Task.CompletedTask;
        Flash();
        HasFreePurchase = false;
        return Task.CompletedTask;
    }
    
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not MerchantRoom)
        {
            HasFreePurchase = false;
            return Task.CompletedTask;
        }
        Flash();
        HasFreePurchase = true;
        return Task.CompletedTask;
    }
}

internal class NameSixMap : ActMap
    {
        private readonly MapPoint? _secondBoss;

        public NameSixMap(int count, ActMap original)
        {
            var oldRows = original.GetRowCount();
            Grid = new MapPoint?[7, oldRows + count];

            for (var row = 1; row < oldRows; row++)
            {
                for (var column = 0; column < 7; column++)
                {
                    var point = original.GetPoint(column, row);

                    if (point == null)
                    {
                        continue;
                    }

                    Grid[column, row] = point;
                }
            }

            StartingMapPoint = original.StartingMapPoint;

            BossMapPoint = original.BossMapPoint;
            BossMapPoint.coord.row += count;

            _secondBoss = original.SecondBossMapPoint;
            if (_secondBoss != null)
            {
                _secondBoss.coord.row += count;
            }

            var restSites = BossMapPoint.parents.ToList();

            foreach (var restSite in restSites)
            {
                restSite.coord.row += count;

                foreach (var parent in restSite.parents.ToList())
                {
                    parent.RemoveChildPoint(restSite);

                    var previous = parent;

                    for (var i = 1; i <= count; i++)
                    {
                        var shop = new MapPoint(parent.coord.col, parent.coord.row + i)
                        {
                            PointType = MapPointType.Shop,
                            CanBeModified = false
                        };

                        Grid[shop.coord.col, shop.coord.row] = shop;

                        previous.AddChildPoint(shop);

                        previous = shop;
                    }

                    previous.AddChildPoint(restSite);
                }
            }
        }

        public override MapPoint? SecondBossMapPoint => _secondBoss;
        public sealed override MapPoint BossMapPoint { get; }
        public override MapPoint StartingMapPoint { get; }
        protected sealed override MapPoint?[,] Grid { get; }
    }