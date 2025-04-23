using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Vector3 _GravityPointTF = Vector3.zero; 
    public float _GravityForce = 10f;
    public float _Damping;  // 가중치

    private Rigidbody _Rig;

    void Start()
    {
        _Rig = this.gameObject.GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        if (_GravityPointTF != null && _Rig != null)
        {
            
            Vector3 tDirectionToCenter = _GravityPointTF - transform.position;

            
            _Rig.AddForce(tDirectionToCenter.normalized * _GravityForce);

            
            _Rig.linearVelocity *= 1f - (_Damping * Time.fixedDeltaTime);
        }
    }
}
