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

    public List<Pool> _Pools; // Ǯ ���
    public Dictionary<string, Queue<GameObject>> _PoolDictionary; // Ǯ ��ųʸ�

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
            return null; // ��û�� Ǯ�� ���� ��� null ��ȯ
        }

        // ��� ������ ������Ʈ�� ã�� ������ �ݺ�
        for (int i = 0; i < _PoolDictionary[tName].Count; i++)
        {
            GameObject tObjectToSpawn = _PoolDictionary[tName].Dequeue();

            // ������Ʈ�� ��Ȱ��ȭ �����̸� ���
            if (!tObjectToSpawn.activeInHierarchy)
            {
                tObjectToSpawn.SetActive(true);
                return tObjectToSpawn;
            }

            // Ȱ��ȭ �����̸� �ٽ� ť�� �ֱ�
            _PoolDictionary[tName].Enqueue(tObjectToSpawn);
        }

        // Ǯ�� ����ְų� ��� Ȱ��ȭ�� ��� ���ο� ������Ʈ ����
        GameObject tNewObject =
            Instantiate(_Pools.Find(p => p.Name == tName).Prefab,
            _Pools.Find(p => p.Name == tName).Parent);
        tNewObject.SetActive(false);
        _PoolDictionary[tName].Enqueue(tNewObject);

        // ���� ������ ������Ʈ�� ��ȯ
        GameObject newObjectToSpawn = _PoolDictionary[tName].Dequeue();
        newObjectToSpawn.SetActive(true);
        return newObjectToSpawn;
    }

    public void ReturnObject(GameObject tObject)
    {
        tObject.SetActive(false);

        foreach (var tPool in _Pools)
        {
            // �����հ� ���� ������Ʈ�� �������� ��
            if (tPool.Prefab.name == tObject.name.Replace("(Clone)", "").Trim())
            {
                _PoolDictionary[tPool.Name].Enqueue(tObject);
                break;
            }
        }
    }
}
