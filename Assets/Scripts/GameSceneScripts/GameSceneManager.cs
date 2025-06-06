using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // Planet 오브젝트의 종류
    int _MaxID;

    // 오프닝 시간
    [SerializeField]
    private int _OpeningDuration;
    public int GetOpeningDuration => _OpeningDuration;

    private bool _IsOpening = true;
    public bool GetIsOpening => _IsOpening;

    [SerializeField]
    GameSceneUIManager _GameSceneUIManager;

    bool IsEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        _MaxID = ObjectPoolManager._Inst._Pools.Count - 1;

        SoundManager._Inst.PlayBGM("GameSceneBGM");


        StartCoroutine(BlockShootOnOpening());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 오프닝 동안 발사 방지
    IEnumerator BlockShootOnOpening()
    {
        yield return new WaitForSeconds(_OpeningDuration);

        _IsOpening = false;
    }


    // A, B의 충돌
    public void CollisionObject(GameObject tObject_A, GameObject tObject_B)
    {
        // 사운드
        SoundManager._Inst.PlaySFX(SoundCategory.SFX, "CombineSound");

        // Object_A나 B의 ID
        int tObjectID = tObject_A.gameObject.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID;

        // 더이상 합쳐질게 없다면 그냥 리턴
        if (tObjectID >= _MaxID)
        {
            return;
        }        

        // 오브젝트 리턴
        ReturnObject(tObject_A, tObject_B);

       // 새로운 오브젝트 ID는 기존의 ID + 1
        int tNewObjectID = tObjectID + 1;

        // 이름을 찾아서 풀에서 Get
        string tNewObjectName = ObjectPoolManager._Inst._Pools[tNewObjectID].Name;

        // 새로운 오브젝트의 위치 설정 -> 기존 오브젝트 A와 B의 중간지점
        Vector3 tSpawnPos = (tObject_A.transform.position + tObject_B.transform.position) / 2f;

        // 새로운 오브젝트 Get
        GetObject(tNewObjectName.ToString(), tSpawnPos);


    }

    // 오브젝트를 받아 제거
    void ReturnObject(GameObject tObject_A, GameObject tObject_B)
    {
        ObjectPoolManager._Inst.ReturnObject(tObject_A);
        ObjectPoolManager._Inst.ReturnObject(tObject_B);
    }

    // ID를 받아 생성
    void GetObject(string tName, Vector3 tSpawnPos)
    {
        GameObject tGO = ObjectPoolManager._Inst.GetObject(tName);
        tGO.transform.position = tSpawnPos;
    }

    public void Ending()
    {
        if (!IsEnd)
        {
            IsEnd = true;

            _GameSceneUIManager.Ending();

            // 발사 오브젝트 정지
            GameObject[] tShootObject = GameObject.FindGameObjectsWithTag("ShootObject");
            foreach (var item in tShootObject)
            {
                item.GetComponent<ShootObjectBasement>().PauseObject();
            }

            // 데이터 Playfab 저장
            PlayFabLeaderboardManager._Inst.SaveScore(GameSceneScoreManager._Inst.GetScore,
            onSuccess: () =>
            {
                Debug.Log("PlayFab에 최종 점수 업로드 성공!");
                // 예: 여기서 리더보드 씬으로 전환하거나 완료 팝업 띄우기
            },
            onError: (errMsg) =>
            {
                Debug.LogError("PlayFab 점수 업로드 실패: " + errMsg);
                // 예: 오류 UI 표시
            }
            );
        }
        
    }

}
