using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUIObjectBase : MonoBehaviour
{
    [SerializeField]
    float _YPosition;

    [SerializeField]
    Rigidbody _Rig;

    [SerializeField]
    Vector3 _ResetRotationAngle;

    bool _IsDrop = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        ResetRotation();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //    if (transform.position.y >= _YPosition)
        //    {
        //        _Rig.useGravity = true;
        //        _Rig.constraints = RigidbodyConstraints.None;
        //    }
        //    else
        //    {
        //        _Rig.useGravity = false;
        //        _Rig.linearVelocity = Vector3.zero;
        //        _Rig.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        //        _Rig.freezeRotation = true;

        //    }

        if (transform.position.y > _YPosition)
        {
            _Rig.useGravity = true;
            _Rig.constraints = RigidbodyConstraints.None;
        }
        else
        {
            _Rig.useGravity = false;
            _Rig.linearVelocity = Vector3.zero;

            // 정확하게 멈추기 위해 위치 고정            
            Vector3 correctedPos = transform.position;
            correctedPos.y = _YPosition;
            transform.position = correctedPos;

            _Rig.constraints = RigidbodyConstraints.FreezeAll;

            // 사운드
            if(_IsDrop)
            {
                _IsDrop = false;
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "MainSceneObjectDrop");
            }
        }
    }

    protected virtual void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(_ResetRotationAngle);
    }

}
