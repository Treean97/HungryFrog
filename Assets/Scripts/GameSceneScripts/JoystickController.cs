using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform _BackGround;
    [SerializeField] RectTransform _Handle;
    public Vector2 _InputDirection { get; private set; }
    public static int _TouchID { get; private set; } = -1;

    // 입력이 바뀔 때 구독할 수 있도록 이벤트로도 노출
    public event Action<Vector2> OnInputChanged;

    public void OnPointerDown(PointerEventData e)
    {
        _TouchID = e.pointerId;
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 tDelta = (Vector2)e.position - (Vector2)_BackGround.position;
        _InputDirection = tDelta.normalized;
        float tDist = Mathf.Clamp(tDelta.magnitude, 0, _BackGround.sizeDelta.x / 2f);
        _Handle.anchoredPosition = _InputDirection * tDist;
        OnInputChanged?.Invoke(_InputDirection);
    }

    public void OnPointerUp(PointerEventData e)
    {
        _TouchID = -1;
        _InputDirection = Vector2.zero;
        _Handle.anchoredPosition = Vector2.zero;
        OnInputChanged?.Invoke(_InputDirection);
    }
}
