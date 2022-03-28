using UnityEngine;

namespace Assets.Gameplay.Magic
{
    public class Spell
    {
        public float Damage;
        public Element SpellElement;
        public float StaminaCost = 3f;

        public Spell (float damage, Element element)
        {
            Damage = damage;
            SpellElement = element;
        }

        public void Randomize()
        {
            Damage = Random.Range(1, 20);
            SpellElement = (Element)Random.Range(0, 3);
        }

        public float GetResultDamage(float resist, bool shieldIsActive, Element? weakness = null)
        {
            float resultDamage = Damage / 100f * (100 - resist) * (shieldIsActive ? 0.1f : 1f) * (weakness == SpellElement ? 1.75f : 1);

            return resultDamage;
        }
    }
}