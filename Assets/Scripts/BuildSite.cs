using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    public class BuildSite : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] public TowerAsset[] buildableTowers;
        public void SetBuildableTowers(TowerAsset[] towers) 
        { 
            if(towers == null || towers.Length == 0)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                buildableTowers = towers; //!
            }
           
        } 
        public static event Action<BuildSite> OnClickEvent; 

        public static void HideContols()
        {
            OnClickEvent(null);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(this);
        }

    }
}

