using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    private Transform _target;
    private Vector3 _cameraOffset;
    private float _cameraLerpSpeed;

    private void Start() {
        _cameraLerpSpeed = 2f;
        _cameraOffset = new Vector3(0, 0.1f, -0.4f);
    }

    
    private void LateUpdate() {
        if (_target) {
            LerpMoveCamera();
        }
        if(_cameraPivot) {
            Vector3 pos = transform.position;
            Vector3 targetPos = _cameraPivot.transform.position;

            Vector3 forward = (targetPos - pos).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 up = Vector3.Cross(forward, right).normalized;
            Matrix4x4 mat = new Matrix4x4(right, up, forward, new Vector4(0, 0, 0, 1));
            transform.rotation = mat.rotation;

            transform.LookAt(_cameraPivot);
        }
        Test();

    }

    //Test
    [SerializeField] private List<Transform> _targetList = new List<Transform>();
    float timer;
    int index = 0;
    private void Test() {
        timer += Time.deltaTime;

        if (timer > 5) {
            _target = _targetList[index];
            index = _targetList.Count > index + 1 ? index + 1 : 0;
            timer -= 5;
        }
    }
    //

    private void LerpMoveCamera() {
        transform.position = Vector3.Lerp(this.transform.position, _target.transform.position + _cameraOffset, Time.deltaTime * _cameraLerpSpeed);
    }
}
