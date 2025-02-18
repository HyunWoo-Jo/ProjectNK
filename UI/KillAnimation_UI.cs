using DG.Tweening;
using UnityEngine;
using static N.UI.OpenAnimation_UI;

namespace N.UI
{
    public enum KillAnimationType {
        Scale,
        ScaleX,
        ScaleY,
    }
    public class KillAnimation_UI : MonoBehaviour
    {
        [SerializeField] private KillAnimationType _type;
        [SerializeField] private float _time = 0.2f;

        [SerializeField] bool _isAutoKill = true;
        [SerializeField] private float _delay = 1f;
        public void OnAnimation() {
            switch (_type) {
                case KillAnimationType.Scale:
                transform.DOScale(Vector3.zero, _time).OnComplete(Kill);
                break;
                case KillAnimationType.ScaleX:
                transform.DOScaleX(0, _time).OnComplete(Kill);
                break;
                case KillAnimationType.ScaleY:
                transform.DOScaleY(0, _time).OnComplete(Kill);
                break;
            }
        }

        private void OnDestroy() {
            DOTween.Kill(this.transform);
        }

        private void Kill() {
            Destroy(this.gameObject);
        }
        private void Start() {
            if (_isAutoKill) {
                // delay 시간후 OnAnimation 실행
                Invoke("OnAnimation", _delay);
            }
        }

    }
}
