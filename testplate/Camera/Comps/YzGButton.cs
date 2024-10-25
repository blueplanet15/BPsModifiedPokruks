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

        public void Click() {
            var controller = CameraController.Instance;
            
            onClick?.Invoke();
            switch (name) {
                case "BackButton":
                    controller.MainPage.SetActive(true);
                    controller.MiscPage.SetActive(false);
                    break;
                case "ControlsButton":
                    if (!controller.openedurl) {
                        Application.OpenURL("https://github.com/Yizzii/YizziCamModV2#controls");
                        controller.openedurl = true;
                    }
                    break;
                case "TPVButton":
                    if (controller.TPVMode == CameraController.TPVModes.BACK) {
                        if (controller.flipped) {
                            controller.flipped = false;
                            controller.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            controller.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            controller.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        }
                    } else if (controller.TPVMode == CameraController.TPVModes.FRONT) {
                        if (!controller.flipped) {
                            controller.flipped = true;
                            controller.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            controller.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            controller.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        }
                    }

                    controller.fp = false;
                    controller.fpv = false;
                    controller.tpv = true;
                    break;
                case "FPVButton":
                    if (controller.flipped) {
                        controller.flipped = false;
                        controller.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        controller.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        controller.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                    }

                    controller.fp = false;
                    controller.fpv = true;
                    break;
                case "FlipCamButton":
                    controller.flipped = !controller.flipped;
                    controller.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                    controller.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                    controller.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                    break;
                case "FovDown":
                    CameraController.ChangeFov(-5);
                    break;
                case "FovUP":
                    CameraController.ChangeFov(5);
                    break;
                case "MiscButton":
                    controller.MainPage.SetActive(false);
                    controller.MiscPage.SetActive(true);
                    break;
                case "NearClipDown":
                    CameraController.ChangeNearClip(-0.01f);
                    break;
                case "NearClipUp":
                    CameraController.ChangeNearClip(0.01f);
                    break;
                case "FPButton":
                    controller.fp = !controller.fp;
                    break;
                case "MinDistDownButton":
                    controller.minDist -= 0.1f;
                    if (controller.minDist < 1) controller.minDist = 1;
                    controller.MinDistText.text = controller.minDist.ToString();
                    break;
                case "MinDistUpButton":
                    controller.minDist += 0.1f;
                    if (controller.minDist > 10) controller.minDist = 10;
                    controller.MinDistText.text = controller.minDist.ToString();
                    break;
                case "SpeedUpButton":
                    controller.fpspeed += 0.01f;
                    if (controller.fpspeed > 0.1) controller.fpspeed = 0.1f;
                    controller.SpeedText.text = controller.fpspeed.ToString();
                    break;
                case "SpeedDownButton":
                    controller.fpspeed -= 0.01f;
                    if (controller.fpspeed < 0.01) controller.fpspeed = 0.01f;
                    controller.SpeedText.text = controller.fpspeed.ToString();
                    break;
                case "TPModeDownButton":
                    if (controller.TPVMode == CameraController.TPVModes.BACK)
                        controller.TPVMode = CameraController.TPVModes.FRONT;
                    else
                        controller.TPVMode = CameraController.TPVModes.BACK;
                    controller.TPText.text = controller.TPVMode.ToString();
                    break;
                case "TPModeUpButton":
                    if (controller.TPVMode == CameraController.TPVModes.BACK)
                        controller.TPVMode = CameraController.TPVModes.FRONT;
                    else
                        controller.TPVMode = CameraController.TPVModes.BACK;
                    controller.TPText.text = controller.TPVMode.ToString();
                    break;
                case "TPRotButton":
                    controller.followheadrot = !controller.followheadrot;
                    controller.TPRotText.text = controller.followheadrot.ToString().ToUpper();
                    break;
                case "TPRotButton1":
                    controller.followheadrot = !controller.followheadrot;
                    controller.TPRotText.text = controller.followheadrot.ToString().ToUpper();
                    break;
                case "GreenScreenButton":
                    controller.ColorScreenGO.active = !controller.ColorScreenGO.active;
                    if (controller.ColorScreenGO.active)
                        controller.ColorScreenText.text = "(ENABLED)";
                    else
                        controller.ColorScreenText.text = "(DISABLED)";
                    break;
                case "RedButton":
                    foreach (var mat in controller.ScreenMats) mat.color = Color.red;
                    break;
                case "GreenButton":
                    foreach (var mat in controller.ScreenMats) mat.color = Color.green;
                    break;
                case "BlueButton":
                    foreach (var mat in controller.ScreenMats) mat.color = Color.blue;
                    break;
            }
        }

        public void Vibration(bool isLeftHand) {
            GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 4f);
        }

        private void OnTriggerStay(Collider col) {
            if (!isHoldable) return;
            if (col.name != "RightHandTriggerCollider" && col.name != "LeftHandTriggerCollider") return;

            if (isHolding && (Time.time - lastHoldTick) >  holdTickInterval) {
                Click();
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
            Click();
            lastClicked = Time.time;
        }
    }
}