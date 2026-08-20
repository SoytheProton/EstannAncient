using BaseLib.Abstracts;
using BaseLib.Extensions;
using EstannAncient.EstannAncientCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;

namespace EstannAncient.EstannAncientCode.Relics;

public abstract class EstannAncientRelic : CustomRelicModel
{
    //EstannAncient/images/relics
    public override string PackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".RelicImagePath();
        }
    }

    protected override string PackedIconOutlinePath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic_outline.png".RelicImagePath();
        }
    }

    protected override string BigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
            return ResourceLoader.Exists(path) ? path : "relic.png".BigRelicImagePath();
        }
    }
    
    protected virtual bool RelicAllowedToSpawn(Player owner)
    {
        return true;
    }

    protected EstannAncientRelic()
    {
        this.AddCustomAncientSpawnCondition(model => ((EstannAncientRelic)ToMutable()).RelicAllowedToSpawn(model.Owner));
    }
}