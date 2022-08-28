using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;
using Logger = QModManager.Utility.Logger;

namespace ExosuitPickupDistanceMod_BZ
{
    public class ExosuitPickupDistanceMod
    {
        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch(nameof(Exosuit.UpdateActiveTarget))]
        internal class PatchUpdateActiveTarget
        {
            [HarmonyPrefix]
            public static bool Prefix(Exosuit __instance)
            {
                __instance.gameObject.EnsureComponent<ExosuitTargeting>().SetIsUpdatingActiveTarget(true);

                Logger.Log(Logger.Level.Debug, "isUpdatingActiveTarget: true");
                return true; // allow original method to run
            }

            [HarmonyPostfix]
            public static void Postfix(Exosuit __instance)
            {
                Logger.Log(Logger.Level.Debug, "isUpdatingActiveTarget: false");
                __instance.gameObject.EnsureComponent<ExosuitTargeting>().SetIsUpdatingActiveTarget(false);
            }
        }

        [HarmonyPatch(typeof(Targeting), nameof(Targeting.GetTarget), new Type[] { typeof(GameObject), typeof(float), typeof(GameObject), typeof(float) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out })]
        internal class PatchGetTarget
        {
            [HarmonyPrefix]
            public static bool Prefix(ref GameObject ignoreObj, ref float maxDistance)
            {
                Player player = ignoreObj.GetComponentInChildren<Player>();

                Logger.Log(Logger.Level.Debug, $"activeTarget maxDistance: {maxDistance}");

                if (player != null && player.IsInClawExosuit())
                {
                    Exosuit exosuit = player.currentMountedVehicle as Exosuit;
                    bool isUpdatingActiveTarget = exosuit.gameObject.EnsureComponent<ExosuitTargeting>().GetIsUpdatingActiveTarget();

                    if (isUpdatingActiveTarget)
                    {
                        maxDistance *= QMod.Config.ExosuitPickupDistanceModifier;
                    }
                }

                return true;
            }

        }

        static void Finalizer(Exception __exception)
        {
            Logger.Log(Logger.Level.Error, __exception.Message);
        }


    }

    public class ExosuitTargeting: MonoBehaviour
    {
        private bool isUpdatingActiveTarget = false;

        public bool GetIsUpdatingActiveTarget()
        {
            return this.isUpdatingActiveTarget;
        }

        public void SetIsUpdatingActiveTarget(bool isUpdatingActiveTarget)
        {
            this.isUpdatingActiveTarget = isUpdatingActiveTarget;
        }
    }
}
