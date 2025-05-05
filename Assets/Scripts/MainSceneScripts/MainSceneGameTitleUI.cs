using UnityEngine;
using UnityEngine.UIElements;

public class MainSceneGameTitleUI : MainSceneUIObjectBase
{
    [SerializeField] private Rigidbody _Rig;
    [SerializeField] private float _YPosition;

    private bool _IsDrop = true;

    MainSceneManager _MainSceneManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        _MainSceneManager = GameObject.FindGameObjectWithTag("MainSceneManager").GetComponent<MainSceneManager>();

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (transform.position.y > _YPosition)
        {
            _Rig.useGravity = true;
            _Rig.constraints = RigidbodyConstraints.None;
        }
        else
        {
            _Rig.useGravity = false;
            _Rig.linearVelocity = Vector3.zero;

            // 위치 정확히 고정
            Vector3 tPos = transform.position;
            tPos.y = _YPosition;
            transform.position = tPos;

            _Rig.constraints = RigidbodyConstraints.FreezeAll;

            // 고정 위치에 도달 시 1회만 사운드 재생
            if (_IsDrop)
            {
                _IsDrop = false;
                SoundManager._Inst.PlaySFX(SoundCategory.SFX, "MainSceneObjectDrop");
            }
        }
    }
    
    

    private void OnDestroy()
    {
        if (_MainSceneManager != null)
        {
            _MainSceneManager.RespawnGameTitle();
        }        
    }

}
