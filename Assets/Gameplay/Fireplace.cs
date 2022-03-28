using Assets.Gameplay.Abstract;
using Assets.Gameplay.Enemies;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gameplay
{
    public class Fireplace : MonoBehaviour
    {
        [SerializeField] private GameObject _uiCanvas;
        [SerializeField] private GameObject _fireplaceCanvas;

        [SerializeField] private Image _transitionImage;

        private void Start()
        {
            _transitionImage.gameObject.SetActive(false);
        }
        public void Rest()
        {
            foreach(var enemy in FindObjectsOfType<Enemy>())
            {
                Destroy(enemy.gameObject);
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
                enemySpot.RespawnEnemies();
            }

            _uiCanvas.SetActive(true);
            _fireplaceCanvas.SetActive(false);
        }

        public IEnumerator TransitionRoutine()
        {
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