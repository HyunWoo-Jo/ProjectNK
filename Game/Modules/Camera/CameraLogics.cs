using System;
using UnityEngine;
using N.Data;
namespace N.Game {
    public enum CameraLogicClassName{
        StandardGameCameraLogic,

    }

    public abstract class CameraLogic : MonoBehaviour
    {
        protected InGameData _gameData;

        protected Camera _mainCamera;


        public void Init(Camera camera, InGameData gameData) {
            _mainCamera = camera;
            _gameData = gameData;
        }
  

        public abstract void WorkCamera();

        /// <summary>
        /// ½½·Ô º¯°æ
        /// </summary>
        /// <param name="index"></param>
        public void ChangeSlot(int index) {
           _gameData.cameraTracePos = _gameData.wall_list[index].position;
        }
    }

    public class StandardGameCameraLogic : CameraLogic {
        public override void WorkCamera() {
            if (_gameData.isTraceCamera) { // lerp Target
                CameraFuntion.DistanceProportional(_mainCamera.transform, _gameData.cameraTraceTr, _gameData.cameraPivotTr, _gameData.cameraTracePos, 0.5f, new Vector3(0, 0.25f, -0.2f));
                CameraFuntion.EaseInOutLerpTarget(_mainCamera.transform, _gameData.cameraTraceTr);
            }
        }
    }
}
