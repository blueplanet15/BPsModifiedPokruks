using System;
using System.Collections.Generic;
using System.Reflection;
using CameraMod.Button;
using CameraMod.Button.Buttons;
using CameraMod.Camera.Comps;
using CameraMod.Camera.Pages;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;

//using BepInEx;

#pragma warning disable CS0618
namespace CameraMod.Camera {
    public enum UpdateMode {
        Patch,
        Update,
        LateUpdate
    }

    public class CameraController : MonoBehaviour {
        public enum TpvModes {
            Back,
            Front
        }

        public static CameraController Instance;

        public static UpdateMode UpdateMode = UpdateMode.Patch;
        public static bool BindEnabled = true;
        public GameObject cameraTablet;
        public GameObject firstPersonCameraGo;
        public GameObject thirdPersonCameraGo;
        public GameObject cmVirtualCameraGo;
        public GameObject fakeWebCam;
        public GameObject tabletCameraGo;
        public MainPage mainPage;
        public MiscPage miscPage;
        public GameObject cameraFollower;
        public GameObject tpvBodyFollower;
        public GameObject colorScreenGo;
        private readonly List<BaseButton> buttons = new List<BaseButton>();
        public List<BaseButton> colorButtons = new List<BaseButton>();
        public List<Material> screenMats = new List<Material>();
        public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public UnityEngine.Camera tabletCamera;
        public UnityEngine.Camera firstPersonCamera;
        public UnityEngine.Camera thirdPersonCamera;
        public CinemachineVirtualCamera cmVirtualCamera;

        public Text colorScreenText;

        public bool followheadrot = true;
        public bool isFaceCamera;
        public bool tpv;
        public bool fpv = true;
        public bool fp;
        public bool openedurl;
        public float minDist = 2f;
        public float fpspeed = 0.01f;
        public float smoothing = 0.07f;
        public TpvModes tpvMode = TpvModes.Back;
        private float dist;
        private bool init;
        private Vector3 targetPosition;
        private Vector3 velocity = Vector3.zero;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (UpdateMode == UpdateMode.Update) AnUpdate();
        }

        private void LateUpdate() {
            if (UpdateMode == UpdateMode.LateUpdate) AnUpdate();
        }

        public void SetNearClip(float val) {
            var tabletCam = tabletCamera;
            tabletCam.nearClipPlane = val;
            if (tabletCam.nearClipPlane < 0.01) {
                tabletCam.nearClipPlane = 1f;
                thirdPersonCamera.nearClipPlane = 1f;
            }
            if (tabletCam.nearClipPlane > 1.0) {
                tabletCam.nearClipPlane = 0.01f;
                thirdPersonCamera.nearClipPlane = 0.01f;
            }

            thirdPersonCamera.nearClipPlane = tabletCamera.nearClipPlane;
            mainPage.NearClipText.text = tabletCamera.nearClipPlane.ToString("#.##");
        }

        public void ChangeNearClip(float diff) {
            SetNearClip(tabletCamera.nearClipPlane + diff);
            PlayerPrefs.SetFloat("CameraNearClip", tabletCamera.nearClipPlane);
        }

        private const float MIN_SMOOTHING = 0.01f;
        private const float MAX_SMOOTHING = 1f;

        public void SetSmoothing(float val) {
            smoothing = val;
            if (smoothing < MIN_SMOOTHING) smoothing = MIN_SMOOTHING;
            if (smoothing > MAX_SMOOTHING) smoothing = MAX_SMOOTHING;
            mainPage.SmoothText.text = smoothing.ToString("#.##");
        }
        public void ChangeSmoothing(float change) {
            SetSmoothing(smoothing + change);
            PlayerPrefs.SetFloat("CameraSmoothing", smoothing);
        }
        public void ChangeFov(float difference) {
            var controller = Instance;

            var max = 130;
            var min = 20;

            var newFov = Mathf.Clamp(controller.tabletCamera.fieldOfView + difference, min, max);
            SetFov(newFov);
            PlayerPrefs.SetInt("CameraFov", (int) newFov);
        }

        public void SetFov(float fov) {
            var newFov = fov;
            tabletCamera.fieldOfView = newFov;
            thirdPersonCamera.fieldOfView = newFov;
            mainPage.FOVText.text = tabletCamera.fieldOfView.ToString("#.##");
        }
        
