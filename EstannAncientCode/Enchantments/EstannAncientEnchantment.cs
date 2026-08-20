using BaseLib.Abstracts;
using BaseLib.Extensions;
using EstannAncient.EstannAncientCode.Extensions;
using Godot;

namespace EstannAncient.EstannAncientCode.Enchantments;

public abstract class EstannAncientEnchantment : CustomEnchantmentModel
{
    protected override string? CustomIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
            return ResourceLoader.Exists(path) ? path : null;
        }
    }
}