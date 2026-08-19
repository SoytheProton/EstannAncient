using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace EstannAncient.EstannAncientCode;

[ModInitializer(nameof(Initialize))]
public partial class EstannAncientMainFile : Node
{
    public const string ModId = "EstannAncient"; //Used for resource filepath

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}