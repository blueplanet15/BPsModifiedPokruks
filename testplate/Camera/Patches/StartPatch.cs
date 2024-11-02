using HarmonyLib;
using System;
using UnityEngine;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public class StartPatch {
        public static void Postfix() {
            Console.WriteLine("YA HUY");
            new GameObject().AddComponent<CameraController>();
            CameraController.Instance.Init();
        }
    }
}