using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using UnityEngine;

namespace Assets.Gameplay.Enemies
{
    public class ShootingFly : SimpleFly
    {
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _damage;
        [SerializeField] private Bullet _bulletPrefab;

        private float _shootingCooldown = 2f;
        private float _shootingTimer;

        private void Start()
        {
            _shootingTimer = _shootingCooldown;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _shootingTimer -= Time.deltaTime;
            
            if (_shootingTimer <= 0)
            {
                _shootingTimer = _shootingCooldown;
                Shoot();
            }
            Move();
        }

        public override void Move()
        {
            if (Vector2.Distance(transform.position, FindObjectOfType<Character>().transform.position) > 5)
                base.Move();
            else
                _rigidbody.velocity = Random.insideUnitCircle;
        }

        public override void Shoot()
        {
            var target = FindObjectOfType<Character>().transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0)  - transform.position;

            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = Vector3.Normalize(target);
            Spell spell = new Spell(_damage, Element.Rock);

            bullet.Init(direction, this, spell, .5f, false);
        }
    }
}