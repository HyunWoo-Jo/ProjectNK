using System.Collections.Generic;
using UnityEngine;

namespace N.Data
{
    public enum PlayState {
        Play,
        AI,
        Hide,
    }

    /// <summary>
    /// 인게임에 필요한 데이터를 가지고 있는 클래스
    /// </summary>
    public class InGameData : MonoBehaviour
    {
        public PlayState playState;

        [Header("Camera")]
        public Transform cameraPivotTr; // 카메라가 바라보는 곳
        public Transform cameraTraceTr;
        [HideInInspector] public Vector3 cameraTracePos;
        public bool isTraceCamera = true;

        [Header("Prop")]
        public List<Transform> wall_list = new();

        [Header("Input")]
        public Vector2 limitPos;
        public Vector2 screenPosition;

        [Header("Character")]
        public List<GameObject> characterObj_list = new();
        public int currentCharacterIndex;
    }
}
