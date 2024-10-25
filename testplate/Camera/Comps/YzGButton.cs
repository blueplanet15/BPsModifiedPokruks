using System;
using UnityEngine;
using UnityEngine.Serialization;

#pragma warning disable CS0618
namespace CameraMod.Camera.Comps {
    internal class YzGButton : MonoBehaviour {
        public Action onClick;
        
        public YzGButton OnClick(Action onClickDelegate) {
            onClick = onClickDelegate;
            return this;
        }

        public bool isHoldable = false;
        public YzGButton MakeHoldable() {
            isHoldable = true;
            return this;
        }
        
        private void Start() {
            gameObject.layer = 18;
        }

        private float lastClicked;
        private float lastHoldTick;
        private float holdTickInterval = 0.1f;
        
        public float clickMinInterval = 0.1f;
        public float enterExitTapMaxDelay = 0.3f;
        
        public float entered = Time.time;
        public bool isHolding => collidersCount > 0 && (Time.time - entered) > enterExitTapMaxDelay;
        private int collidersCount = 0;
        private void OnTriggerEnter(Collider col) {
            if (col.name != "RightHandTriggerCollider" && col.name != "LeftHandTriggerCollider") {
                return;
            }

            collidersCount += 1;
            entered = Time.time;
        }

        public void Vibration(bool isLeftHand) {
            GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 4f);
        }

        private void OnTriggerStay(Collider col) {
            if (!isHoldable) return;
            if (col.name != "RightHandTriggerCollider" && col.name != "LeftHandTriggerCollider") return;

            if (isHolding && (Time.time - lastHoldTick) >  holdTickInterval) {
                onClick?.Invoke();
                var isLeftHand = col.name == "LeftHandTriggerCollider";
                Vibration(isLeftHand);
                lastHoldTick = Time.time;
            }
        }

        private void OnTriggerExit(Collider col) {
            if (col.name != "RightHandTriggerCollider" && col.name != "LeftHandTriggerCollider") {
                return;
            }
            collidersCount -= 1;
            if (collidersCount < 0) {
                collidersCount = 0;
            }
            var isLeftHand = col.name == "LeftHandTriggerCollider";
                
            var fromLastClicked = Time.time - lastClicked;
            if (clickMinInterval > fromLastClicked) {
                return;
            }
            if (enterExitTapMaxDelay < Time.time - entered) {
                return;
            }

            Vibration(isLeftHand);
            onClick?.Invoke();
            lastClicked = Time.time;
        }
    }
}