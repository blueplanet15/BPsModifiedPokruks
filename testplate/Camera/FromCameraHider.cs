using UnityEngine;
using UnityEngine.Rendering;

namespace YizziCamModV2 {
    internal abstract class FromCameraHider : MonoBehaviour {
        public Camera cam;

        public void Start() {
            cam = GetComponent<Camera>();
        }

        public void OnEnable() {
            RenderPipelineManager.endCameraRendering += endCameraRendering;
            RenderPipelineManager.beginCameraRendering += beginCameraRendering;
        }

        public void OnDisable() {
            RenderPipelineManager.endCameraRendering -= endCameraRendering;
            RenderPipelineManager.beginCameraRendering -= beginCameraRendering;
        }

        private void endCameraRendering(ScriptableRenderContext context, Camera camera) {
            if (cam == camera) Show();
        }

        private void beginCameraRendering(ScriptableRenderContext context, Camera camera) {
            if (cam == camera) Hide();
        }

        public abstract void Hide();
        public abstract void Show();
    }
}