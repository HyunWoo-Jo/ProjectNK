using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
namespace N.UI
{
    /// <summary>
    /// 메인 로비의 버튼을 초기화하고 UI를 불러옴
    /// </summary>
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private UI_Controller _controller;
        [SerializeField] private EventTrigger _chepterButton;
        [SerializeField] private EventTrigger _lobbyButton;
        [SerializeField] private EventTrigger _inventoryButton;
        private Dictionary<string, GameObject> _onUI_dic = new();
        private void Awake() {
            string typeName = typeof(LobbyUIController).Name;
#if UNITY_EDITOR
            Assert.IsNotNull(_controller, typeName);
            Assert.IsNotNull(_chepterButton, typeName);
            Assert.IsNotNull(_lobbyButton, typeName);
            Assert.IsNotNull(_inventoryButton, typeName);
#endif
            // Button Init
            _chepterButton.AddEventButton(EventTriggerType.PointerDown, OnChepterButton, nameof(OnChepterButton) + "." + typeName);
            _lobbyButton.AddEventButton(EventTriggerType.PointerDown, OnLobbyButton, nameof(OnLobbyButton) + "." + typeName);
            _inventoryButton.AddEventButton(EventTriggerType.PointerDown, OnInvenButton, nameof(OnInvenButton) + "." + typeName);
        }
    
        private void OnChepterButton() {
            
        }

        private void OnLobbyButton() {

            CloseAllUI();
        }

        private void OnInvenButton() {
            string key = typeof(InventoryView_UI).Name;
            if (_onUI_dic.ContainsKey(key)) {

            } else { // 생성
                CloseAllUI();
                var view = _controller.InstantiateUI<InventoryView_UI>(4);
                _onUI_dic.Add(key, view.gameObject);
            }
        }

        private void CloseAllUI() {
            foreach(var item in _onUI_dic) {
                _controller.RemoveUI(item.Key, item.Value);
            }
            _onUI_dic.Clear();
        }
    }
}
