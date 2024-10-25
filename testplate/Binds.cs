using System.Collections.Generic;
using CameraMod.Camera.Comps;

namespace CameraMod {
    public class Binds {
        private static HashSet<string> bindAliases = new HashSet<string>();
        
        public static void Init() {
            foreach (var bindAlias in Configs.controls.CurrentSettings.activateBind.Split(" ")) {
                bindAliases.Add(bindAlias);
            }
            Configs.controls.Changed += (cfg) => {
                bindAliases.Clear();
                foreach (var bindAlias in cfg.activateBind.Split(" ")) {
                    bindAliases.Add(bindAlias);
                }
            };
        }
        
        public static bool Tablet() {
            var inputManager = InputManager.instance;
            foreach (var bindAlias in bindAliases) {
                if (bindAlias == "RP" && inputManager.RightPrimaryButton)
                    return true;
                if (bindAlias == "LP" && inputManager.LeftPrimaryButton)
                    return true;
                if (bindAlias == "RS" && inputManager.RightSecondaryButton)
                    return true;
                if (bindAlias == "LS" && inputManager.LeftSecondaryButton)
                    return true;
            }

            return false;
        } 
    }
}