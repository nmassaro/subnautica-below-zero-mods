using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using System.Reflection;
using Logger = QModManager.Utility.Logger;


namespace SeaTruckModuleWeightMod_BZ
{
    [QModCore]
    public static class QMod
    {
        internal static Config Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();


        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modName = ($"nkrista_{assembly.GetName().Name}");

            Logger.Log(Logger.Level.Info, $"Patching {modName}");

            Harmony harmony = new Harmony(modName);
            harmony.PatchAll(assembly);

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }

    /// <summary>
    /// Set up mod options in menu
    /// </summary>
    [Menu("Seatruck Module Weight")]
    public class Config : ConfigFile
    {
        [ToggleAttribute("Seatruck modules add weight")]
        public bool ShouldSeatruckModulesAddWeight = false;
    }
}
