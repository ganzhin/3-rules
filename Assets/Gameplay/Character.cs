using Assets.Gameplay.Abstract;
using Assets.Gameplay.Magic;
using Assets.Gameplay.Magic.UI;
using Assets.Gameplay.Rules;
using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;

namespace Assets.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Controls))]
    [RequireComponent(typeof(SpellCasting))]
    [RequireComponent(typeof(BarsUI))]
    public class Character : MonoBehaviour, IUnit
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _dashDistance;

        [SerializeField] private int _potionCount;
        [SerializeField] private int _potionPower;

        [SerializeField] private float _maxStamina;
        [SerializeField] private float _stamina;
        [SerializeField] private float _staminaRestoreSpeed = 2;
        [SerializeField] private float _iFrameTime = .5f;

        private Controls _controls;
        private Rigidbody2D _rigidbody;
        private SpellCasting _spellCasting;
        private Focus _focus;
        private BarsUI _barsUI;

        private bool _shieldIsActive;
        [SerializeField] private GameObject _shield;

        private float _shieldCooldown = 8;
        private float _shieldTimer;

        private float _ultimateCooldown = 14;
        private float _ultimateTimer;

        [SerializeField] private float _dashTimer = .2f;

        private bool _staminaSpent = false;
        [SerializeField] private float _staminaRestoreTime = 1;
        private float _staminaTimer;

        private bool _rest;

        [SerializeField] private float _maxHealth;

        public float MaxHealth { get => _maxHealth; set { _maxHealth = value; } }
        public float Health { get; set; }
        public float Armor { get; set; }
        public Element? Weakness { get { return null; } set { return; } }
        public bool Immortality { get; set; }
        public float Stamina { get => _stamina; }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _controls = GetComponent<Controls>();
            _spellCasting = GetComponent<SpellCasting>();
            _focus = FindObjectOfType<Focus>();
            _barsUI = GetComponent<BarsUI>();
            _shieldIsActive = false;

            Health = MaxHealth;
        }

        private void Update()
        {
            if (!_rest)
            {
                Shoot();
                if (_controls.RMB) Dash();
                if (_controls.Q) Ultimate();
                if (_controls.R) RotateSpells();
            }
            
            if (_staminaSpent == false)
            {
                if (!Levels.StaminaFreeze || _rigidbody.velocity == Vector2.zero)
                {
                    RecoverStamina(Time.deltaTime);
                }
            }
            if (_controls.E) { if (!_rest) Shield(); CampRest(); }

            _staminaTimer += Time.deltaTime;
            _staminaTimer = Mathf.Clamp(_staminaTimer, 0, _staminaRestoreTime*2);
        }

        private void FixedUpdate()
        {
            if (!_controls.RMB || _dashTimer < .1f || _stamina <= 0)
            {
                if (!_rest && !Levels.OnlyDash)
                    Move();
            }
            if (!_controls.RMB)
            {
                if (_dashTimer < .2f) _dashTimer += Time.deltaTime;
            }

            if (_rest) _rigidbody.velocity = Vector3.zero;

            if (Levels.RegenerationWeakness)
            {
                ((IUnit)this).Heal(Time.deltaTime * .1f);
            }

            _shieldTimer = Mathf.Clamp(_shieldTimer + Time.deltaTime, 0, _shieldCooldown);
            _ultimateTimer = Mathf.Clamp(_ultimateTimer + Time.deltaTime, 0, _ultimateCooldown);
            _barsUI.UpdateHealth(Health, MaxHealth);
            _barsUI.UpdateQE(_ultimateTimer / _ultimateCooldown, _shieldTimer / _shieldCooldown);
        
        }

        private void RotateSpells()
        {
            _spellCasting.NextSpell();
        }

        public void Death()
        {
            Debug.Log($"{this} is dead");
            Destroy(this);
        }

        public void Move()
        {
            _rigidbody.velocity = _controls.Direction * _movementSpeed;
        }

        public void Dash()
        {
            if (Levels.NoDashes) return;

            if (_dashTimer > .1 && _stamina > 0)
            {
                if (Levels.HealDashes) ((IUnit)this).Heal(Time.deltaTime);
                _rigidbody.AddForce(_dashDistance * _controls.Direction, ForceMode2D.Impulse);
                _dashTimer -= Time.deltaTime;
                SpendStamina(Time.fixedDeltaTime * 2);
                if (Levels.ImmortalityDashes) StartCoroutine(ImmortalityFrames());
            }
        }

        public void Shoot()
        {
            if (_stamina > 0)
            {
                if (_controls.LMB) _focus.Charge(this);
                else _focus.Shoot(this);
            }
            else
            {
                _focus.Shoot(this);
            }
        }

        public void Shield()
        {
            if (Levels.NoShield) return;
            if (_shieldTimer == _shieldCooldown)
                StartCoroutine(ShieldRoutine());
        }

        private IEnumerator ShieldRoutine()
        {
            _shieldTimer = 0;
            _shieldIsActive = true;
            _shield.SetActive(true);
            yield return new WaitForSeconds(3f);

            _shield.SetActive(false);
            _shieldIsActive = false;
        }

        public void Ultimate()
        {
            if (_ultimateTimer == _ultimateCooldown)
            {
                _ultimateTimer = 0;
                _focus.Ultimate(this);
            }
        }

        public Spell GetCurrentSpell()
        {
            return _spellCasting.GetCurrentSpell();
        }

        public void SpendStamina(float amount)
        {
            _stamina -= amount;
            StartCoroutine(SpentTrigger());
            _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
            _barsUI.UpdateStamina(_stamina, _maxStamina);
        }
        public void RecoverStamina(float amount)
        {
            if (_staminaTimer >= (_stamina > 0 ? _staminaRestoreTime : _staminaRestoreTime * 2))
            {
                _stamina += amount * _staminaRestoreSpeed;
                _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
                _barsUI.UpdateStamina(_stamina, _maxStamina);
            }
        }

        public IEnumerator SpentTrigger()
        {
            _staminaSpent = true;
            _staminaTimer = 0;
            yield return new WaitForEndOfFrame();
            _staminaSpent = false;
        }

        public void CampRest()
        {
            foreach (var fireplace in FindObjectsOfType<Fireplace>())
            {
                if (Vector3.Distance(fireplace.transform.position, transform.position) < 3f)
                {
                    _rest = !_rest;
                    if (_rest)
                        fireplace.Rest();
                    else
                        fireplace.EndRest();    
                    _stamina = _maxStamina;
                }
            }
        }
    
        public IEnumerator ImmortalityFrames()
        {
            Immortality = true;
            for (float f = 0; f < _iFrameTime; f += .15f)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, .15f);
                yield return new WaitForSeconds(.075f);
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.clear, 0f);
                yield return new WaitForSeconds(.075f);
            }
            Immortality = false;
        }

        public void TakeDamage(Spell spell, float charge = 1)
        {
            if (!Immortality)
            {
                Health -= spell.GetResultDamage(Armor, _shieldIsActive, Weakness) * charge * (Levels.RegenerationWeakness ? 2 : 1);

                if (Health <= 0)
                {
                    Death();
                }

                StartCoroutine(ImmortalityFrames());
            }
        }
    }
}