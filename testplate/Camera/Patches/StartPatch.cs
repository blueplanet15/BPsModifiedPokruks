using System.Collections.Generic;
using HarmonyLib;
using PokrukMenu.Classes;
using UnityEngine;
using static PokrukMenu.Menu.Buttons;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(GorillaTagger))]
    [HarmonyPatch("Start", MethodType.Normal)]
    internal class StartPatch {
        private static void Postfix() {
            new GameObject().AddComponent<CameraController>();
            CameraController.Instance.YizziStart();

            var headCosmeticsHider = CameraController.Instance.ThirdPersonCameraGO.AddComponent<HeadCosmeticsHider>();
            headCosmeticsHider.enabled = false;

            Pages[PageNames.MainMods].Add(transferButton("Camera", "CameraMod", ""));
            Pages.Add(
                "Camera",
                new List<ButtonInfo> {
                    toMainButton,
                    new ButtonInfo {
                        isTogglable = true,
                        buttonText = "Bind Enabled",
                        enableMethod = () => { CameraController.bindEnabled = true; },
                        disableMethod = () => { CameraController.bindEnabled = false; },
                        enabled = CameraController.bindEnabled,
                        doSave = true
                    },
                    new ButtonInfo {
                        isTogglable = false,
                        buttonText = "Update mode",
                        method = () => { CameraController.UpdateMode = UpdateMode.Update; }
                    },
                    new ButtonInfo {
                        isTogglable = false,
                        buttonText = "LateUpdate mode",
                        method = () => { CameraController.UpdateMode = UpdateMode.LateUpdate; }
                    },
                    new ButtonInfo {
                        isTogglable = false,
                        buttonText = "Patch mode",
                        method = () => { CameraController.UpdateMode = UpdateMode.Patch; }
                    },
                    toToggleButton("Hide Head Cosmetics",
                        () => headCosmeticsHider.enabled,
                        val => headCosmeticsHider.enabled = val)
                });
        }
    }
}