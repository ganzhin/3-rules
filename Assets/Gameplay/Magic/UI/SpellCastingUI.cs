using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Gameplay.Magic.UI
{
    public class SpellCastingUI : MonoBehaviour
    {
        [SerializeField] private Image _spellBackGround;
        [SerializeField] private Image[] _spellImage;
        [SerializeField] private Text _spellText;
        public void NextSpell(int elementId, float damage)
        {
            for (int i = 0; i < 2; i++)
            {
                var id = elementId + i >= Enum.GetNames(typeof(Element)).Length ? 0 : elementId + i;
                _spellImage[i].sprite = Resources.Load<Sprite>($"Inventory/{(Element)id}");
            }
            _spellText.text = damage.ToString();
        }
    }
}