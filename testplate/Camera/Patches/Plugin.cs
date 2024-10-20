using System;
using System.ComponentModel;
using BepInEx;

namespace CameraMod.Camera.Patches {
    [Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin {
        public void Start() {
            Console.Title = $"{PluginInfo.Name} // Build " + PluginInfo.Version;
        }

        public void OnEnable() {
            HarmonyPatcher.ApplyHarmonyPatches();
        }

        public void OnDisable() {
            HarmonyPatcher.RemoveHarmonyPatches();
        }
    }
}