        public void Init() {
            var tagger = GorillaTagger.Instance;
            
            gameObject.AddComponent<InputManager>().gameObject.AddComponent<UI>();
            var assetsPath = Assembly.GetExecutingAssembly().GetName().Name + ".Camera.Assets";
            Debug.Log(assetsPath);
            colorScreenGo = LoadBundle("ColorScreen", assetsPath + ".colorscreen");
            cameraTablet = LoadBundle("CameraTablet", assetsPath + ".yizzicam");
            firstPersonCameraGo = tagger.mainCamera;

            thirdPersonCameraGo = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
            cmVirtualCameraGo = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera/CM vcam1");
            tpvBodyFollower = tagger.bodyCollider.gameObject;
            cmVirtualCamera = cmVirtualCameraGo.GetComponent<CinemachineVirtualCamera>();
            firstPersonCamera = firstPersonCameraGo.GetComponent<UnityEngine.Camera>();


            thirdPersonCamera = thirdPersonCameraGo.GetComponent<UnityEngine.Camera>();

            cameraTablet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            cameraFollower =
                GameObject.Find(
                    "Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera/Camera Follower");


            tabletCameraGo = GameObject.Find("CameraTablet(Clone)/Camera");
            tabletCamera = tabletCameraGo.GetComponent<UnityEngine.Camera>();

            fakeWebCam = GameObject.Find("CameraTablet(Clone)/FakeCamera");
            GameObject.Find("CameraTablet(Clone)/LeftGrabCol").AddComponent<LeftGrabTrigger>();
            GameObject.Find("CameraTablet(Clone)/RightGrabCol").AddComponent<RightGrabTrigger>();
            mainPage = new MainPage(GameObject.Find("CameraTablet(Clone)/MainPage"));
            miscPage = new MiscPage(GameObject.Find("CameraTablet(Clone)/MiscPage"));
            
            RegisterButtons();
            
            cmVirtualCamera.enabled = false;
            thirdPersonCameraGo.transform.SetParent(cameraTablet.transform, true);
            cameraTablet.transform.position = new Vector3(-65, 12, -82);
            var tabletT = tabletCamera.transform;
            thirdPersonCameraGo.transform.position = tabletT.position;
            thirdPersonCameraGo.transform.rotation = tabletT.rotation;
            cameraTablet.transform.Rotate(0, 180, 0);

            colorScreenText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/ColorScreenText")
                .GetComponent<Text>();
            
            
            void SetColor(Color color) {
                foreach (var mat in screenMats) mat.color = color;
            }
            
            colorButtons.Add(Button("ColorScreen(Clone)/Stuff/RedButton", () => {
                SetColor(Color.red);
            }));
            colorButtons.Add(Button("ColorScreen(Clone)/Stuff/GreenButton", () => {
                SetColor(Color.green);
            }));
            colorButtons.Add(Button("ColorScreen(Clone)/Stuff/BlueButton", () => {
                SetColor(Color.blue);
            }));
            
            new [] {
                "ColorScreen(Clone)/Screen1",
                "ColorScreen(Clone)/Screen2",
                "ColorScreen(Clone)/Screen3"
            }.ForEach(screenMatPath=>
                screenMats.Add(
                    GameObject.Find(screenMatPath)
                    .GetComponent<MeshRenderer>()
                    .material)
            );

            new[] {
                "CameraTablet(Clone)/FakeCamera",
                "CameraTablet(Clone)/Tablet",
                "CameraTablet(Clone)/Handle",
                "CameraTablet(Clone)/Handle2"
            }.ForEach(meshPath => {
                meshRenderers.Add(GameObject.Find(meshPath).GetComponent<MeshRenderer>());
            });

            colorScreenGo.transform.position = new Vector3(-54.3f, 16.21f, -122.96f);
            colorScreenGo.transform.Rotate(0, 30, 0);
            colorScreenGo.SetActive(false);
            miscPage.GO.SetActive(false);
            thirdPersonCamera.nearClipPlane = 0.1f;
            tabletCamera.nearClipPlane = 0.1f;
            fakeWebCam.transform.Rotate(-180, 180, 0);
            init = true;
            
            var fov = PlayerPrefs.GetInt("CameraFov", 100);
            SetFov(fov);
            
            var newSmoothing = PlayerPrefs.GetFloat("CameraSmoothing", 0.07f);
            SetSmoothing(newSmoothing);
            Binds.Init();
        }
        
        public void Flip() {
            isFaceCamera = !isFaceCamera;
            thirdPersonCameraGo.transform.Rotate(0.0f, 180f, 0.0f);
            tabletCameraGo.transform.Rotate(0.0f, 180f, 0.0f);
            fakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
        }

