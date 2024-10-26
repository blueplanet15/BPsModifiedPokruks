using System;
using CameraMod.Camera;
using CameraMod.Camera.Comps;
using UnityEngine;

namespace CameraMod.Button.Buttons {
    public class HoldableButton : BaseButton {
        private float lastClicked;
        private float lastHoldTick;
        private float holdTickInterval = 0.1f;
        
        public float clickMinInterval = 0.1f;
        public float enterExitTapMaxDelay = 0.3f;
        
        public float entered = Time.time;
        public bool isHolding => collidersCount > 0 && (Time.time - entered) > enterExitTapMaxDelay;
        private int collidersCount = 0;
        private void OnTriggerEnter(Collider col) {
            if (CameraController.Instance.ButtonsTimeouted) return;
            
            if (!isHand(col)) {
                return;
            }
            collidersCount += 1;
            entered = Time.time;
        }

        private void OnTriggerStay(Collider col) {
            if (CameraController.Instance.ButtonsTimeouted) return;
            
            if (!isHand(col)) return;

            if (isHolding && (Time.time - lastHoldTick) >  holdTickInterval) {
                onClick?.Invoke();
                Vibration(isLeft(col));
                lastHoldTick = Time.time;
            }
        }

        private void OnTriggerExit(Collider col) {
            if (CameraController.Instance.ButtonsTimeouted) return;
            
            if (!isHand(col)) {
                return;
            }
            collidersCount -= 1;
            if (collidersCount < 0) {
                collidersCount = 0;
            }
                
            var fromLastClicked = Time.time - lastClicked;
            if (clickMinInterval > fromLastClicked) {
                return;
            }
            if (enterExitTapMaxDelay < Time.time - entered) {
                return;
            }

            Vibration(isLeft(col));
            onClick?.Invoke();
            lastClicked = Time.time;
        }
    }
}