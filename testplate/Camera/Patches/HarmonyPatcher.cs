using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace CameraMod.Patches {
    public class HarmonyPatcher : MonoBehaviour {
        public static Harmony instance;
        public static bool IsPatched { get; private set; }

        internal static void ApplyHarmonyPatches() {
            if (!IsPatched) {
                if (instance == null) instance = new Harmony(PluginInfo.GUID);
                instance.PatchAll(Assembly.GetExecutingAssembly());

                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches() {
            if (instance != null && IsPatched) IsPatched = false;
        }
    }
}