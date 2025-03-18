using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace N.Game
{
    public class ChapterController : MonoBehaviour {
        [SerializeField] private NavMeshAgent _unitGroupAgent;
        [SerializeField] private Camera _mapCamera;
        [SerializeField] private LineRenderer _lineRenderer;
        // main Camera
        private Vector3 _mainCameraOffset = new Vector3(0, 18.9f, -4f);
        private float _mainCameraLerpSpeed = 2f;
        // map Camera
        private Vector3 _mapCameraOffset = new Vector3(0, 5f, 0);
        // nav agent
        private float _maxNavMeshDistance = 1.0f; // ��ȿ�� NavMesh�κ��� �ִ� �Ÿ� ����
        private int _navIndex = -1;
        private bool _isOnUI = false;
        private int _pathLayer;
        private int _enemyLayer;
        private void Awake() {
#if UNITY_EDITOR
            // ����
            Assert.IsNotNull(_unitGroupAgent);
            Assert.IsNotNull(_mapCamera);
            Assert.IsNotNull(_lineRenderer);
#endif
            _mapCamera.aspect = 1f;
            _mapCamera.rect = new Rect(0, 0, 1, 1);
            _mapCamera.orthographicSize = 7f;
            _pathLayer = LayerMask.GetMask("Path");
            _enemyLayer = LayerMask.GetMask("Enemy");
        }

        void Update() {
            WorkNavAgent();
            MoveMapCamera();
            MoveMainCamera();
            ClickEnemyObject();
        }

        /// <summary>
        /// Enemy Click Event ó��
        /// </summary>
        private void ClickEnemyObject() {
            if (Input.GetMouseButtonDown(0) && !_isOnUI) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, _enemyLayer)) {
                    var hitEnemy = hit.collider.gameObject.GetComponent<ChapterEnemy>();
                    if (hitEnemy != null) {
                        // Chapter ���� ������Ʈ
                        var mainLogic = MainLogicManager.Instance;
                        mainLogic.spawnDataList = hitEnemy.EnemySpawnDataList;
                        mainLogic.cameraLogicClassName = hitEnemy.CameraLogicClassName;
                        mainLogic.combatLogicClassName = hitEnemy.CombatLogicClassName;
                        mainLogic.enemyLogicClassName = hitEnemy.EnemyLogicClassName;
                        mainLogic.inputLogicClassName_list = hitEnemy.InputLogicClassName;

                    }

                }
            }
        }


        /// <summary>
        /// unit Group�� ������
        /// </summary>
        private void WorkNavAgent() {
            if (Input.GetMouseButtonDown(0) && !_isOnUI) // ���콺 ���� Ŭ�� ��
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, _pathLayer)) // Ŭ���� ������ �ִ��� Ȯ��
                {
                    NavMeshHit navHit;
                    // Ŭ���� ��ġ�� NavMesh ���� ��ȿ���� Ȯ��
                    if (NavMesh.SamplePosition(hit.point, out navHit, _maxNavMeshDistance, NavMesh.AllAreas)) {
                        // ��ȿ�� NavMesh ��ġ�� �̵�
                        NavMeshPath navMeshPath = new ();
                        _unitGroupAgent.CalculatePath(navHit.position, navMeshPath);

                        // ������ ����
                        _unitGroupAgent.SetDestination(navHit.position);
                        
                        // LineRenderer ������Ʈ
                        Vector3[] corners = navMeshPath.corners.Reverse().Select(coner => coner + new Vector3(0,0.03f,0)).ToArray();
                        _lineRenderer.positionCount = corners.Length;
                        _lineRenderer.SetPositions(corners);
                        _navIndex = corners.Length - 1;
                    }
                }

            }
            // LineRender position�� �°� ����
            if(_lineRenderer.positionCount > 0) {
                _navIndex = _lineRenderer.positionCount - 1;
                _lineRenderer.SetPosition(_navIndex, _unitGroupAgent.transform.position);
                if (_navIndex > 0 && Vector3.Distance(_lineRenderer.GetPosition(_navIndex), _lineRenderer.GetPosition(_navIndex - 1)) < 0.15f) {

                    _lineRenderer.positionCount = _navIndex;
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
