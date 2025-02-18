using UnityEngine;
using UnityEngine.SceneManagement;
using N.DesignPattern;
using System.Collections;
using System;
namespace N.Game
{
    public class LoadManager : Singleton<LoadManager>
    {
        private AsyncOperation _loadAsyncOper;
        private Action<float> _progressHandler;

        public bool IsDone() {
            if (_loadAsyncOper != null) {
                return _loadAsyncOper.isDone;
            }
            return false;
        }
        public float Progress() {
            if (_loadAsyncOper != null) {
                return _loadAsyncOper.progress;
            }
            return 0;
        }
        public void AddProgressHandler(Action<float> action) {
            _progressHandler += action;
        }
        public void RemoveProgressHandler(Action<float> action) {
            _progressHandler -= action;
        }

        public void LoadAsync(string nextScene, bool allowSceneActivation) {
            _loadAsyncOper = SceneManager.LoadSceneAsync(nextScene);
            _loadAsyncOper.allowSceneActivation = allowSceneActivation; // 씬 자동 전환 방지
            StartCoroutine(LoadProgress());
        }

        private IEnumerator LoadProgress() {
            while (_loadAsyncOper != null) {
                float progress = _loadAsyncOper.progress;
                _progressHandler?.Invoke(progress);
                if (progress >= 1f || _loadAsyncOper.isDone) {
                    break;
                }
                yield return null;
            }
        }

        public void Done() {
            if (_loadAsyncOper != null) {
                _loadAsyncOper.allowSceneActivation = true;
                _loadAsyncOper = null;
            }
        }

    }
}
