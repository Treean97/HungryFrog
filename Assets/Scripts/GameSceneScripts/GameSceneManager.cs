using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // Planet 풀의 최대 ID
    int _MaxID;

    // 오프닝 지속 시간
    [SerializeField]
    private int _OpeningDuration;
    public int GetOpeningDuration => _OpeningDuration;

    // 오프닝 진행 중 여부
    private bool _IsOpening = true;
    public bool GetIsOpening => _IsOpening;

    // UI 매니저 참조
    [SerializeField]
    GameSceneUIManager _GameSceneUIManager;

    // 엔딩 처리 플래그
    bool IsEnd = false;

    void Start()
    {
        // 풀 ID 초기화 및 BGM 재생, 오프닝 블록 시작
        _MaxID = ObjectPoolManager._Inst._Pools.Count - 1;
        SoundManager._Inst.PlayBGM("GameSceneBGM");
        StartCoroutine(BlockShootOnOpening());
    }

    void Update()
    {
        // (필요 시 업데이트 로직 추가)
    }

    // 오프닝 시간 동안 발사 기능 차단
    IEnumerator BlockShootOnOpening()
    {
        yield return new WaitForSeconds(_OpeningDuration);
        _IsOpening = false;
    }

    // 두 오브젝트 충돌 시 합성 처리
    public void CollisionObject(GameObject tObject_A, GameObject tObject_B)
    {
        // 합성 사운드 재생
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "CombineSound");

        // A 오브젝트의 ID 가져오기
        int tObjectID = tObject_A.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID;

        // 최대 ID 이상이면 합성 불가
        if (tObjectID >= _MaxID)
            return;

        // 충돌 오브젝트 반환
        ReturnObject(tObject_A, tObject_B);

        // 새 오브젝트 ID 및 이름 결정
        int tNewObjectID = tObjectID + 1;
        string tNewObjectName = ObjectPoolManager._Inst._Pools[tNewObjectID].Name;

        // 합성 위치 계산 및 오브젝트 생성
        Vector3 tSpawnPos = (tObject_A.transform.position + tObject_B.transform.position) / 2f;
        GetObject(tNewObjectName, tSpawnPos);
    }

    // 오브젝트 풀에 사용된 오브젝트 반환
    void ReturnObject(GameObject tObject_A, GameObject tObject_B)
    {
        ObjectPoolManager._Inst.ReturnObject(tObject_A);
        ObjectPoolManager._Inst.ReturnObject(tObject_B);
    }

    // 풀에서 오브젝트 가져와 위치 설정
    void GetObject(string tName, Vector3 tSpawnPos)
    {
        GameObject tGO = ObjectPoolManager._Inst.GetObject(tName);
        tGO.transform.position = tSpawnPos;
    }

    // 게임 엔딩 처리 및 점수 저장
    public void Ending()
    {
        if (IsEnd) return;
        IsEnd = true;

        // UI 엔딩 연출
        _GameSceneUIManager.Ending();

        // 모든 투사체 일시정지
        GameObject[] tShootObject = GameObject.FindGameObjectsWithTag("ShootObject");
        foreach (var item in tShootObject)
            item.GetComponent<ShootObjectBasement>().PauseObject();

        // PlayFab에 점수 저장 요청
        PlayFabLeaderboardManager._Inst.SaveScore(
            GameSceneScoreManager._Inst.GetScore,
            onSuccess: () =>
            {
                Debug.Log("PlayFab 점수 저장 성공!");
            },
            onError: (errMsg) =>
            {
                Debug.LogError("PlayFab 점수 저장 실패: " + errMsg);
            }
        );
    }
}
