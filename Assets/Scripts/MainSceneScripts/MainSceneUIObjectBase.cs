using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUIObjectBase : MonoBehaviour
{
    [Header("Position Set")]    
    [SerializeField] private Vector3 _ResetRotationAngle;

    [Header("Respawn Set")]
    [SerializeField]
    protected float _RespawnDelay = 2;

    protected virtual void Start()
    {
        ResetRotation();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(_ResetRotationAngle);
    }
}
