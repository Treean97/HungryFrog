using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSceneScoreUI : MonoBehaviour
{

    [SerializeField]
    TMP_Text _ScoreText;

    int _Score = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int tAddScore)
    {
        _Score += tAddScore;

        SetScore();
    }

    void SetScore()
    {
        _ScoreText.text = _Score.ToString();
    }
}
