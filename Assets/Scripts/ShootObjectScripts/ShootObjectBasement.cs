using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootObjectBasement : MonoBehaviour
{
    [SerializeField]
    ShootObject _ShootObjectData;       // 풀링된 오브젝트의 데이터

    GameSceneManager _GameSceneManager; // 씬 매니저 참조

    public ShootObject GetShootObjectData => _ShootObjectData; // 데이터 접근 프로퍼티

    bool _IsStable = false;             // 충돌 후 안정 상태 플래그
    public bool GetIsStable => _IsStable;

    private MeshCollider _MeshCollider;  // 콜라이더 참조

    private void Awake()
    {
        _MeshCollider = GetComponent<MeshCollider>(); // 콜라이더 가져오기
    }

    private void Start()
    {
        // 씬 매니저 가져오기
        _GameSceneManager = GameObject
            .FindGameObjectWithTag("GameSceneManager")
            .GetComponent<GameSceneManager>();
    }

    private void OnEnable()
    {
        _IsStable = false;              // 활성화 시 안정 플래그 초기화
        StartCoroutine(TriggerOnOff()); // 트리거 모드 잠깐 켜기
    }

    private void OnDisable()
    {
        // 비활성화 시 콜라이더 트리거 모드 유지 및 코루틴 정리
        _MeshCollider.isTrigger = true;
        StopAllCoroutines();
    }

    // 첫 프레임에만 트리거 모드로 설정하고 다음 프레임에 리셋
    private IEnumerator TriggerOnOff()
    {
        _MeshCollider.isTrigger = true;
        yield return null;
        _MeshCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 첫 충돌 시 안정 상태로 전환
        if (!_IsStable)
            _IsStable = true;

        // 중심 구체와의 충돌은 무시
        if (collision.gameObject.CompareTag("ZeroPointSphere"))
            return;

        // 같은 ID끼리만 합성 로직 실행
        var other = collision.gameObject.GetComponent<ShootObjectBasement>();
        if (other._ShootObjectData.GetShootObjectID == _ShootObjectData.GetShootObjectID)
        {
            // 한 쌍만 처리하기 위한 해시코드 비교
            if (this.GetHashCode() < other.GetHashCode())
            {
                // 합성 처리 호출
                _GameSceneManager.CollisionObject(this.gameObject, collision.gameObject);

                // 합성 점수 추가
                GameSceneScoreManager._Inst.AddScoreByCombine(_ShootObjectData.GetShootObjectID);
            }
        }
    }

    // 오브젝트 일시 정지 (엔딩 시 호출)
    public void PauseObject()
    {
        var tRig = GetComponent<Rigidbody>();
        // 위치/회전 모두 고정
        tRig.constraints = RigidbodyConstraints.FreezePositionX 
                         | RigidbodyConstraints.FreezePositionY 
                         | RigidbodyConstraints.FreezePositionZ;
        tRig.freezeRotation = true;
    }
}
