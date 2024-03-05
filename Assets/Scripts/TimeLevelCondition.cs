using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

public class TimeLevelCondition : MonoBehaviour, ILevelCondition
{
    [SerializeField] private float timeLimit = 4f;

    private void Start()
    {
        timeLimit += Time.time;
    }
    public bool IsComleted => Time.time > timeLimit;
}
