using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CameraMod.Camera.Pages {
    public class MainPage {
        public GameObject GO;

        public TextMeshPro FOVText;
        public TextMeshPro SmoothText;
        public TextMeshPro NearClipText;
        
        public MainPage(Transform rootT) {
            GO = rootT.gameObject;
            
            FOVText = rootT.Find("Canvas/FovValueText").GetComponent<TextMeshPro>();
            SmoothText = rootT.Find("Canvas/SmoothingValueText").GetComponent<TextMeshPro>();
            NearClipText = rootT.Find("Canvas/NearClipValueText").GetComponent<TextMeshPro>();
        }
    }
}
