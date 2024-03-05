using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image upgradeIcon;
        private int costNumber = 0;
        [SerializeField] private Text level, costText;
        [SerializeField] private Button buyButton;


        public void Initialize()
        {
            upgradeIcon.sprite = asset.sprite;
            var savedLevel = Upgrades.GetUpgradeLevel(asset);

            if(savedLevel >= asset.costByLevel.Length)
            {
                level.text = "Level: " + (savedLevel+1) + " (max)";
                buyButton.interactable = false;
                buyButton.transform.Find("TextBuy").gameObject.SetActive(false);
                buyButton.transform.Find("healmet (1)").gameObject.SetActive(false);
                costText.text = "X";
                costNumber = int.MaxValue;
            }
            else
            {
                level.text = "Level: " + (savedLevel + 1).ToString();
                costNumber = asset.costByLevel[savedLevel];
                costText.text = costNumber.ToString();
            }

            
        }

        internal void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
            Initialize();
        }
    }
}

