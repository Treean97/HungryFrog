using UnityEngine;
using System.Collections;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private float _DissolveDuration = 2f; // 디졸브 진행 시간

    private Material _Material; // 사용할 머티리얼 참조

    private void Awake()
    {
        // 렌더러에서 인스턴스 머티리얼 가져오기
        Renderer tRenderer = GetComponent<Renderer>();
        if (tRenderer != null)
        {
            _Material = tRenderer.material;
        }
    }

    // 1) 대상에게 서서히 나타나는 디졸브 효과
    public IEnumerator DissolveInCo()
    {
        float tElapsed = 0f;

        // 경과 시간에 따라 _Dissolve 값을 1→0으로 보간
        while (tElapsed < _DissolveDuration)
        {
            tElapsed += Time.deltaTime;
            float tAmount = Mathf.Lerp(1f, 0f, tElapsed / _DissolveDuration);
            _Material.SetFloat("_Dissolve", tAmount);
            yield return null;
        }

        // 완료 후 확실히 0 설정
        _Material.SetFloat("_Dissolve", 0f);
    }

    // 2) 대상에게 서서히 사라지는 디졸브 효과
    public IEnumerator DissolveOutCo()
    {
        float tElapsed = 0f;

        // 경과 시간에 따라 _Dissolve 값을 0→1로 보간
        while (tElapsed < _DissolveDuration)
        {
            tElapsed += Time.deltaTime;
            float tAmount = Mathf.Lerp(0f, 1f, tElapsed / _DissolveDuration);
            _Material.SetFloat("_Dissolve", tAmount);
            yield return null;
        }

        // 완료 후 확실히 1 설정
        _Material.SetFloat("_Dissolve", 1f);
    }
}
