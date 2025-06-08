using System.Text;
using UnityEngine;

public class MainSceneNPCController : MonoBehaviour
{
    [SerializeField] Animator _Animator;  // NPC 애니메이터 컴포넌트

    float _MaxDelaySwitchAni = 10f;      // 애니메이션 전환 최대 지연 시간
    float _MinDelaySwitchAni = 5f;       // 애니메이션 전환 최소 지연 시간

    float _MaxRandomDelay;               // 현재 랜덤 지연 시간
    float _CurRandomDelay;               // 누적된 경과 시간

    void Start()
    {
        // 시작할 때 지연 시간을 랜덤으로 설정
        _MaxRandomDelay = Random.Range(_MinDelaySwitchAni, _MaxDelaySwitchAni);
    }

    void Update()
    {
        // 누적된 시간이 설정된 지연 시간을 넘으면 애니메이션 전환
        if (_CurRandomDelay >= _MaxRandomDelay)
        {
            SwitchAni();
        }

        // Base 레이어의 현재 애니메이터 상태 정보 확인
        AnimatorStateInfo tStateInfo = _Animator.GetCurrentAnimatorStateInfo(0);

        // Idle 상태일 때만 시간 누적
        if (tStateInfo.IsName("Idle"))
        {
            _CurRandomDelay += Time.deltaTime;
        }
    }

    void SwitchAni()
    {
        // 다음 전환을 위한 새로운 랜덤 지연 시간 설정
        _MaxRandomDelay = Random.Range(_MinDelaySwitchAni, _MaxDelaySwitchAni);

        // 경과 시간 초기화
        _CurRandomDelay = 0f;

        // Talk_0 또는 Talk_1 트리거 선택
        int tRandom = Random.Range(0, 2);
        StringBuilder tSB = new StringBuilder();
        tSB.Append("Talk_");
        tSB.Append(tRandom.ToString());

        // 애니메이터에 트리거 전달
        _Animator.SetTrigger(tSB.ToString());
    }
}
