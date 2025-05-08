using System.Collections;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public float _ThrowForce = 10f;
    public float _RotateSpeed;

    private Vector3 _ThrowVector;
    private Vector3 _Velocity;

    private Coroutine _ReturnCoroutine;

    void Update()
    {
        RotateAroundLocalX();
        MoveStraight();
    }

    void RotateAroundLocalX()
    {
        float tRotationAmount = _RotateSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * tRotationAmount);
    }

    public void SetThrowVector(Vector3 tVector)
    {
        _ThrowVector = tVector;
        _Velocity = _ThrowVector * _ThrowForce;

        if (_ReturnCoroutine != null)
            StopCoroutine(_ReturnCoroutine);

        _ReturnCoroutine = StartCoroutine(ReturnToPoolAfterSeconds(5f));
    }

    private void MoveStraight()
    {
        transform.position += _Velocity * Time.deltaTime;
    }

    private IEnumerator ReturnToPoolAfterSeconds(float tDelay)
    {
        yield return new WaitForSeconds(tDelay);
        ObjectPoolManager._Inst.ReturnObject(gameObject);
    }
}
