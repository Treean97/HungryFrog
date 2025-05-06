using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonDimChildren : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 정확히 RGB(200, 200, 200), 알파는 원래값 유지
    private readonly Color32 _PressedColor = new Color32(200, 200, 200, 255);

    private List<Graphic> _ChildGraphics = new List<Graphic>();
    private Dictionary<Graphic, Color> _OriginalColors = new Dictionary<Graphic, Color>();

    private void Awake()
    {
        // 자기 자신 포함 모든 자식의 Graphic (Image, Text 등) 가져오기
        GetComponentsInChildren(true, _ChildGraphics);

        foreach (var g in _ChildGraphics)
        {
            _OriginalColors[g] = g.color;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var g in _ChildGraphics)
        {
            Color newColor = _PressedColor;
            newColor.a = _OriginalColors[g].a; // 원래 알파값 유지
            g.color = newColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RestoreColors();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreColors();
    }

    private void RestoreColors()
    {
        foreach (var g in _ChildGraphics)
        {
            g.color = _OriginalColors[g];
        }
    }
}
