using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Assets.Gameplay.Enemies
{
    public class SimpleFly : Enemy
    {
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _damage;

        public override float Health { get => _health; set { _health = value; } }
        public override float MaxHealth { get => _maxHealth; set { _maxHealth = value; } }
        public override float Armor { get; set; }

        public override void Move()
        {
            var speed = Random.Range(1f, 4f);
            Vector3 characterPosition = FindObjectOfType<Character>().transform.position;
            if (Vector2.Distance(transform.position, characterPosition) < 12f)
                _rigidbody.velocity = Vector3.Normalize(characterPosition - transform.position) * speed;

            _rigidbody.velocity += Random.insideUnitCircle;
        }
        public override void Shoot()
        {
            return;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            IUnit character = collision.gameObject.GetComponent<Character>();

            if (character != null)
            {
                Spell spell = new Spell(_damage, Element.Fire);
                character.TakeDamage(spell);
            }
        }
    }
}