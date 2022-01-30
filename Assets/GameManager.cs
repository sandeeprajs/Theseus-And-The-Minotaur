using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool playerTurn;
    public bool PlayerTurn { get => playerTurn; set => playerTurn = value; }


    [SerializeField] MinotaurController minotaurController;

    void Start()
    {
        if (instance != null)
            DestroyImmediate(this.gameObject);
        else
            instance = this;

        minotaurController = GameObject.FindObjectOfType<MinotaurController>();

        playerTurn = false;
    }

    void Update()
    {
        if(playerTurn == false)
        {
            if (minotaurController.isMyTurn == false)
            {
                minotaurController.isMyTurn = true;
                minotaurController.MoveTheMinaotaur();
            }
        }
    }
}
