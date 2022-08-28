using HarmonyLib;
using System;
using Logger = QModManager.Utility.Logger;

namespace SeaTruckModuleWeightMod_BZ
{
    public class SeaTruckModuleWeightMod
    {
        [HarmonyPatch(typeof(SeaTruckSegment))]
        [HarmonyPatch(nameof(SeaTruckSegment.GetAttachedWeight))]
        internal class PatchGetAttachedWeight
        {
            [HarmonyPrefix]
            public static bool Prefix(SeaTruckSegment __instance, ref float __result)
            {
                if (QMod.Config.ShouldSeatruckModulesAddWeight)
                {
                    return true; // run original method
                }

                __result = 0;
                return false;
            }

        }

        [HarmonyFinalizer]
        static void Finalizer(Exception __exception)
        {
            Logger.Log(Logger.Level.Error, __exception.Message);
        }
    }
}
