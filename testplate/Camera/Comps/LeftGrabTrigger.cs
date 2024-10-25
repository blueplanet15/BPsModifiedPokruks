using UnityEngine;

#pragma warning disable CS0618
namespace CameraMod.Camera.Comps {
    internal class LeftGrabTrigger : MonoBehaviour {
        private Transform leftHandT => GorillaTagger.Instance.leftHandTransform;
        private CameraController controller => CameraController.Instance;
        private Transform tabletT => controller.CameraTablet.transform;
        
        private void Start() {
            gameObject.layer = 18;
        }

        private void OnTriggerStay(Collider col) {
            if (col.name.Contains("Left"))
                if (InputManager.instance.LeftGrip & !controller.fpv) {
                    tabletT.parent = leftHandT;
                    if (controller.fp) controller.fp = false;
                }

            if (!InputManager.instance.LeftGrip & (tabletT.parent == leftHandT))
                tabletT.parent = null;
        }
    }
}