        private float lastPageChangedTime;
        private readonly float pageChangeButtonsTimeout = 0.2f;
        public bool ButtonsTimeouted => Time.time - lastPageChangedTime < pageChangeButtonsTimeout;

        public void EnableFPV() {
            if (isFaceCamera) {
                Flip();
            }

            fp = false;
            fpv = true;
            UI.Instance.freecam = false;
        }
        
        public void RegisterButtons() {
            AddTabletButton("MainPage/MiscButton", () => {
                mainPage.GO.SetActive(false);
                miscPage.GO.SetActive(true);
                lastPageChangedTime = Time.time;
            });
            AddTabletButton("MiscPage/BackButton", () => {
                mainPage.GO.SetActive(true);
                miscPage.GO.SetActive(false);
                lastPageChangedTime = Time.time;
            });
            
            AddTabletButton("MainPage/FPVButton", () => {
                EnableFPV();
            });
            
            AddHoldableTabletButton("MainPage/SmoothingDownButton", () => ChangeSmoothing(-0.01f));
            AddHoldableTabletButton("MainPage/SmoothingUpButton", () => ChangeSmoothing(0.01f));
            
            AddTabletButton("MainPage/FovUP", () => ChangeFov(5));
            AddTabletButton("MainPage/FovDown", () => ChangeFov(-5));
            
            AddTabletButton("MainPage/NearClipUp", () => ChangeNearClip(0.01f));
            AddTabletButton("MainPage/NearClipDown", () => ChangeNearClip(-0.01f));
            
            AddTabletButton("MainPage/FlipCamButton", () => {
                Flip();
            });
            
            AddTabletButton("MainPage/FPButton", () => fp = !fp);
            
            AddTabletButton("MainPage/ControlsButton", () => {
                if (!openedurl) {
                    Application.OpenURL("https://github.com/Yizzii/YizziCamModV2#controls");
                    openedurl = true;
                }
            });
            AddTabletButton("MainPage/TPVButton", () => {
                if (tpvMode == TpvModes.Back) {
                    if (isFaceCamera) {
                        Flip();
                    }
                } else if (tpvMode == TpvModes.Front) {
                    if (!isFaceCamera) {
                        Flip();
                    }
                }

                fp = false;
                fpv = false;
                tpv = true;
            });
            
            AddTabletButton("MiscPage/MinDistDownButton", () => {
                minDist -= 0.1f;
                if (minDist < 1) minDist = 1;
                miscPage.MinDistText.text = minDist.ToString("#.##");
            });
            AddTabletButton("MiscPage/MinDistUpButton", () => {
                minDist += 0.1f;
                if (minDist > 10) minDist = 10;
                miscPage.MinDistText.text = minDist.ToString("#.##");
            });
            AddTabletButton("MiscPage/SpeedUpButton", () => {
                fpspeed += 0.01f;
                if (fpspeed > 0.1) fpspeed = 0.1f;
                miscPage.SpeedText.text = fpspeed.ToString("#.##");
            });
            AddTabletButton("MiscPage/SpeedDownButton", () => {
                fpspeed -= 0.01f;
                if (fpspeed < 0.01) fpspeed = 0.01f;
                miscPage.SpeedText.text = fpspeed.ToString("#.##");
            });
            AddTabletButton("MiscPage/TPModeDownButton", () => {
                if (tpvMode == TpvModes.Back)
                    tpvMode = TpvModes.Front;
                else
                    tpvMode = TpvModes.Back;
                miscPage.TpText.text = tpvMode.ToString();
            });
            AddTabletButton("MiscPage/TPModeUpButton", () => {
                if (tpvMode == TpvModes.Back)
                    tpvMode = TpvModes.Front;
                else
                    tpvMode = TpvModes.Back;
                miscPage.TpText.text = tpvMode.ToString();
            });
            AddTabletButton("MiscPage/TPRotButton", () => {
                followheadrot = !followheadrot;
                miscPage.TpRotText.text = followheadrot.ToString().ToUpper();
            });
            AddTabletButton("MiscPage/TPRotButton1", () => {
                followheadrot = !followheadrot;
                miscPage.TpRotText.text = followheadrot.ToString().ToUpper();
            });
            
            AddTabletButton("MiscPage/GreenScreenButton", () => {
                colorScreenGo.active = !colorScreenGo.active;
                if (colorScreenGo.active)
                    colorScreenText.text = "(ENABLED)";
                else
                    colorScreenText.text = "(DISABLED)";
            });
        }

        public ClickButton Button(string buttonPath, Action onClick) {
            var button = GameObject.Find(buttonPath)
                .AddComponent<ClickButton>();
            button.OnClick(onClick);
            return button;
        }

