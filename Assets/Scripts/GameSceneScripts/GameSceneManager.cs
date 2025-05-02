using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // Planet ������Ʈ�� ����
    int _MaxID;

    // ������ �ð�
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



    // A, B�� �浹
    public void CollisionObject(GameObject tObject_A, GameObject tObject_B)
    {
        // Object_A�� B�� ID
        int tObjectID = tObject_A.gameObject.GetComponent<ShootObjectBasement>().GetShootObjectData.GetShootObjectID;

        // ���̻� �������� ���ٸ� �׳� ����
        if (tObjectID >= _MaxID)
        {
            return;
        }

        // ������Ʈ ����
        ReturnObject(tObject_A, tObject_B);

       // ���ο� ������Ʈ ID�� ������ ID + 1
        int tNewObjectID = tObjectID + 1;

        // �̸��� ã�Ƽ� Ǯ���� Get
        string tNewObjectName = ObjectPoolManager._Inst._Pools[tNewObjectID].Name;

        // ���ο� ������Ʈ�� ��ġ ���� -> ���� ������Ʈ A�� B�� �߰�����
        Vector3 tSpawnPos = (tObject_A.transform.position + tObject_B.transform.position) / 2f;

        // ���ο� ������Ʈ Get
        GetObject(tNewObjectName.ToString(), tSpawnPos);


    }

    // ������Ʈ�� �޾� ����
    void ReturnObject(GameObject tObject_A, GameObject tObject_B)
    {
        ObjectPoolManager._Inst.ReturnObject(tObject_A);
        ObjectPoolManager._Inst.ReturnObject(tObject_B);
    }

    // ID�� �޾� ����
    void GetObject(string tName, Vector3 tSpawnPos)
    {
        GameObject tGO = ObjectPoolManager._Inst.GetObject(tName);
        tGO.transform.position = tSpawnPos;
    }
}
