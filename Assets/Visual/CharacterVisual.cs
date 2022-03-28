using UnityEngine;

namespace Assets.Visual
{
    public class CharacterVisual : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _animator = GetComponent<Animator>(); 
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _animator.SetBool("Moving", _rigidbody.velocity != Vector2.zero);
            _spriteRenderer.flipX = _rigidbody.velocity.x > 0;

        }
    }
}
