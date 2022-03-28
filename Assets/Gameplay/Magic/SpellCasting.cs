using Assets.Gameplay.Magic.UI;
using System;
using UnityEngine;

namespace Assets.Gameplay.Magic
{
    [RequireComponent(typeof(SpellCastingUI))]
    public class SpellCasting : MonoBehaviour
    {
        private const float BaseDamage = 5;

        [SerializeField] private Spell[] _spells = new Spell[3];
        [SerializeField] private int _currentSpellIndex = 0;

        private SpellCastingUI _spellCastingUI;

        private void Start()
        {
            for (int i = 0; i < _spells.Length; i++)
            {
                _spells[i] = new Spell(BaseDamage, (Element)i);
            }
            _spellCastingUI = GetComponent<SpellCastingUI>();
        }

        public void NextSpell()
        {
            _currentSpellIndex++;
            if (_currentSpellIndex >= _spells.Length)
            {
                _currentSpellIndex = 0;
            }
            _spellCastingUI.NextSpell((int)_spells[_currentSpellIndex].SpellElement, _spells[_currentSpellIndex].Damage);
        }

        public Spell GetCurrentSpell()
        {
            return _spells[_currentSpellIndex];
        }
    }
}