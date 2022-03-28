using Assets.Gameplay.Abstract;
using Assets.Gameplay.Enemies;
using Assets.Gameplay.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gameplay
{
    public class Fireplace : MonoBehaviour
    {
        [SerializeField] private GameObject _uiCanvas;
        [SerializeField] private GameObject _fireplaceCanvas;

        [SerializeField] private Image _transitionImage;

        [SerializeField] private GameObject _targetPointerPrefab;
        private List<Transform> _pointers = new List<Transform>();
        private List<Transform> _enemies = new List<Transform>();

        private void Start()
        {
            _transitionImage.gameObject.SetActive(false);
            for (int i = 0; i < 50; i++)
            {
                _pointers.Add(Instantiate(_targetPointerPrefab).transform);
            }
        }

        private void Update()
        {
            foreach (var pointer in _pointers)
            {
                pointer.transform.position = Vector3.forward * 1000;
            }

            if (_enemies.Count > 0)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    Transform pointer = _pointers[i];
                    Transform target = _enemies[i];
                    if (FindObjectOfType<Character>())
                    {
                        if (target != null && Vector3.Distance(FindObjectOfType<Character>().transform.position, target.position) > 7f)
                        {
                            pointer.transform.position = Vector3.MoveTowards(FindObjectOfType<Character>().transform.position, target.position, 2);
                        }
                    }
                }
            }
        }

        public void Rest()
        {
            if (!FindObjectOfType<Character>()) return;

            FindObjectOfType<Levels>().CheckLastMob();
            foreach(var enemy in FindObjectsOfType<Enemy>())
            {
                Destroy(enemy.gameObject);
                _enemies.Remove(enemy.transform);
            }
            IUnit character = FindObjectOfType<Character>();
            character.Heal(character.MaxHealth);

            _uiCanvas.SetActive(false);
            _fireplaceCanvas.SetActive(true);
            StartCoroutine(TransitionRoutine());
            FindObjectOfType<World>().RespawnTrees();
        }

        public void EndRest()
        {
            foreach (var enemySpot in FindObjectsOfType<EnemySpot>())
            {
                _enemies.AddRange(enemySpot.RespawnEnemies());
            }

            _uiCanvas.SetActive(true);
            _fireplaceCanvas.SetActive(false);
        }

        public IEnumerator TransitionRoutine()
        {
            if (!FindObjectOfType<Character>()) yield break;

            _transitionImage.gameObject.SetActive(true);
            yield return null;
            for (float i = 0; i <= 1; i+= Time.deltaTime)
            {
                _transitionImage.CrossFadeAlpha(1, 1f, false);
                yield return null;
            }
            yield return null;

            Vector3 sphere = Random.onUnitSphere;
            sphere.z = 0;

            FindObjectOfType<Character>().transform.position = transform.position + sphere * 1.5f;
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                _transitionImage.CrossFadeAlpha(0, 1f, false);
                yield return null;
            }
            _transitionImage.gameObject.SetActive(false);

        }
    }
}