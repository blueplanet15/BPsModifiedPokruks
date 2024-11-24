using UnityEngine;
using UnityEngine.UI;

namespace CameraMod.Camera.Pages {
    public class MainPage {
        public GameObject GO;

        public Text FOVText;
        public Text SmoothText;
        public Text NearClipText;
        
        public MainPage(GameObject rootObject) {
            GO = rootObject;
            
            var rootT = rootObject.transform;
            
            FOVText = rootT.Find("Canvas/FovValueText").GetComponent<Text>();
            SmoothText = rootT.Find("Canvas/SmoothingValueText").GetComponent<Text>();
            NearClipText = rootT.Find("Canvas/NearClipValueText").GetComponent<Text>();
        }
    }
}
