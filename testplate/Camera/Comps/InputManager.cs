using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace CameraMod.Camera.Comps {
    public class InputManager : MonoBehaviour {
        public static InputManager instance;
        public bool LeftGrip;
        public bool RightGrip;
        public bool RightPrimaryButton; // x

        public bool LeftPrimaryButton;

        // gamepad
        public Vector2 GPLeftStick;
        public Vector2 GPRightStick;
        private XRNode lHandNode = XRNode.LeftHand;
        private XRNode rHandNode = XRNode.RightHand;

        private void Start() {
            instance = this;
        }

        private void Update() {
            LeftGrip = ControllerInputPoller.instance.leftGrab;
            RightGrip = ControllerInputPoller.instance.rightGrab;
            RightPrimaryButton = ControllerInputPoller.instance.rightControllerPrimaryButton;
            LeftPrimaryButton = ControllerInputPoller.instance.leftControllerPrimaryButton;

            if (Gamepad.current != null) {
                GPLeftStick = Gamepad.current.leftStick.ReadValue();
                GPRightStick = Gamepad.current.rightStick.ReadValue();
            }
        }
    }
}