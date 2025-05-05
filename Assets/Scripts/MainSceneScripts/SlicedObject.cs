using UnityEngine;
using System.Collections;

public class SlicedObject : MonoBehaviour
{
    [Header("Dissolve Set")]
    [SerializeField] private float _DissolveDuration = 1.0f;

    private DissolveEffect _Dissolve;

    MainSceneManager _MainSceneManager;


    private void Start()
    {
        _MainSceneManager = GameObject.FindGameObjectWithTag("MainSceneManager").GetComponent<MainSceneManager>();

        _Dissolve = GetComponent<DissolveEffect>();

        StartCoroutine(DestroyAfterDissolve());
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager._Inst.PlayRandomSFX(SoundCategory.MainSceneSlicedObjectCollision);
    }

    private IEnumerator DestroyAfterDissolve()
    {       
        yield return StartCoroutine(_Dissolve.DissolveOutCo()); // 디졸브 끝날 때까지 기다림
        Destroy(gameObject); // 이후에 제거
    }

}
