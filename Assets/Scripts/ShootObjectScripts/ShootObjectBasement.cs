using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootObjectBasement : MonoBehaviour
{
    // ������Ʈ ������
    [SerializeField]
    ShootObject _ShootObjectData;
    public ShootObject GetShootObjectData { get { return _ShootObjectData; } }

    bool _IsStable = false;
    public bool GetIsStable {  get { return _IsStable; } }

    private MeshCollider _MeshCollider;

    private void Awake()
    {

        _MeshCollider = GetComponent<MeshCollider>();

    }
    
    
    private void Start()
    {

    }
    
    // Ǯ���� ����� �� �ٽ� IsStable = false;
    private void OnEnable()
    {
        _IsStable = false;

        StartCoroutine(TriggerOnOff());
    }

    private void OnDisable()
    {
        // ���� �� �ݵ�� Ʈ���� �ѵα�
        _MeshCollider.isTrigger = true;
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TriggerOnOff()
    {        
        _MeshCollider.isTrigger = true;

        yield return null;

        _MeshCollider.isTrigger = false;
    }


    // �浹 üũ
    // 1. ���� ���� ID����
    // 2. ���ٸ� �Ѵ� ��Ȱ��ȭ �� ���� ID ������Ʈ Ǯ�� -> ������ ��ġ�� ? 
   
    private void OnCollisionEnter(Collision collision)
    {
        // ù ���� �� Stable ���·� ����
        if(_IsStable == false)
        {
            _IsStable = true;
        }        

        //�߾� �����̶�� ó������ ����
        if (collision.gameObject.CompareTag("ZeroPointSphere"))
        {
            return;
        }

        if (collision.gameObject.GetComponent<ShootObjectBasement>()._ShootObjectData.GetShootObjectID == this._ShootObjectData.GetShootObjectID)
        {
            // �� �� �� �������� �۵��� �� �ְ� �ؽ��ڵ�� ����
            if(this.gameObject.GetHashCode() < collision.gameObject.GetHashCode())
            {
                // �浹�� �Լ� ����
                GameSceneManager._Inst.CollisionObject(this.gameObject, collision.gameObject);

                // ���� �߰� (��ü�� ������Ʈ�� ID�� ���� ȯ��)
                GameSceneScoreManager._Inst.AddScoreByCombine(GetShootObjectData.GetShootObjectID);
                
            }
        }
    }
}
