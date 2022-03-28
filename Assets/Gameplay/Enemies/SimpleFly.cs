using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using Assets.Gameplay.Rules;
using UnityEngine;

namespace Assets.Gameplay.Enemies
{
    public class SimpleFly : Enemy
    {
        internal float _health = 5;
        internal float _maxHealth = 5;
        internal float _damage = 1.2f;

        public override float Health { get => _health; set { _health = value; } }
        public override float MaxHealth { get => _maxHealth; set { _maxHealth = value; } }
        public override float Armor { get; set; }

        public virtual void Start()
        {
            if (Levels.StrongerFlies) _damage *= 1.25f;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public virtual void Update()
        {
            if (Levels.RegenerationFlies) Health += Time.deltaTime * .1f;
        }

        public override void Move()
        {
            var speed = Random.Range(1f, 4f) * (Levels.FasterFlies ? 2 : 1);
            Vector3 characterPosition = FindObjectOfType<Character>().transform.position;
            if (Levels.ChaosFlies) characterPosition = Random.insideUnitCircle * 5;
            if (Vector2.Distance(transform.position, characterPosition) < 20f || Levels.ChaosFlies)
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
                Spell spell = new Spell(_damage * (Levels.ContactBuff ? 1.2f : 1), Element.Fire);
                character.TakeDamage(spell);
                if (Levels.ContactBuff) TakeDamage(spell, Time.deltaTime);

            }
        }
    }
}