using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonDimChildren : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 눌렀을 때 적용할 색상 (RGB 200,200,200)
    private readonly Color32 _PressedColor = new Color32(200, 200, 200, 255);

    private List<Graphic> _ChildGraphics = new List<Graphic>();           // 자식 UI 그래픽들
    private Dictionary<Graphic, Color> _OriginalColors = new Dictionary<Graphic, Color>(); // 원본 색상 저장

    private void Awake()
    {
        // 자신과 자식 객체에서 모든 Graphic 컴포넌트(Image, Text 등)를 가져옴
        GetComponentsInChildren(true, _ChildGraphics);

        // 각 그래픽의 원본 색상을 저장
        foreach (var g in _ChildGraphics)
        {
            _OriginalColors[g] = g.color;
        }
    }

    // 포인터(터치/클릭) 누름 시작 시 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var g in _ChildGraphics)
        {
            // 눌린 색상으로 변경하되 원본 알파값은 유지
            Color newColor = _PressedColor;
            newColor.a = _OriginalColors[g].a;
            g.color = newColor;
        }
    }

    // 포인터 떼었을 때 호출: 색상 복원
    public void OnPointerUp(PointerEventData eventData)
    {
        RestoreColors();
    }

    // 포인터가 버튼 영역에서 나갔을 때 호출: 색상 복원
    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreColors();
    }

    // 원본 색상으로 복원
    private void RestoreColors()
    {
        foreach (var g in _ChildGraphics)
        {
            g.color = _OriginalColors[g];
        }
    }
}
