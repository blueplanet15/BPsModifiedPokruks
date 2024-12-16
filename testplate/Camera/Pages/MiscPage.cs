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
        
        public MiscPage(Transform t) {
            GO = t.gameObject;
            
            MinDistText = t.Find("Canvas/MinDistValueText").GetComponent<TextMeshPro>();
            SpeedText = t.Find("Canvas/SpeedValueText").GetComponent<TextMeshPro>();
            TpText = t.Find("Canvas/TPText").GetComponent<TextMeshPro>();
            TpRotText = t.Find("Canvas/TPRotText").GetComponent<TextMeshPro>();
        }
    }
}
