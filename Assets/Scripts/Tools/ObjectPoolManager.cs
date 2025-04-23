using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager _Inst;

    [System.Serializable]
    public class Pool
    {
        public string Name;
        public GameObject Prefab;
        public int Size;
        public Transform Parent;
    }

    public List<Pool> _Pools; // 풀 목록
    public Dictionary<string, Queue<GameObject>> _PoolDictionary; // 풀 딕셔너리

    void Awake()
    {
        _Inst = this;

        _PoolDictionary = new Dictionary<string, Queue<GameObject>>();

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

    public GameObject GetObject(string tName)
    {
        if (!_PoolDictionary.ContainsKey(tName))
        {
            return null; // 요청한 풀이 없는 경우 null 반환
        }

        // 사용 가능한 오브젝트를 찾을 때까지 반복
        for (int i = 0; i < _PoolDictionary[tName].Count; i++)
        {
            GameObject tObjectToSpawn = _PoolDictionary[tName].Dequeue();

            // 오브젝트가 비활성화 상태이면 사용
            if (!tObjectToSpawn.activeInHierarchy)
            {
                tObjectToSpawn.SetActive(true);
                return tObjectToSpawn;
            }

            // 활성화 상태이면 다시 큐에 넣기
            _PoolDictionary[tName].Enqueue(tObjectToSpawn);
        }

        // 풀이 비어있거나 모두 활성화된 경우 새로운 오브젝트 생성
        GameObject tNewObject =
            Instantiate(_Pools.Find(p => p.Name == tName).Prefab,
            _Pools.Find(p => p.Name == tName).Parent);
        tNewObject.SetActive(false);
        _PoolDictionary[tName].Enqueue(tNewObject);

        // 새로 생성한 오브젝트를 반환
        GameObject newObjectToSpawn = _PoolDictionary[tName].Dequeue();
        newObjectToSpawn.SetActive(true);
        return newObjectToSpawn;
    }

    public void ReturnObject(GameObject tObject)
    {
        tObject.SetActive(false);

        foreach (var tPool in _Pools)
        {
            // 프리팹과 실제 오브젝트의 프리팹을 비교
            if (tPool.Prefab.name == tObject.name.Replace("(Clone)", "").Trim())
            {
                _PoolDictionary[tPool.Name].Enqueue(tObject);
                break;
            }
        }
    }
}
