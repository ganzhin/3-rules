using System.Collections;
using UnityEngine;

namespace Assets.Visual
{
    public class FocusVisual : MonoBehaviour
    {
        [SerializeField] private GameObject _particles0;
        [SerializeField] private GameObject _particles1;

        public void Charge(float charge)
        {
            _particles0.SetActive(charge > 0);
            _particles1.SetActive(charge == 1);
        }
    }
}