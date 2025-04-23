using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalForceUI : MonoBehaviour
{
    [SerializeField]
    Sprite[] _ForceDirectionSpriteArray;

    [SerializeField]
    Image _AdditionalForceImage;

    [SerializeField]
    TMP_Text _AdditionalForceText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetAdditionalForceText(float tForce)
    {
        _AdditionalForceText.text = tForce.ToString();
    }

    public void SetAdditionalForceImage(int tIndex)
    {
        _AdditionalForceImage.sprite = _ForceDirectionSpriteArray[tIndex];
    }
}
