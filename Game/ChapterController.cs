using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace N.Game
{
    public class ChapterController : MonoBehaviour {
        [SerializeField] private NavMeshAgent _unitGroupAgent;
        [SerializeField] private Camera _mapCamera;
        // main Camera
        private Vector3 _mainCameraOffset = new Vector3(0, 18.9f, -4f);
        private float _mainCameraLerpSpeed = 2f;
        // map Camera
        private Vector3 _mapCameraOffset = new Vector3(0, 5f, 0);
        // nav agent
        private float _maxNavMeshDistance = 1.0f; // ��ȿ�� NavMesh�κ��� �ִ� �Ÿ� ����

        private void Awake() {
#if UNITY_EDITOR
            // ����
            Assert.IsNotNull(_unitGroupAgent);
            Assert.IsNotNull(_mapCamera);
#endif
            _mapCamera.aspect = 1f;
            _mapCamera.rect = new Rect(0, 0, 1, 1);
            _mapCamera.orthographicSize = 7f;
        }

        void Update() {
            WorkNavAgent();
            MoveMapCamera();
            MoveMainCamera();
        }

        /// <summary>
        /// unit Group�� ������
        /// </summary>
        private void WorkNavAgent() {
            if (Input.GetMouseButtonDown(0)) // ���콺 ���� Ŭ�� ��
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) // Ŭ���� ������ �ִ��� Ȯ��
                {
                    NavMeshHit navHit;
                    // Ŭ���� ��ġ�� NavMesh ���� ��ȿ���� Ȯ��
                    if (NavMesh.SamplePosition(hit.point, out navHit, _maxNavMeshDistance, NavMesh.AllAreas)) {
                        // ��ȿ�� NavMesh ��ġ�� �̵�
                        _unitGroupAgent.SetDestination(navHit.position);
                    }
                }
            }
        }

        // Map Camera �̵�
        private void MoveMapCamera() {
            Vector3 newPos = _unitGroupAgent.transform.position + _mapCameraOffset;
            _mapCamera.transform.position = newPos;      
        }
        /// <summary>
        /// Main Camera �̵�
        /// </summary>
        private void MoveMainCamera() {
            Vector3 targetPos = _unitGroupAgent.transform.position + _mainCameraOffset;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, Time.deltaTime * _mainCameraLerpSpeed);
        }
    }
}
