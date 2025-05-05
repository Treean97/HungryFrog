using UnityEngine;
using System.Collections;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private float _DissolveDuration = 2f;

    private Material _Material;

    private void Awake()
    {
        Renderer tRenderer = GetComponent<Renderer>();
        if (tRenderer != null)
        {
            _Material = tRenderer.material; // 인스턴싱 필요
        }
    }

    // 외부에서 실행할 수 있도록 코루틴 반환
    public IEnumerator DissolveInCo()
    {
        float tElapsed = 0f;

        while (tElapsed < _DissolveDuration)
        {
            tElapsed += Time.deltaTime;
            float tAmount = Mathf.Lerp(1f, 0f, tElapsed / _DissolveDuration);
            _Material.SetFloat("_Dissolve", tAmount);
            yield return null;
        }

        _Material.SetFloat("_Dissolve", 0f);
    }

    public IEnumerator DissolveOutCo()
    {        
        float tElapsed = 0f;

        while (tElapsed < _DissolveDuration)
        {
            tElapsed += Time.deltaTime;
            float tAmount = Mathf.Lerp(0f, 1f, tElapsed / _DissolveDuration);
            _Material.SetFloat("_Dissolve", tAmount);
            yield return null;
            
        }
        
        _Material.SetFloat("_Dissolve", 1f);
    }
}
