using System.Collections.Generic;
using UnityEngine;
using N.UI;

namespace N.Data
{
    public enum PlayState {
        Play,
        AI,
        Hide,
    }

    /// <summary>
    /// �ΰ��ӿ� �ʿ��� �����͸� ������ �ִ� Ŭ����
    /// </summary>
    public class InGameData : MonoBehaviour
    {
        public PlayState playState;

        [Header("Canvas")]
        public Canvas mainCanvas;

        [Header("Camera")]
        public Transform cameraPivotTr; // ī�޶� �ٶ󺸴� ��
        public Transform cameraTraceTr;
        [HideInInspector] public Vector3 cameraTracePos;
        public bool isTraceCamera = true;

        [Header("Prop")]
        public List<Transform> wall_list = new();

        [Header("Input")]
        public Vector2 limitPos;

        

        [Header("UI")]
        public AimView_UI aimView;

        [Header("Character")]
        public List<GameObject> characterObj_list = new();
        public int currentCharacterIndex;
    }
}
