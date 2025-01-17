using UnityEngine;
using N.Data;
using Unity;
using System.Collections;
using System.Collections.Generic;
namespace N.Game
{
    /// <summary>
    /// 초기화가 되면 씬을 스타트 하도록 설정 (Editor용) 실제 빌드시 메인 씬에서 초기화 하고 들어오기 때문에 필요없음
    /// </summary>
    public class PlaySceneBooter : MonoBehaviour
    {
        [SerializeField] private GameObject mainLogicObj;

        public void Awake() {

            StartCoroutine(InitStart());
            
        }
        public IEnumerator InitStart() {

            while (true) {
                if (DataManager.Instance.IsAble) {
                    mainLogicObj.SetActive(true);
                    break;
                }
                yield return null;
            }

        }
    }
}
