using System.Collections.Generic;
using UnityEngine;

namespace N.Game
{
    /// <summary>
    /// 인게임에 필요한 데이터를 가지고 있는 클래스
    /// </summary>
    public class InGameData : MonoBehaviour
    {
        [Header("Camera")]
        public Transform cameraPivotTr;
        public Transform cameraTraceTr;
        [HideInInspector] public Vector3 cameraTracePos;
        public bool isTraceCamera = true;

        [Header("Prop")]
        public List<Transform> wall_list = new();

        [Header("Input")]
        public Vector2 limitPos;
    }
}
