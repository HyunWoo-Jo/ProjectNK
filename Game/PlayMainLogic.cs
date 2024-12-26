using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
namespace N.Game
{
    public class PlayMainLogic : MonoBehaviour
    {
        [Header("Camera")]
        private Camera _mainCamera;
        [SerializeField] Transform _cameraPivotTr;
        [SerializeField] Transform _cameraTraceTr;
        private Vector3 _cameraTracePos;
        private bool _isTraceCamera = true; 


        [Header("Prop")]
        [SerializeField] List<Transform> _wall_list = new ();

       


        // 초기화
        void Awake()
        {
            _mainCamera = Camera.main;
            ChangeSlot(0);
        }
        

        // Update is called once per frame
        void Update()
        {
           // Test();
        }

        private void LateUpdate() {
            // Camera
            WorkCamera();
            //
        }

        // Camera
        private void WorkCamera() {
            if (_isTraceCamera) { // lerp Target
                CameraFuntion.DistanceProportional(_mainCamera.transform, _cameraTraceTr, _cameraPivotTr, _cameraTracePos, 0.5f, new Vector3(0, 0.2f, -0.2f));
                CameraFuntion.EaseInOutLerpTarget(_mainCamera.transform, _cameraTraceTr);
            }
            //
        }

        //Test
        float timer = 5f;
        int index = 0;
        private void Test() {
            timer += Time.deltaTime;

            if (timer > 5) {
                ChangeSlot(index);
                index = _wall_list.Count > index + 1 ? index + 1 : 0;
                timer -= 5;
            }
        }
        //
        /// <summary>
        /// 슬롯 변경
        /// </summary>
        /// <param name="index"></param>
        private void ChangeSlot(int index) {
            _cameraTracePos = _wall_list[index].position;
        }
    }
}
