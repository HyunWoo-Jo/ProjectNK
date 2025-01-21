using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
namespace N.UI
{
    public class PortraitSlot_UI : MonoBehaviour
    {
        [SerializeField] internal Image portrait_img;
        [SerializeField] internal TextMeshProUGUI ammo_text;
        [SerializeField] internal Image shield_img;
        [SerializeField] internal Image hp_img;

        private void Awake() {

#if UNITY_EDITOR
            // Assertion
            Assert.IsNotNull(portrait_img, "PortraitSlot_UI");
            Assert.IsNotNull(ammo_text, "PortraitSlot_UI");
            Assert.IsNotNull(shield_img, "PortraitSlot_UI");
            Assert.IsNotNull(hp_img, "PortraitSlot_UI");
#endif
        }
    }
}
