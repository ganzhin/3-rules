using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using Assets.Visual;
using UnityEngine;

namespace Assets.Gameplay
{
    public class Focus : MonoBehaviour
    {
        [SerializeField] private float _followSpeed;
        [SerializeField] private Bullet _bulletPrefab;

        [SerializeField] private float _minChargeTime = .1f;
        [SerializeField] private float _chargeTime = .3f;
        private float _charge;

        private Character _character;
        private bool _piercing;
        private FocusVisual _visual;
        private void Start()
        {
            _character = FindObjectOfType<Character>();
            _charge = _chargeTime;
            _visual = GetComponent<FocusVisual>();
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, _character.transform.position) > 1f)
            {
                transform.position = Vector2.Lerp(transform.position, _character.transform.position, _followSpeed * Time.deltaTime);
                transform.position += Vector3.back;
            }
            _visual.Charge(_charge/_chargeTime);
        }

        public void Charge(Character character)
        {
            _charge += Time.deltaTime;
            _charge = Mathf.Clamp(_charge, 0, _chargeTime);

            if (_charge < _chargeTime)
                character.SpendStamina(Time.deltaTime * character.GetCurrentSpell().StaminaCost);
        }
        public void Shoot(Character character)
        {
            if (_charge > _minChargeTime)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
                Vector2 direction = Vector3.Normalize(mousePosition);
                float charge = Mathf.Lerp(0, 1.2f, _charge / _chargeTime);
                bullet.Init(direction, character, character.GetCurrentSpell(), charge, _piercing);
                _charge = 0;
            }
        }

        public void Ultimate(Character character)
        {
            character.SpendStamina(character.Stamina);
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            var spell = new Spell(character.GetCurrentSpell().Damage*Time.deltaTime/2, character.GetCurrentSpell().SpellElement);
            bullet.Init(Vector3.zero, character, spell, 10f, true);
        }
    }
}