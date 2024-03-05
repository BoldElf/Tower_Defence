using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        private Text m_Text;

        public enum UpdateSource { Gold, Life, Armor,Mana}
        public UpdateSource source = UpdateSource.Gold;

        void Start()
        {
            m_Text = GetComponent<Text>();

            switch(source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.Instance.LifeUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Armor:
                    TDPlayer.Instance.ArmorUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Mana:
                    TDPlayer.Instance.ManaUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int life)
        {
            m_Text.text = life.ToString();
        }
    }
}

