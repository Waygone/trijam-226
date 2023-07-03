using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI highscore;

    private void Start()
    {
        float sc = PlayerPrefs.GetFloat("Score");
        float hs = PlayerPrefs.GetFloat("Highscore");

        score.text = "x" + sc.ToString();

        if(sc > hs)
        {
            PlayerPrefs.SetFloat("Highscore", sc);
        }

        highscore.text = "x" + PlayerPrefs.GetFloat("Highscore");
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

}
