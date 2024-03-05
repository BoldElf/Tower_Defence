using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        
        public int goldCost = 15;
        public Sprite sprite;
        public Sprite GUISprite;
        public int Tower = 1;
        public TurretProperties turretProperties;

        [SerializeField] private UpgradeAsset requieredUpgrade;
        [SerializeField] private int requieredUpgradeLevel;

        public bool IsAvailble() => !requieredUpgrade || 
            requieredUpgradeLevel <= Upgrades.GetUpgradeLevel(requieredUpgrade);

        [SerializeField]public TowerAsset[] m_UpgradesTo;
    }
}
