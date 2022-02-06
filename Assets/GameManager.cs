using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool playerTurn;
    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }

    public bool isGameOver;
    

    [SerializeField] MinotaurController minotaurController;
    [SerializeField] TheseusController theseusController;
    private void Awake()
    {
        if (instance != null)
            DestroyImmediate(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {        
        minotaurController = GameObject.FindObjectOfType<MinotaurController>();
        theseusController = GameObject.FindObjectOfType<TheseusController>();

        playerTurn = false;
        isGameOver = false;
    }

    void Update()
    {
        if(minotaurController.transform.position == theseusController.transform.position)
        {
            isGameOver = true;

            if(Input.GetKeyDown(KeyCode.R))
            {
                GameOver();
            }

        }

        if(playerTurn == false)
        {
            if (minotaurController.isMyTurn == false)
            {
                minotaurController.isMyTurn = true;
                minotaurController.MoveTheMinaotaur();
            }
        }
    }

    public void LevelCompleted()
    {
        LevelLoader.instance.LoadNextLevel();
    }

    public void GameOver()
    {
        isGameOver = true;
        LevelLoader.instance.Restart();
    }
}
