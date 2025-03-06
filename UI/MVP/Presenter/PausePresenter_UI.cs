
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;
using N.Game;
using Unity.Android.Gradle.Manifest;
using N.Data;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class PausePresenter_UI : Presenter_UI<PauseModel_UI, IPauseView_UI> {
        // Your logic here
        #region internal
        // 다시 Scene Load
        internal void ReloadScene() {
            LoadManager.Instance.LoadAsync(SceneManager.GetActiveScene().name, true);
        }

        // Lobby로 이동
        internal void LoadLobbyScene() {
            LoadManager.Instance.LoadAsync("MainLobbyScene", true);
        }

        internal void Pause(bool isActive) {
            Settings.isTimeStop = isActive;
        }
        #endregion
    }
}
