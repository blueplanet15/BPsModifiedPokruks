using GorillaLocomotion;
using HarmonyLib;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(Player), "LateUpdate")]
    public class LocomotionPatch {
        public static void Postfix() {
            if (CameraController.UpdateMode == UpdateMode.Patch) CameraController.Instance.AnUpdate();
        }
    }
}