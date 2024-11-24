using UnityEngine;
using UnityEngine.UI;

namespace CameraMod.Camera.Pages {
    public class MiscPage {
        public GameObject GO;

        public Text MinDistText;
        public Text SpeedText;
        public Text TpText;
        public Text TpRotText;
        
        public MiscPage(GameObject GO) {
            this.GO = GO;
            
            MinDistText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/MinDistValueText").GetComponent<Text>();
            SpeedText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/SpeedValueText").GetComponent<Text>();
            TpText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/TPText").GetComponent<Text>();
            TpRotText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/TPRotText").GetComponent<Text>();
        }
    }
}
