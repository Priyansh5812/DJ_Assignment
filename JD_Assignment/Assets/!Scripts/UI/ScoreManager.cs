using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreGUI;
    private int score = 0;
    void Start()
    {
        scoreGUI.text = score.ToString();
        ProjectileEvent.Service.onProjectileHit.AddListener(AddScore);   
    }

    



    private void AddScore(int score)
    {
        this.score += score;
        scoreGUI.text = this.score.ToString();
    }
}
