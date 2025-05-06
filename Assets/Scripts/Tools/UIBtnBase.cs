using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBtnBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool _IsBlocking = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _IsBlocking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _IsBlocking = false;
    }    

}
