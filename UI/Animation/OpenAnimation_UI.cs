using UnityEngine;
using DG.Tweening;
using System;
using NUnit.Framework;
namespace N.UI
{
    /// <summary>
    /// Enable 될때 Animation이 실행되도록 해주는 Class
    /// </summary>
    public class OpenAnimation_UI : MonoBehaviour
    {
        public enum OpenAnimationType {
            Scale,
            ScaleX,
            ScaleY,
            MoveRight,
        }
        [SerializeField] private OpenAnimationType _type;
        [SerializeField] private float _value;
        [SerializeField] private float _time = 0.2f;
        private Action _completeHandler;
        private Vector3 _initLocalPosition;
        private void Awake() {
            _initLocalPosition = transform.localPosition;
#if UNITY_EDITOR
            Assert.False(_time == 0f, typeof(OpenAnimation_UI).Name);
#endif
        }
        private void OnEnable() {
            OnAnimation();
        }
        private void OnDisable() {
            transform.localPosition = _initLocalPosition;
        }
        private void OnDestroy() {
            DOTween.Kill(this.transform);
        }

        private void OnAnimation() {
            switch (_type) {
                case OpenAnimationType.Scale:
                transform.localScale = Vector3.zero;
                transform.DOScale(Vector3.one, _time).OnComplete(() => {
                    _completeHandler?.Invoke();
                });
                break;
                case OpenAnimationType.ScaleX:
                transform.localScale = new Vector3(0, 1, 1);
                transform.DOScaleX(1, _time);
                break;
                case OpenAnimationType.ScaleY:
                transform.localScale = new Vector3(1, 0, 1);
                transform.DOScaleY(1, _time);
                break;
                case OpenAnimationType.MoveRight:
                transform.localPosition = transform.localPosition + new Vector3(-_value, 0, 0);
                transform.DOLocalMoveX(transform.localPosition.x + _value, _time).OnComplete(() => {
                    _completeHandler?.Invoke();
                });
                break;
                
            }
        }


        public void AddCompleteHanlder(Action action) {
            _completeHandler += action;
        }

    }
}
