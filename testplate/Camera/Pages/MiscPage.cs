using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CameraMod.Camera.Pages {
    public class MiscPage {
        public GameObject GO;

        public TextMeshPro MinDistText;
        public TextMeshPro SpeedText;
        public TextMeshPro TpText;
        public TextMeshPro TpRotText;
        
        public MiscPage(GameObject GO) {
            this.GO = GO;
            
            MinDistText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/MinDistValueText").GetComponent<TextMeshPro>();
            SpeedText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/SpeedValueText").GetComponent<TextMeshPro>();
            TpText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/TPText").GetComponent<TextMeshPro>();
            TpRotText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/TPRotText").GetComponent<TextMeshPro>();
        }
    }
}
