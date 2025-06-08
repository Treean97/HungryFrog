using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // 1) 싱글톤 인스턴스
    public static ObjectPoolManager _Inst;

    // 2) 풀 정보 구조체
    [System.Serializable]
    public class Pool
    {
        public string Name;       // 풀 이름
        public GameObject Prefab; // 인스턴스화할 프리팹
        public int Size;          // 초기 생성 개수
        public Transform Parent;  // 생성된 오브젝트의 부모
    }

    // 3) 풀 리스트
    public List<Pool> _Pools;

    // 4) 풀 딕셔너리 (이름 → 오브젝트 큐)
    public Dictionary<string, Queue<GameObject>> _PoolDictionary;

    private void Awake()
    {
        // 5) 싱글톤 초기화
        _Inst = this;

        // 6) 딕셔너리 생성
        _PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 7) 각 풀에 대해 사전 생성
        foreach (Pool tPool in _Pools)
        {
            Queue<GameObject> tObjectPool = new Queue<GameObject>();

            for (int i = 0; i < tPool.Size; i++)
            {
                GameObject tObj = Instantiate(tPool.Prefab, tPool.Parent);
                tObj.SetActive(false);
                tObjectPool.Enqueue(tObj);
            }

            _PoolDictionary.Add(tPool.Name, tObjectPool);
        }
    }

    // 8) 풀에서 오브젝트 가져오기
    public GameObject GetObject(string tName)
    {
        // 9) 풀 부재 시 null 반환
        if (!_PoolDictionary.ContainsKey(tName))
            return null;

        var tQueue = _PoolDictionary[tName];
        int tCount = tQueue.Count;

        // 10) 비활성 객체 탐색
        for (int i = 0; i < tCount; i++)
        {
            GameObject tObj = tQueue.Dequeue();

            if (!tObj.activeInHierarchy)
            {
                // 11) 비활성 시 활성화 후 반환
                tObj.SetActive(true);
                return tObj;
            }

            // 12) 활성된 객체는 다시 큐에 등록
            tQueue.Enqueue(tObj);
        }

        // 13) 모두 사용 중이면 새 객체 생성
        Pool tInfo = _Pools.Find(p => p.Name == tName);
        GameObject tNewObj = Instantiate(tInfo.Prefab, tInfo.Parent);
        tNewObj.SetActive(false);
        tQueue.Enqueue(tNewObj);

        // 14) 새로 생성한 객체 활성화 후 반환
        GameObject tSpawn = tQueue.Dequeue();
        tSpawn.SetActive(true);
        return tSpawn;
    }

    // 15) 오브젝트 반환
    public void ReturnObject(GameObject tObject)
    {
        tObject.SetActive(false);

        // 16) 해당 풀 찾아서 큐에 재등록
        foreach (var tPool in _Pools)
        {
            if (tPool.Prefab.name == tObject.name.Replace("(Clone)", "").Trim())
            {
                _PoolDictionary[tPool.Name].Enqueue(tObject);
                break;
            }
        }
    }
}
