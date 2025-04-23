using GorillaNetworking;
using HarmonyLib;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(CosmeticItemRegistry), nameof(CosmeticItemRegistry.Initialize))]
    public class CosmeticsLoaded {
        public static void Postfix(CosmeticItemRegistry __instance) {
            CameraController.Instance.InitCosmeticsHider();
        }
    }
}
