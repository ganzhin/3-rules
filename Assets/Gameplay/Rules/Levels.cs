using Assets.Gameplay.Abstract;
using UnityEngine;

namespace Assets.Gameplay.Rules
{
    public class Levels : MonoBehaviour
    {
        private int _level;

        public void CheckLastMob()
        {
            if (FindObjectsOfType<Enemy>().Length == 0)
            {
                _level++;
            }
        }
    }
}