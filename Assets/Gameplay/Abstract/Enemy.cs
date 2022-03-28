using Assets.Gameplay.Magic;
using Assets.Gameplay.Rules;
using System.Collections;
using UnityEngine;

namespace Assets.Gameplay.Abstract
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Enemy : MonoBehaviour, IUnit
    {
        [SerializeField] internal Element _weakness;

        public abstract float Health { get; set; }
        public abstract float MaxHealth { get; set; }
        public abstract float Armor { get; set; }
        public Element? Weakness { get => _weakness; set { _weakness = (Element)value; } }
        public bool Immortality { get; set; }

        [SerializeField] internal Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Move();
        }
        public virtual void Death()
        {
            FindObjectOfType<Levels>().CheckLastMob();
            Destroy(gameObject);
        }
        public abstract void Move();
        public abstract void Shoot();

        public IEnumerator ImmortalityFrames()
        {
            return null;
        }

        public void TakeDamage(Spell spell, float charge = 1)
        {
            if (!Immortality)
            {
                Health -= spell.GetResultDamage(Armor, false, Weakness) * charge;
                if (Health <= 0)
                {
                    Death();
                }
            }
        }
    }
}