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
        private float _maxNavMeshDistance = 1.0f; // 유효한 NavMesh로부터 최대 거리 설정

        private void Awake() {
#if UNITY_EDITOR
            // 검증
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
        /// unit Group을 움직임
        /// </summary>
        private void WorkNavAgent() {
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭 시
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) // 클릭된 지점이 있는지 확인
                {
                    NavMeshHit navHit;
                    // 클릭된 위치가 NavMesh 위에 유효한지 확인
                    if (NavMesh.SamplePosition(hit.point, out navHit, _maxNavMeshDistance, NavMesh.AllAreas)) {
                        // 유효한 NavMesh 위치로 이동
                        _unitGroupAgent.SetDestination(navHit.position);
                    }
                }
            }
        }

        // Map Camera 이동
        private void MoveMapCamera() {
            Vector3 newPos = _unitGroupAgent.transform.position + _mapCameraOffset;
            _mapCamera.transform.position = newPos;      
        }
        /// <summary>
        /// Main Camera 이동
        /// </summary>
        private void MoveMainCamera() {
            Vector3 targetPos = _unitGroupAgent.transform.position + _mainCameraOffset;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, Time.deltaTime * _mainCameraLerpSpeed);
        }
    }
}
