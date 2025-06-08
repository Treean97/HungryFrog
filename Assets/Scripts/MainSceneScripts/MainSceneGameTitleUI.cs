using UnityEngine;
using UnityEngine.UIElements;

public class MainSceneGameTitleUI : MainSceneUIObjectBase
{
    [SerializeField] private Rigidbody _Rig;       // 타이틀 오브젝트의 Rigidbody
    [SerializeField] private float _YPosition;     // 최종 고정 Y 위치

    private bool _IsDrop = true;                   // 첫 낙하 여부 플래그

    MainSceneManager _MainSceneManager;             // 메인 씬 매니저 참조

    protected override void Start()
    {
        base.Start();

        // 메인 씬 매니저 가져오기
        _MainSceneManager = GameObject
            .FindGameObjectWithTag("MainSceneManager")
            .GetComponent<MainSceneManager>();
    }

    protected override void Update()
    {
        // 현재 높이가 목표보다 높으면 중력 적용하여 낙하
        if (transform.position.y > _YPosition)
        {
            _Rig.useGravity = true;
            _Rig.constraints = RigidbodyConstraints.None;
        }
        else
        {
            // 목표 위치 이하로 내려오면 중지 및 위치 고정
            _Rig.useGravity = false;
            _Rig.linearVelocity = Vector3.zero;

            Vector3 tPos = transform.position;
            tPos.y = _YPosition;
            transform.position = tPos;

            _Rig.constraints = RigidbodyConstraints.FreezeAll;

            // 첫 낙하 시 사운드 재생
            if (_IsDrop)
            {
                _IsDrop = false;
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "MainSceneObjectDrop");
            }
        }
    }
    
    private void OnDestroy()
    {
        // 오브젝트 제거 시 메인 씬 매니저에 리스폰 요청
        if (_MainSceneManager != null)
            _MainSceneManager.RespawnGameTitle();
    }
}
