using UnityEngine;
using N.Data;
using Unity;
using System.Collections;
using System.Collections.Generic;
namespace N.Game
{
    /// <summary>
    /// �ʱ�ȭ�� �Ǹ� ���� ��ŸƮ �ϵ��� ���� (Editor��) ���� ����� ���� ������ �ʱ�ȭ �ϰ� ������ ������ �ʿ����
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
