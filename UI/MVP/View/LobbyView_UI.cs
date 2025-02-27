
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using N.Utills;
using UnityEngine.Assertions;
using Codice.Client.BaseCommands.Import;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI
{
    public interface ILobbyView_UI : IView_UI {
        // Your logic here
    }

    public class LobbyView_UI : View_UI<LobbyPresenter_UI,LobbyModel_UI> ,ILobbyView_UI
    {
        [SerializeField] private EventTrigger _lobbyButton;
        [SerializeField] private EventTrigger _inventoryButton;
        [SerializeField] private EventTrigger _combatSortieButton;
        private Dictionary<string, GameObject> _onUI_dic = new(); // UI가 이미 켜져있나 확인하는 dictionary
        protected override void CreatePresenter() {
            _presenter = new LobbyPresenter_UI();
            _presenter.Init(this);  
        }

        // Your logic here
        #region public
        private void Awake() {
            string typeName = typeof(LobbyView_UI).Name;
#if UNITY_EDITOR
            Assert.IsNotNull(_lobbyButton, typeName);
            Assert.IsNotNull(_inventoryButton, typeName);
            Assert.IsNotNull(_combatSortieButton, typeName);
#endif
            // Button Init
            _lobbyButton.AddEventButton(EventTriggerType.PointerDown, OnLobbyButton, nameof(OnLobbyButton) + "." + typeName);
            _inventoryButton.AddEventButton(EventTriggerType.PointerDown, OnInvenButton, nameof(OnInvenButton) + "." + typeName);
            _combatSortieButton.AddEventButton(EventTriggerType.PointerDown, OnCombatSortie, nameof(OnCombatSortie) + "." + typeName);

        }

        private void OnLobbyButton() {

            CloseAllUI();
        }

        // 전투 출격 버튼
        private void OnCombatSortie() {
            _presenter.LoadNextScene();
        }

        private void OnInvenButton() {
            string key = typeof(InventoryView_UI).Name;
            if (_onUI_dic.ContainsKey(key)) {

            } else { // 생성
                CloseAllUI();
                var view = UI_Controller.Instance.InstantiateUI<InventoryView_UI>(4);
                _onUI_dic.Add(key, view.gameObject);
            }
        }

        private void CloseAllUI() {
            foreach (var item in _onUI_dic) {
                UI_Controller.Instance.RemoveUI(item.Key, item.Value);
            }
            _onUI_dic.Clear();
        }
        #endregion

        #region internal

        #endregion
    }
}
