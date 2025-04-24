using System.Text;
using UnityEngine;

public class MainSceneNPCController : MonoBehaviour
{
    [SerializeField]
    Animator _Animator;

    float _MaxDelaySwitchAni = 10;
    float _MinDelaySwitchAni = 5;

    [SerializeField]
    float _MaxRandomDelay;
    [SerializeField]
    float _CurRandomDelay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MaxRandomDelay = Random.Range(_MinDelaySwitchAni, _MaxDelaySwitchAni);

    }

    // Update is called once per frame
    void Update()
    {
        if(_CurRandomDelay >= _MaxRandomDelay)
        {
            SwitchAni();
        }

        // idle�� ���� ī��Ʈ ����
        // 0�� Base Layer�� �ǹ���
        AnimatorStateInfo tStateInfo = _Animator.GetCurrentAnimatorStateInfo(0);

        if (tStateInfo.IsName("Idle"))
        {
            _CurRandomDelay += Time.deltaTime;
        }
    }

    void SwitchAni()
    {
        // ������ �ٽ� ����
        _MaxRandomDelay = Random.Range(_MinDelaySwitchAni, _MaxDelaySwitchAni);

        // ���� �� �ʱ�ȭ
        _CurRandomDelay = 0;

        // 0,1 �� ��ȭ �ִϸ��̼� �� ���� ����
        int tRandom = Random.Range(0, 2);

        StringBuilder tSB = new StringBuilder();
        tSB.Append("Talk_");
        tSB.Append(tRandom.ToString());

        _Animator.SetTrigger(tSB.ToString());

    }



}
