using TMPro;
using UnityEngine;

public class MainSceneDisplayIDUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _DisplayID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    public void UpdateUI()
    {
        _DisplayID.text = PlayFabLeaderboardManager._Inst.DisplayId;
    }

}
