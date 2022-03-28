using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gameplay.Magic.UI
{
    public class BarsUI : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Slider _staminaSlider;
        [SerializeField] private Slider _qSlider;
        [SerializeField] private Slider _eSlider;
        public void UpdateHealth(float current, float max)
        {
            if (_healthSlider != null)
            {
                _healthSlider.maxValue = max;
                _healthSlider.value = current;
            }
        }
        
        public void UpdateStamina(float current, float max)
        {
            if (_staminaSlider != null)
            {
                _staminaSlider.maxValue = max;
                _staminaSlider.value = current;
            }
        }

        public void UpdateQE(float q, float e)
        {
            if (_qSlider != null)
            {
                _qSlider.value = q;
            }
            if (_eSlider != null)
            {
                _eSlider.value = e;
            }
        }
    }
}