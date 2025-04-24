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

        // idle일 때만 카운트 증가
        // 0은 Base Layer를 의미함
        AnimatorStateInfo tStateInfo = _Animator.GetCurrentAnimatorStateInfo(0);

        if (tStateInfo.IsName("Idle"))
        {
            _CurRandomDelay += Time.deltaTime;
        }
    }

    void SwitchAni()
    {
        // 랜덤값 다시 생성
        _MaxRandomDelay = Random.Range(_MinDelaySwitchAni, _MaxDelaySwitchAni);

        // 현재 값 초기화
        _CurRandomDelay = 0;

        // 0,1 번 대화 애니메이션 중 랜덤 실행
        int tRandom = Random.Range(0, 2);

        StringBuilder tSB = new StringBuilder();
        tSB.Append("Talk_");
        tSB.Append(tRandom.ToString());

        _Animator.SetTrigger(tSB.ToString());

    }



}
