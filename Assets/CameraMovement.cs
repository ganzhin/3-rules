using Assets.Gameplay;
using UnityEngine;

namespace Assets
{
    public class CameraMovement : MonoBehaviour
    {
        private float _followSpeed = 5;
        private Character _character;


        private void Start()
        {
            _character = FindObjectOfType<Character>();
        }

        private void Update()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 targetPosition = (_character.transform.position * 3 + mousePosition) / 4f;

            transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * _followSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, -30);
        }
    }
}