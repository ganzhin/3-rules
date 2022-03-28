using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using Assets.Gameplay.Rules;
using UnityEngine;

namespace Assets.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _charge;
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;

        private Spell _spell;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private IUnit _sourceUnit;

        private bool _piercing;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }

            _rigidbody.velocity = _direction * _speed;
        }

        public void Init(Vector2 direction, IUnit sourceUnit, Spell spell, float charge, bool piercing)
        {
            _direction = direction;
            _sourceUnit = sourceUnit;
            _spell = spell;
            _charge = charge;
            _piercing = piercing;

            transform.localScale = transform.localScale * charge * 2.25f;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var unit = other.gameObject.GetComponent<IUnit>();
            if (unit != null && unit.GetType() != _sourceUnit.GetType())
            {
                var multiplier = Levels.LowHpBuild && unit.Health <= unit.MaxHealth / 5 ? 1.3f : 1;
                multiplier += Levels.FirstAttackBuff && unit.Health == unit.MaxHealth ? 0.25f : 0;
                unit.TakeDamage(_spell, _charge * multiplier);
                if (!_piercing)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}