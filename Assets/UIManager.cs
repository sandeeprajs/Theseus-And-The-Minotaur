using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseText;
    [SerializeField] GameObject gameOverText;

    private void Start()
    {
        pauseText.SetActive(false);
        gameOverText.SetActive(false);
    }

    public void Restart()
    {
        LevelLoader.instance.Restart();
    }

    public void PlayOrWait()
    {
        if(Time.timeScale == 0)
        {
            pauseText.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            pauseText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void LoadNextLevel()
    {
        LevelLoader.instance.LoadNextLevel();
    }


    private void Update()
    {
        if(GameManager.instance.isGameOver)
            gameOverText.SetActive(true);
        else
            gameOverText.SetActive(false);
    }
}
