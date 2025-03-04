using N.Data;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace N.UI
{
    public class EquipmentUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        private EquipmentData _equipData;
        private string _key;
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_iconImage);
#endif
        }
        private void OnDestroy() {
            // 제거
            if(!string.IsNullOrEmpty(_key))
                DataManager.Instance.ReleaseAsset(_key);
        }

        public void SetEquipment(EquipmentData equipData) {
            _equipData = equipData;
            _key = _equipData.type.ToString() + ".png";
            // icon 로드
            _iconImage.sprite = DataManager.Instance.LoadAssetSync<Sprite>(_key);            
        }

    }
}
