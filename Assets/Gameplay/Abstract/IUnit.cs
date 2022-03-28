using Assets.Gameplay.Magic;
using System.Collections;
using UnityEngine;

namespace Assets.Gameplay.Abstract
{
    public interface IUnit
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Armor { get; set; }
        public Element? Weakness { get; set; }
        public bool Immortality { get; set; }


        public void Move();
        public void Shoot();
        public void TakeDamage(Spell spell, float charge = 1);
        public void Death();
        public virtual void Heal(float heal)
        {
            Health = Mathf.Clamp(Health + heal, 0, MaxHealth);
        }

        public IEnumerator ImmortalityFrames();
    }
}
