using GorillaLocomotion;
using HarmonyLib;
using YizziCamModV2;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(Player), "LateUpdate")]
    public class LocomotionPatch {
        public static void Postfix() {
            if (CameraController.UpdateMode == UpdateMode.Patch) CameraController.Instance.AnUpdate();
        }
    }
}