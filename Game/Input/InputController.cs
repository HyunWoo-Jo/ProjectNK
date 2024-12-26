using System;
using UnityEngine;
using N.Data;
namespace N.Game
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPivot;
        [SerializeField] Vector2 _limitPos;
        private void Update() {
            if(Input.GetMouseButton(0)) {
                Vector3 mouseDirection = Input.mousePositionDelta;

                Vector2 newPos = _cameraPivot.transform.position + (mouseDirection * Settings.CursorSpeed * Time.deltaTime);
                newPos.x = Mathf.Clamp(newPos.x, -_limitPos.x, _limitPos.x);
                newPos.y = Mathf.Clamp(newPos.y, -_limitPos.y, _limitPos.y);
                _cameraPivot.transform.position = newPos;
            }
        }
    }
}
