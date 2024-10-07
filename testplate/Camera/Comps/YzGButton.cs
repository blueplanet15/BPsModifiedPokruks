using UnityEngine;

#pragma warning disable CS0618
namespace YizziCamModV2.Comps {
    internal class YzGButton : MonoBehaviour {
        private void Start() {
            gameObject.layer = 18;
        }

        private void OnEnable() {
            Invoke("ButtonTimer", 1f);
        }

        private void OnDisable() {
            CameraController.Instance.canbeused = false;
        }

        private void OnTriggerEnter(Collider col) {
            var controller = CameraController.Instance;
            if (controller.canbeused &&
                (col.name == "RightHandTriggerCollider") | (col.name == "LeftHandTriggerCollider")) {
                CameraController.Instance.canbeused = false;
                Invoke("ButtonTimer", 1f);
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
                    case "SmoothingDownButton":
                        controller.smoothing -= 0.01f;
                        if (controller.smoothing < 0.01f) controller.smoothing = 0.11f;
                        controller.SmoothText.text = controller.smoothing.ToString();
                        controller.canbeused = true;
                        break;
                    case "SmoothingUpButton":
                        controller.smoothing += 0.01f;
                        if (controller.smoothing > 0.11f) controller.smoothing = 0.01f;
                        controller.SmoothText.text = controller.smoothing.ToString();
                        controller.canbeused = true;
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
                        controller.TabletCamera.nearClipPlane -= 0.01f;
                        if (controller.TabletCamera.nearClipPlane < 0.01) {
                            controller.TabletCamera.nearClipPlane = 1f;
                            controller.ThirdPersonCamera.nearClipPlane = 1f;
                        }

                        controller.ThirdPersonCamera.nearClipPlane = controller.TabletCamera.nearClipPlane;
                        controller.NearClipText.text = controller.TabletCamera.nearClipPlane.ToString();
                        controller.canbeused = true;
                        break;
                    case "NearClipUp":
                        controller.TabletCamera.nearClipPlane += 0.01f;
                        if (controller.TabletCamera.nearClipPlane > 1.0) {
                            controller.TabletCamera.nearClipPlane = 0.01f;
                            controller.ThirdPersonCamera.nearClipPlane = 0.01f;
                        }

                        controller.ThirdPersonCamera.nearClipPlane = controller.TabletCamera.nearClipPlane;
                        controller.NearClipText.text = controller.TabletCamera.nearClipPlane.ToString();
                        controller.canbeused = true;
                        break;
                    case "FPButton":
                        controller.fp = !controller.fp;
                        break;
                    case "MinDistDownButton":
                        controller.minDist -= 0.1f;
                        if (controller.minDist < 1) controller.minDist = 1;
                        controller.MinDistText.text = controller.minDist.ToString();
                        controller.canbeused = true;
                        break;
                    case "MinDistUpButton":
                        controller.minDist += 0.1f;
                        if (controller.minDist > 10) controller.minDist = 10;
                        controller.MinDistText.text = controller.minDist.ToString();
                        controller.canbeused = true;
                        break;
                    case "SpeedUpButton":
                        controller.fpspeed += 0.01f;
                        if (controller.fpspeed > 0.1) controller.fpspeed = 0.1f;
                        controller.SpeedText.text = controller.fpspeed.ToString();
                        controller.canbeused = true;
                        break;
                    case "SpeedDownButton":
                        controller.fpspeed -= 0.01f;
                        if (controller.fpspeed < 0.01) controller.fpspeed = 0.01f;
                        controller.SpeedText.text = controller.fpspeed.ToString();
                        controller.canbeused = true;
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
        }

        private void ButtonTimer() {
            if (!enabled) CameraController.Instance.canbeused = false;
            CameraController.Instance.canbeused = true;
        }
    }
}