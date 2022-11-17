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
                SurfaceClimbSettings surfaceClimbSettings = raycastHit.collider.GetComponent<SurfaceClimbSettings>();

                if (surfaceClimbSettings?.Unclimbable ?? false) __result = 1;
                else __result = surfaceClimbSettings?.slipPercentage ?? surface.slipPercentage;

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
