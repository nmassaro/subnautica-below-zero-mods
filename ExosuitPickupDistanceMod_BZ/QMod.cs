using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;
// ### Enhancing the mod ###
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Handlers;

namespace ExosuitPickupDistanceMod_BZ
{
    [QModCore]
    public static class QMod
    { 
        // ### Enhancing the mod ###
        // Set up an instance of the Config class, allowing the player to configure your mod
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
    /// ### Enhancing the mod ###
    /// Set up the Mod menu
    /// </summary>
    [Menu("Exosuit Pickup Distance")]
    public class Config: ConfigFile
    {
        /// <summary>
        /// Slider element for float value of the modifiers. We'll allow 1.0 (unchanged) to 5.0 (death bringer).
        /// Default to 1.0;
        /// </summary>

        [Slider("Exosuit pickup distance modifier", Format = "{0:F2}", Min = 1.0F, Max = 5.0F, DefaultValue = 1.0F, Step = 0.1F)]
        public float ExosuitPickupDistanceModifier = 1.0F;
    }
}
