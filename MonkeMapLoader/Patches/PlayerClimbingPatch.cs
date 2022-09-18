using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;
using VmodMonkeMapLoader.Behaviours;

namespace VmodMonkeMapLoader.Patches
{
	[HarmonyPatch(typeof(Player))]
	[HarmonyPatch("GetSlidePercentage", MethodType.Normal)]
	internal class PlayerClimbingPatch
	{
        internal static void Postfix(Player __instance, ref float __result, RaycastHit raycastHit)
        {
            // Check for surface component
            Surface surface = raycastHit.collider.GetComponent<Surface>();
            if (surface != null)
            {
                __instance.currentOverride = __instance.GetComponent<GorillaSurfaceOverride>();
                __result = surface.slipPercentage;

                SurfaceClimbSettings surfaceClimbSettings = raycastHit.collider.GetComponent<SurfaceClimbSettings>();
                if (surfaceClimbSettings != null)
                    __result = surfaceClimbSettings.slipPercentage;
                    if (surfaceClimbSettings.Unclimbable == true)
                        __result = 1;

                return;
            }

            // Check for unreadable mesh
            MeshCollider meshCollider = raycastHit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                __instance.currentOverride = __instance.GetComponent<GorillaSurfaceOverride>();

                __result = __instance.defaultSlideFactor;
                return;
            }

            if (!meshCollider.sharedMesh.isReadable)
            {
                __instance.currentOverride = __instance.GetComponent<GorillaSurfaceOverride>();

                __result = __instance.defaultSlideFactor;
                return;
            }

        }
    }
}
