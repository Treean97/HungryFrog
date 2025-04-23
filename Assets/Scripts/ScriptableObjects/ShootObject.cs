using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootObjectData", menuName = "Scriptable Object/ShootObjectData", order = 0)]
public class ShootObject : ScriptableObject
{
    [SerializeField]
    private int _ShootObjectID;
    public int GetShootObjectID { get { return _ShootObjectID; } }
    [SerializeField]
    private string _ShootObjectName;
    public string GetShootObjectName { get {  return _ShootObjectName; } }

    [SerializeField]
    private Sprite _ShootObjectSpriteOnUI;
    public Sprite GetShootObjectSpriteOnUI { get { return _ShootObjectSpriteOnUI; } }
}
