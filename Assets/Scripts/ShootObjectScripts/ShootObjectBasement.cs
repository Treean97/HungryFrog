using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootObjectBasement : MonoBehaviour
{
    // 오브젝트 데이터
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
    
    // 풀에서 재생성 시 다시 IsStable = false;
    private void OnEnable()
    {
        _IsStable = false;

        StartCoroutine(TriggerOnOff());
    }

    private void OnDisable()
    {
        // 꺼질 때 반드시 트리거 켜두기
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


    // 충돌 체크
    // 1. 나와 같은 ID인지
    // 2. 같다면 둘다 비활성화 후 다음 ID 오브젝트 풀링 -> 생성할 위치는 ? 
   
    private void OnCollisionEnter(Collision collision)
    {
        // 첫 접촉 시 Stable 상태로 변경
        if(_IsStable == false)
        {
            _IsStable = true;
        }        

        //중앙 구슬이라면 처리하지 않음
        if (collision.gameObject.CompareTag("ZeroPointSphere"))
        {
            return;
        }

        if (collision.gameObject.GetComponent<ShootObjectBasement>()._ShootObjectData.GetShootObjectID == this._ShootObjectData.GetShootObjectID)
        {
            // 둘 중 한 곳에서만 작동할 수 있게 해시코드로 제한
            if(this.gameObject.GetHashCode() < collision.gameObject.GetHashCode())
            {
                // 충돌시 함수 수행
                GameSceneManager._Inst.CollisionObject(this.gameObject, collision.gameObject);

                // 점수 추가 (합체된 오브젝트의 ID로 점수 환산)
                GameSceneScoreManager._Inst.AddScoreByCombine(GetShootObjectData.GetShootObjectID);
                
            }
        }
    }
}
