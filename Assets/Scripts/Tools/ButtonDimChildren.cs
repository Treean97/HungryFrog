using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonDimChildren : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // ��Ȯ�� RGB(200, 200, 200), ���Ĵ� ������ ����
    private readonly Color32 _PressedColor = new Color32(200, 200, 200, 255);

    private List<Graphic> _ChildGraphics = new List<Graphic>();
    private Dictionary<Graphic, Color> _OriginalColors = new Dictionary<Graphic, Color>();

    private void Awake()
    {
        // �ڱ� �ڽ� ���� ��� �ڽ��� Graphic (Image, Text ��) ��������
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
            newColor.a = _OriginalColors[g].a; // ���� ���İ� ����
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
