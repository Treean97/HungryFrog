using System.Text;
using UnityEngine;

public class SlicedObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {       
        // ���� ���   
        SoundManager._Inst.PlayRandomSFX(SoundCategory.MainSceneSlicedObjectCollision);
    }
}
