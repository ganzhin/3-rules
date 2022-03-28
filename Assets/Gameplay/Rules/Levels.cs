using Assets.Gameplay.Abstract;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Gameplay.Rules
{
    public class Levels : MonoBehaviour
    {
        public static bool 
            StaminaFreeze,//+
            OnlyDash, //+
            NoDashes, //+
            NoShield, //+
            OnlyCharged, //+
            NoCharged, //+
            StrongerFlies, //+
            FasterFlies, //+
            RegenerationFlies, //+
            FasterGame, //+
            RegenerationWeakness, //+
            HealthShoots, //+
            HealDashes, //+
            ContactBuff, //+
            LowHpBuild, //+
            ChaosFlies, //+
            FirstAttackBuff, //+
            ImmortalityDashes, //+
            NonChargedBuff; //+

        public static int Level;
        private List<string> _names = new List<string>();
        private List<string> _active = new List<string>();
        [SerializeField] private Text _uiText;
        [SerializeField] private List<Button> _buttons;

        private bool _win;
        [SerializeField] private GameObject _winText;
        [SerializeField] private GameObject _looseText;

        private void Start()
        {
            foreach (var field in typeof(Levels).GetFields())
            {
                if (field.FieldType == typeof(bool))
                {
                    _names.Add(field.Name);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                RandomizeRules();
            }
        }

        private void Update()
        {
            if (FindObjectOfType<Character>() == null) _looseText.SetActive(true);
            Time.timeScale = FasterGame ? 1.5f : 1;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_win)
                {
                    Application.Quit();
                }
                if (FindObjectOfType<Character>() == null)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

        public void CheckLastMob()
        {
            if (FindObjectsOfType<Enemy>().Length == 0)
            {
                Level++;
                foreach (var button in _buttons)
                {
                    button.interactable = true;

                    button.onClick.RemoveAllListeners();
                    var id = GetNonDuplicatedRandom();
                    button.onClick.AddListener(delegate { ToggleById(id); DisableButtons(); } );
                    button.GetComponentInChildren<Text>().text = _names[id];
                }
            }
            else
            {
                DisableButtons();
            }
            if (Level == 10)
            {
                _win = true;
                _winText.SetActive(true);
            }
        }
        
        private int GetNonDuplicatedRandom()
        {
            var random = Random.Range(0, _names.Count);
            if (!_active.Contains(_names[random]))
            {
                return random;
            }
            else
            {
                return GetNonDuplicatedRandom();
            }
        }
        private void DisableButtons()
        {
            foreach (var button in _buttons)
            {
                button.interactable = false;
                button.GetComponentInChildren<Text>().text = "(ЗАКРЫТО)";
            }
            
        }

        public void Toggle(string old, string fieldName)
        {
            if (typeof(Levels).GetField(fieldName) != null)
            {
                if (typeof(Levels).GetField(old) != null)
                {
                    typeof(Levels).GetField(old).SetValue(this, false);
                }

                if (_active.Contains(old))
                {
                    _active.Remove(old);
                }

                typeof(Levels).GetField(fieldName).SetValue(this, true);
                _active.Add(fieldName);
                if (old.Length != 0 && _uiText.text.Contains(old)) 
                {
                    _uiText.text = _uiText.text.Replace($"\n{old}", "") + $"\n{fieldName}";
                }
                else
                {
                    _uiText.text += $"\n{fieldName}";
                }   
            }
            
        }

        public void RandomizeRules()
        {
            Toggle("", _names[GetNonDuplicatedRandom()]);
        }

        public void ToggleById(int id)
        {
            Toggle(_active[0], _names[id]);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
