using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace EstannAncient.EstannAncientCode;

[ModInitializer(nameof(Initialize))]
public partial class EstannAncientMainFile : Node
{
    public const string ModId = "EstannAncient"; //Used for resource filepath

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}