using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CameraMod.Camera.Pages {
    public class MainPage {
        public GameObject GO;

        public TextMeshPro FOVText;
        public TextMeshPro SmoothText;
        public TextMeshPro NearClipText;
        
        public MainPage(GameObject rootObject) {
            GO = rootObject;
            
            var rootT = rootObject.transform;
            
            FOVText = rootT.Find("Canvas/FovValueText").GetComponent<TextMeshPro>();
            SmoothText = rootT.Find("Canvas/SmoothingValueText").GetComponent<TextMeshPro>();
            NearClipText = rootT.Find("Canvas/NearClipValueText").GetComponent<TextMeshPro>();
        }
    }
}
