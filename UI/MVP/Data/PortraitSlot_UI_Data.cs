using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
using Unity.Android.Gradle.Manifest;
using DG.Tweening;
namespace N.UI
{
    public class PortraitSlot_UI_Data : MonoBehaviour
    {
        [SerializeField] private Image _background_img;
        [SerializeField] private Image _portrait_img;
        [SerializeField] private TextMeshProUGUI _ammo_text;
        [SerializeField] private Image _shield_img;
        [SerializeField] private Image _hp_img;
        [SerializeField] private Image _reloading_img;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private GameObject _textParent;
         
        private void Awake() {

#if UNITY_EDITOR
            // Assertion

            Assert.IsNotNull(_background_img, "PortraitSlot_UI");
            Assert.IsNotNull(_portrait_img, "PortraitSlot_UI");
            Assert.IsNotNull(_ammo_text, "PortraitSlot_UI");
            Assert.IsNotNull(_shield_img, "PortraitSlot_UI");
            Assert.IsNotNull(_hp_img, "PortraitSlot_UI");
            Assert.IsNotNull(_reloading_img, "PortraitSlot_UI");
            Assert.IsNotNull(_eventTrigger, "PortraitSlot_UI");
            Assert.IsNotNull(_textParent, "PortraitSlot_UI");

#endif
        }
        internal Image GetBackgroundImg() => _background_img;
        internal void SetPortraitImage(Sprite sprite) => _portrait_img.sprite = sprite;
        internal void SetAmmoText(string ammoText) => _ammo_text.text = ammoText;
        internal void SetShieldFillAmount(float amount) => _shield_img.fillAmount = amount;
        internal void SetHpFillAmount(float amount) => _hp_img.fillAmount = amount;
        internal void SetReloadingFillAmount(float amount) => _reloading_img.fillAmount = amount;
        internal void SetActiveReloading(bool isActive) {
            _reloading_img.gameObject.SetActive(isActive);
            _ammo_text.gameObject.SetActive(!isActive);
        }
        internal void SetActiveTextParent(bool isActive) => _textParent.gameObject.SetActive(isActive);

        internal RectTransform GetPortraitRectTransform() => _portrait_img.rectTransform;
        internal void AddButtonHandler(EventTrigger.Entry eventTriggerEntry, string entryClassMethodName) => _eventTrigger.AddEventButton(eventTriggerEntry, entryClassMethodName);


    }
}
