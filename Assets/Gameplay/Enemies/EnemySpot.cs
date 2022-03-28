using Assets.Gameplay.Abstract;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Gameplay.Enemies
{
    public class EnemySpot : MonoBehaviour
    {
        [SerializeField] private int _enemyAmount = 5;
        [SerializeField] private Enemy _enemyPrefab;

        void Start()
        {
            RespawnEnemies();
        }

        public void RespawnEnemies()
        {
            StartCoroutine(RespawnByTime());
        }

        public IEnumerator RespawnByTime()
        {
            for (int i = 0; i < _enemyAmount; i++)
            {
                Vector2 position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle;
                Instantiate(_enemyPrefab, position, Quaternion.identity);
                yield return new WaitForSeconds(.2f);
            }
        }
    }
}