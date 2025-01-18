using UnityEngine;
using UnityEngine.UI;
namespace N.UI
{
    public class Reloading_UI : MonoBehaviour
    {
        [SerializeField] private Image _fillImg;
        public void UpdateFill(float amount) {
            _fillImg.fillAmount = amount;
        }
    }
}
