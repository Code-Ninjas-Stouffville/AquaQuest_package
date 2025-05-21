using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Score : MonoBehaviour
{
    public TMP_Text scoreText;

    public int score = 0;

    public Sprite[] rods;
    public SpriteRenderer rod;
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score; 

    }
    public void Update()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score > 45)
        {
            rod.sprite = rods[1];
            
        }

        if (score > 60)
        {
            rod.sprite = rods[2];
            Debug.Log("Load new screen");
            
        }
        if (score > 100)
        {
            SceneManager.LoadScene("MiniGame");
        }

    }
    public void AddScore(int addScore)
    {
        score = score + addScore;
    }


}
