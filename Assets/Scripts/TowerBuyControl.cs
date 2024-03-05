using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControl : MonoBehaviour
    {

        [SerializeField] private TowerAsset m_TowerAsset;
        public void SetTowerAsset(TowerAsset asset) { m_TowerAsset = asset; }
        [SerializeField] private Text m_Text;
        [SerializeField] private Button m_Button;
        [SerializeField] private Transform m_BuildSite;
        //public Transform SetBuildSite { set { m_BuildSite = value; } }

        public void SetBuildSite(Transform value)
        {
            m_BuildSite = value;
        }

        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);
            m_Text.text = m_TowerAsset.goldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }

        private void GoldStatusCheck(int gold)
        {
            if(gold >= m_TowerAsset.goldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_Text.color = m_Button.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuld(m_TowerAsset, m_BuildSite);
            BuildSite.HideContols();
        }
    }
}
