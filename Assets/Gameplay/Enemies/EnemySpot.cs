using Assets.Gameplay.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay.Enemies
{
    public class EnemySpot : MonoBehaviour
    {
        [SerializeField] private int _enemyAmount = 5;
        [SerializeField] private Enemy _enemyPrefab;
        private List<Transform> _enemies = new List<Transform>();

        public List<Transform> RespawnEnemies()
        {
            for (int i = 0; i < _enemyAmount; i++)
            {
                Vector2 position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle;
                var enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
                _enemies.Add(enemy.transform);
            }

            return _enemies;
        }
    }
}