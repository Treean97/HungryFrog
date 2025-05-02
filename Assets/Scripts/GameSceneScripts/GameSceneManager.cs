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

    [SerializeField]
    GameObject _TouchBlockUI;

    // Start is called before the first frame update
    void Start()
    {
        _MaxID = ObjectPoolManager._Inst._Pools.Count - 1;

        SoundManager._Inst.PlayBGM("GameSceneBGM");

        StartCoroutine(CantTouchDurationTimeCoroutine(GetOpeningDuration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CanTouch()
    {
        _TouchBlockUI.SetActive(false);
    }

    void CantTouch()
    {
        _TouchBlockUI.SetActive(true);
    }

    IEnumerator CantTouchDurationTimeCoroutine(int tTime)
    {
        CantTouch();

        yield return new WaitForSeconds(tTime);

        CanTouch();
    }



    // A, B의 충돌
    public void CollisionObject(GameObject tObject_A, GameObject tObject_B)
    {
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
}
