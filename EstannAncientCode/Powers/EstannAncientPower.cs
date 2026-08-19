using BaseLib.Abstracts;
using BaseLib.Extensions;
using EstannAncient.EstannAncientCode.Extensions;
using Godot;

namespace EstannAncient.EstannAncientCode.Powers;

public abstract class EstannAncientPower : CustomPowerModel
{
    //Loads from EstannAncient/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}