
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using N.Game;
using Unity.Android.Gradle.Manifest;
using N.Data;
using NUnit.Framework;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class PausePresenter_UI : Presenter_UI<PauseModel_UI, IPauseView_UI> {
        // Your logic here
        #region internal
        // �ٽ� Scene Load
        internal void ReloadScene() {
            LoadManager.Instance.LoadAsync(SceneManager.GetActiveScene().name, true);
        }

        // Lobby�� �̵�
        internal void LoadLobbyScene() {
            LoadManager.Instance.LoadAsync("MainLobbyScene", true);
        }
        // ����, ���� ���·� ����
        internal void Pause(bool isActive) {
            Settings.isTimeStop = isActive;
        }

        internal void UpdateCharacterStat(int slotIndex, CharacterStats stats) {
           if(_model._key_list.Count <= slotIndex) {
                _model._key_list.Add(string.Empty);
            }
            // �̹� ���� �� ��� ����
            ReleaseSpirte(slotIndex);
            // ���� ����
            _model._key_list[slotIndex] = stats.portraitName;
            var sprite = DataManager.Instance.LoadAssetSync<Sprite>(_model._key_list[slotIndex]);

            _view.UpdateCharacterSlotUI(slotIndex, sprite, stats.characterName, 0, 0);

        }

        /// <summary>
        /// Adressable ����
        /// </summary>
        /// <param name="slotIndex"></param>
        internal void ReleaseSpirte(int slotIndex) {
            if (!string.IsNullOrEmpty(_model._key_list[slotIndex])) { // �̹� ���� �� ��� ����
                DataManager.Instance.ReleaseAsset(_model._key_list[slotIndex]);
                _model._key_list[slotIndex] = string.Empty;
            }
        }

        #endregion
    }
}
