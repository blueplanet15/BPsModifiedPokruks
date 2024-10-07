using UnityEngine;
using UnityEngine.Rendering;

namespace YizziCamModV2 {
    internal abstract class FromOtherCamerasHider : MonoBehaviour {
        public Camera cam;

        public void Start() {
            cam = GetComponent<Camera>();
        }

        public void OnEnable() {
            RenderPipelineManager.beginFrameRendering += onBeginFrameRendering;
            RenderPipelineManager.beginCameraRendering += onBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += onEndCameraRendering;
            RenderPipelineManager.endFrameRendering += onEndFrameRendering;
        }

        public void OnDisable() {
            RenderPipelineManager.beginFrameRendering -= onBeginFrameRendering;
            RenderPipelineManager.beginCameraRendering -= onBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= onEndCameraRendering;
            RenderPipelineManager.endFrameRendering -= onEndFrameRendering;
        }

        private void onBeginFrameRendering(ScriptableRenderContext context, Camera[] cameras) {
            Hide();
        }

        private void onBeginCameraRendering(ScriptableRenderContext context, Camera camera) {
            if (cam == camera) Show();
        }

        private void onEndCameraRendering(ScriptableRenderContext context, Camera camera) {
            if (cam == camera) Hide();
        }

        private void onEndFrameRendering(ScriptableRenderContext context, Camera[] cameras) {
            Show();
        }

        public abstract void Hide();
        public abstract void Show();
    }
}