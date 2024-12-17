using System;
using CameraMod.Camera;
using CameraMod.Camera.Comps;
using UnityEngine;

namespace CameraMod.Button.Buttons {
    public class ClickButton : BaseButton {
        private float lastClicked;
        public float clickMinInterval = 0.1f;
        
        private void OnTriggerEnter(Collider col) {
            if (CameraController.Instance.ButtonsTimeouted) return;
            
            if (!isHand(col)) {
                return;
            }

            if (Time.time - lastClicked > clickMinInterval) {
                Vibration(isLeft(col));
                onClick?.Invoke();

                lastClicked = Time.time;
            }
        }
    }
}