using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
namespace N.UI
{
    public enum OpenAnimationType {
        Scale,
        ScaleX,
        ScaleY,
        MoveRight,
        MoveUp,
        Alpha = 20,
    }
    [Serializable]
    public struct UIAnimatedOption {
        public OpenAnimationType type;
        public float value;
        public float time;

    }

    /// <summary>
    /// Enable 될때 Animation이 실행되도록 해주는 Class
    /// </summary>
    public class OpenAnimation_UI : MonoBehaviour
    {
        
        [SerializeField] private List<UIAnimatedOption> _uiOption_list = new();
        private Action _completeHandler;
        private Vector3 _initLocalPosition;
        private void OnEnable() {
            OnAnimation();
        }
        private void OnDisable() {
            transform.localPosition = _initLocalPosition;
        }
        private void OnDestroy() {
            DOTween.Kill(this.transform);
            if(_uiOption_list.Select((type) => { return type.type == OpenAnimationType.Alpha; }).Count() > 0) {
                foreach(var image in GetComponentsInChildren<Image>()) {
                    image.DOKill();
                }
            }
        }

        public void OnAnimation() {
            _initLocalPosition = transform.localPosition;
            foreach (var option in _uiOption_list) {
                switch (option.type) {
                    case OpenAnimationType.Scale:
                    transform.localScale = Vector3.zero;
                    transform.DOScale(Vector3.one, option.time).OnComplete(() => {
                        _completeHandler?.Invoke();
                    });
                    break;
                    case OpenAnimationType.ScaleX:
                    transform.localScale = new Vector3(0, 1, 1);
                    transform.DOScaleX(1, option.time);
                    break;
                    case OpenAnimationType.ScaleY:
                    transform.localScale = new Vector3(1, 0, 1);
                    transform.DOScaleY(1, option.time);
                    break;
                    case OpenAnimationType.MoveRight:
                    transform.localPosition = transform.localPosition + new Vector3(-option.value, 0, 0);
                    transform.DOLocalMoveX(transform.localPosition.x + option.value, option.time).OnComplete(() => {
                        _completeHandler?.Invoke();
                    });
                    break;
                    case OpenAnimationType.MoveUp:
                    transform.localPosition = transform.localPosition + new Vector3(0, -option.value, 0);
                    transform.DOLocalMoveY(transform.localPosition.y + option.value, option.time).OnComplete(() => {
                        _completeHandler?.Invoke();
                    });
                    break;
                    case OpenAnimationType.Alpha:
                    var images = transform.GetComponentsInChildren<Image>();
                    foreach (var image in images) {
                        Color color = image.color;
                        color.a = 0f;
                        image.color = color; // alpha 0 셋팅
                        color.a = 1f;
                        image.DOColor(color, option.time);
                    }

                    break;
                }
            }
        }


        public void AddCompleteHanlder(Action action) {
            _completeHandler += action;
        }

    }
}
