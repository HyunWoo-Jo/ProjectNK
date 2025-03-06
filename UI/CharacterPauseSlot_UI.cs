using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Unity.Android.Gradle.Manifest;
namespace N.UI
{
    public class CharacterPauseSlot_UI : MonoBehaviour
    {
        [SerializeField] private Image _portraitImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _dealFillImage;
        [SerializeField] private TextMeshProUGUI _dealValueText;
        [SerializeField] private Image _defenseFillImage;
        [SerializeField] private TextMeshProUGUI _defenseValueText;

        private void Awake() {
#if UNITY_EDITOR
            string scriptName = nameof(CharacterPauseSlot_UI);
            Assert.IsNotNull(_portraitImage, scriptName);
            Assert.IsNotNull(_nameText, scriptName);
            Assert.IsNotNull(_dealFillImage, scriptName);
            Assert.IsNotNull(_dealValueText, scriptName);
            Assert.IsNotNull(_defenseFillImage, scriptName);
            Assert.IsNotNull(_defenseValueText, scriptName);

#endif
        }

        #region internal
        internal void SetPortraitImage(Sprite portrait_sprite) => _portraitImage.sprite = portrait_sprite;
        internal void SetNameText(string text) => _nameText.text = text;

        internal void SetDealFillAmout(float amount) => _dealFillImage.fillAmount = amount;
        internal void SetDealText(string text) => _dealValueText.text = text;
        internal void SetDefenseFillAmout(float amount) => _defenseFillImage.fillAmount = amount;
        internal void SetDefenseText(string text) => _defenseValueText.text = text;
        #endregion
    }
}
