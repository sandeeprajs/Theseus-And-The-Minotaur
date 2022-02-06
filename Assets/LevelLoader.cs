using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;


    private void Awake()
    {
        if (instance != null)
            DestroyImmediate(this.gameObject);
        else
            instance = this;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = ((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);

        nextSceneIndex = Mathf.Clamp(nextSceneIndex, 0, nextSceneIndex);

        SceneManager.LoadScene(nextSceneIndex);
    }
}