        public void AddTabletButton(string relativeButtonPath, Action onClick) {
            buttons.Add(Button("CameraTablet(Clone)/" + relativeButtonPath, onClick));
        }
        public void AddHoldableTabletButton(string buttonPath, Action onClick) {
            buttons.Add(
                GameObject.Find("CameraTablet(Clone)/"+buttonPath)
                    .AddComponent<HoldableButton>()
                    .OnClick(onClick)
            );
        }

        public void SetTabletVisibility(bool visible) {
            foreach (var mr in meshRenderers) mr.enabled = visible;
            tabletCamera.enabled = visible;
        }
        
        public static bool NoTiltMode = true;
        public void AnUpdate() {
            if (!init) return;

            if (fpv) {
                if (mainPage.GO.active) {
                    SetTabletVisibility(false);
                    mainPage.GO.active = false;
                }
                var camera = cameraTablet.transform;
                var follower = cameraFollower.transform;
                
                
                camera.position = follower.position;
                
                var newRotation = camera.rotation.Lerped(follower.rotation, smoothing);
                if (NoTiltMode) {
                    newRotation = newRotation.eulerAngles.Scaled(new Vector3(1,1,0)).ToQuaternion();
                }
                camera.rotation = newRotation;
            }

            if (BindEnabled && Binds.Tablet() && cameraTablet.transform.parent == null) {
                fp = false;
                fpv = false;
                tpv = false;
                if (!mainPage.GO.active) {
                    foreach (var btns in buttons) btns.gameObject.SetActive(true);
                    SetTabletVisibility(true);

                    mainPage.GO.active = true;
                }

                var headTransform = Player.Instance.headCollider.transform;
                var headPos = headTransform.position;
                cameraTablet.transform.position = headPos + headTransform.forward;
                cameraTablet.transform.LookAt(headPos);
                cameraTablet.transform.Rotate(0f, -180f, 0f);
            }

            if (fp) {
                cameraTablet.transform.LookAt(2f * cameraTablet.transform.position - cameraFollower.transform.position);
                if (!isFaceCamera) {
                    Flip();
                }

                dist = Vector3.Distance(cameraFollower.transform.position, cameraTablet.transform.position);
                if (dist > minDist)
                    cameraTablet.transform.position = Vector3.Lerp(cameraTablet.transform.position,
                        cameraFollower.transform.position, fpspeed);
            }

            if (tpv) {
                if (mainPage.GO.active) {
                    SetTabletVisibility(false);
                    mainPage.GO.active = false;
                }
                
                switch (tpvMode) {
                    case TpvModes.Back: {
                        if (followheadrot)
                            targetPosition = cameraFollower.transform.TransformPoint(new Vector3(0.3f, 0.1f, -1.5f));
                        else
                            targetPosition = tpvBodyFollower.transform.TransformPoint(new Vector3(0.3f, 0.1f, -1.5f));
                        cameraTablet.transform.position = Vector3.SmoothDamp(cameraTablet.transform.position,
                            targetPosition, ref velocity, 0.1f);
                        cameraTablet.transform.LookAt(cameraFollower.transform.position);
                        break;
                    }
                    case TpvModes.Front: {
                        if (followheadrot)
                            targetPosition = cameraFollower.transform.TransformPoint(new Vector3(0.1f, 0.3f, 2.5f));
                        else
                            targetPosition = tpvBodyFollower.transform.TransformPoint(new Vector3(0.1f, 0.3f, 2.5f));
                        cameraTablet.transform.position = Vector3.SmoothDamp(cameraTablet.transform.position,
                            targetPosition, ref velocity, 0.1f);
                        cameraTablet.transform.LookAt(2f * cameraTablet.transform.position -
                                                      cameraFollower.transform.position);
                        break;
                    }
                }

                if (Binds.Tablet()) {
                    var headT = Player.Instance.headCollider.transform;
                    
                    cameraTablet.transform.position = headT.position + headT.forward;
                    cameraTablet.transform.LookAt(headT.position);
                    cameraTablet.transform.Rotate(0f, -180f, 0f);
                    
                    SetTabletVisibility(true);
                    cameraTablet.transform.parent = null;
                    tpv = false;
                }
            }
        }

        private GameObject LoadBundle(string goname, string resourcename) {
            var str = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcename);
            var asb = AssetBundle.LoadFromStream(str);
            var go = Instantiate(asb.LoadAsset<GameObject>(goname));
            asb.Unload(false);
            str.Close();
            return go;
        }
    }
}