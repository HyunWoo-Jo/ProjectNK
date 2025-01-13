using System.Collections.Generic;
using UnityEngine;

namespace N.Data
{
    /// <summary>
    /// �ΰ��ӿ� �ʿ��� �����͸� ������ �ִ� Ŭ����
    /// </summary>
    public class InGameData : MonoBehaviour
    {
        [Header("Canvas")]
        public Canvas mainCanvas;

        [Header("Camera")]
        public Transform cameraPivotTr;
        public Transform cameraTraceTr;
        [HideInInspector] public Vector3 cameraTracePos;
        public bool isTraceCamera = true;

        [Header("Prop")]
        public List<Transform> wall_list = new();

        [Header("Input")]
        public Vector2 limitPos;

        public List<CharacterStats> fieldCharacter_list = new();
    }
}
