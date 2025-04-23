using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootChanceInfo
{
    [SerializeField]
    public ShootObject ShootObjectsData;    
    public float Chance = 0f;
}

public class ObjectShootChance : MonoBehaviour
{
    [SerializeField]
    public List<ShootChanceInfo> _ShootChances = new List<ShootChanceInfo>
    {
        new ShootChanceInfo { Chance = 0 },
        new ShootChanceInfo { Chance = 0 },
        new ShootChanceInfo { Chance = 0 },
    };
}

