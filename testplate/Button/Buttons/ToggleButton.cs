using System;
using CameraMod.Camera;
using UnityEngine;

namespace CameraMod.Button.Buttons {
    public class ToggleButton : BaseButton {
        private Vector3 upPosition;
        private Vector3 downPosition;
        
        private Func<bool> getter;
        private Action<bool> setter;
        private bool savable;
        private string saveKey;
        public ToggleButton InitToggleButton(Action<bool> setter, Func<bool> getter, bool savable = true, string overrideSaveName = null) {
            material = GetComponent<Renderer>().material;
            
            upPosition = transform.localPosition;
            downPosition = transform.localPosition + new Vector3(0.02f, 0, 0);
            
            this.getter = getter;
            this.setter = setter;
            this.savable = savable;
            
            saveKey = overrideSaveName ?? name;
            
            if (savable) {
                var save = PlayerPrefs.GetInt(saveKey, -1);
                if (save != -1) {
                    this.setter(save == 1);
                }
            }
            
            UpdateAppearance(getter());
            
            return this;
        }

        public bool IsDown { 
            get => getter(); 
            set {
                setter(value);
                UpdateAppearance(value);
                if (savable) {
                    PlayerPrefs.SetInt(saveKey, value ? 1 : 0);
                }
            } 
        }
        
        public Color enabledColor = new Color(1f, 0.9f, 0.9f);
        public Color disabledColor = new Color(0.9f, 1f, 0.9f);
        
        private Material material;

        private void UpdateAppearance(bool isDown) {
            material.color = isDown ? enabledColor : disabledColor;
            transform.localPosition = isDown ? downPosition : upPosition;
        }
        
        private float lastClicked;
        public float clickMinInterval = 0.1f;
        
        private void OnTriggerEnter(Collider col) {
            if (CameraController.Instance.ButtonsTimeouted) return;
            
            if (!isHand(col)) {
                return;
            }

            if (Time.time - lastClicked > clickMinInterval) {
                Vibration(isLeft(col));
                IsDown = !IsDown;
                onClick?.Invoke();

                lastClicked = Time.time;
            }
        }
    }
}
