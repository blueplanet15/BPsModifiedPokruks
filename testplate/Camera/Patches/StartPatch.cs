using HarmonyLib;
using UnityEngine;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(GorillaTagger))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class StartPatch {
        private static void Postfix() {
            new GameObject().AddComponent<CameraController>();
            CameraController.Instance.Init();
        }
    }
}