
using UnityEngine;
using System.Runtime.CompilerServices;
using N.Data;
using N.Game;
////////////////////////////////////////////////////////////////////////////////////
// Auto Generated Code
#if UNITY_EDITOR
[assembly: InternalsVisibleTo("N.Test")]
#endif
namespace N.UI {

    public class LobbyPresenter_UI : Presenter_UI<LobbyModel_UI, ILobbyView_UI> {
        // Your logic here
        #region internal
        internal void LoadNextScene() {
            Settings.nextSceneName = "GameplayScene";
            LoadManager.Instance.LoadAsync("LoadScene", true);
        }
        #endregion
    }
}
