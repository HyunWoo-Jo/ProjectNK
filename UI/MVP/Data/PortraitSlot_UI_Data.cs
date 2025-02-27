using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using N.Utills;
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
            string scriptName = typeof(PortraitSlot_UI_Data).Name;
            Assert.IsNotNull(_background_img, scriptName);
            Assert.IsNotNull(_portrait_img, scriptName);
            Assert.IsNotNull(_ammo_text, scriptName);
            Assert.IsNotNull(_shield_img, scriptName);
            Assert.IsNotNull(_hp_img, scriptName);
            Assert.IsNotNull(_reloading_img, scriptName);
            Assert.IsNotNull(_eventTrigger, scriptName);
            Assert.IsNotNull(_textParent, scriptName);

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
        internal void AddButtonHandler(EventTriggerType type, Action action, string entryClassMethodName) => _eventTrigger.AddEventButton(type, action, entryClassMethodName);


    }
}
