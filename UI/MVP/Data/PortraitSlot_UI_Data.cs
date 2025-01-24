using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
namespace N.UI
{
    public class PortraitSlot_UI_Data : MonoBehaviour
    {
        [SerializeField] private Image _portrait_img;
        [SerializeField] private TextMeshProUGUI _ammo_text;
        [SerializeField] private Image _shield_img;
        [SerializeField] private Image _hp_img;
        [SerializeField] private EventTrigger _eventTrigger;
        private void Awake() {

#if UNITY_EDITOR
            // Assertion
            Assert.IsNotNull(_portrait_img, "PortraitSlot_UI");
            Assert.IsNotNull(_ammo_text, "PortraitSlot_UI");
            Assert.IsNotNull(_shield_img, "PortraitSlot_UI");
            Assert.IsNotNull(_hp_img, "PortraitSlot_UI");
            Assert.IsNotNull(_eventTrigger, "PortraitSlot_UI");
#endif
        }
        internal void SetPortraitImage(Sprite sprite) => _portrait_img.sprite = sprite;
        internal void SetAmmoText(string ammoText) => _ammo_text.text = ammoText;
        internal void SetShieldFillAmount(float amount) => _shield_img.fillAmount = amount;
        internal void SetHpFillAmount(float amount) => _hp_img.fillAmount = amount;
        internal void AddButtonHandler(EventTrigger.Entry eventTriggerEntry) => _eventTrigger.triggers.Add(eventTriggerEntry);
    }
}
