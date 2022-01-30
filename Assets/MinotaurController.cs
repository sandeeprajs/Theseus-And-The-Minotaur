using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    public enum MinotaurMoveDirection
    {
        None, Left, Right, Top, Down 
    }

    [Header("TwinMotion")]
    [SerializeField] bool isMoving;
    [SerializeField] float moveSpeed = 1.25f;
    [SerializeField] iTween.EaseType easeType = iTween.EaseType.linear;

    [Header("Movement")]
    public bool isMyTurn;
    [SerializeField] int moveCount;
    [SerializeField] float moveDelay;
    [SerializeField] Vector3 destination;

    [Header("Raycasting")]
    [SerializeField] float rayDistance = 1f;
    [SerializeField] Transform hitObjectTrans;
    [SerializeField] RaycastHit2D[] hits;

    [Header("ENUM Directions")]
    [SerializeField] MinotaurMoveDirection minotaurMoveDirection;
    [SerializeField] Transform moveTargetTrans;

    [Header("Player")]
    [SerializeField] Transform playerTrans;
    [SerializeField] float xDistance, yDistance;

    private void Awake()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Start()
    {
        moveDelay = 0.25f;
        isMoving = false;
        //isMyTurn = true;

        minotaurMoveDirection = MinotaurMoveDirection.None;

       // MoveTheMinaotaur();
    }

    private void Update()
    {
        rayDistance = moveCount == 1 ? rayDistance = 1 : rayDistance = 2;
        xDistance = playerTrans.position.x - transform.position.x;

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W Pressed ");
            isMyTurn = true;
            MoveTheMinaotaur();
        }
    }

    public void MoveTheMinaotaur ()
    {
        if(isMyTurn)
        {
            if (playerTrans.position.x - transform.position.x < 0) // Player is in left
            {
                minotaurMoveDirection = MinotaurMoveDirection.Left;

                hits = Physics2D.RaycastAll(transform.position, -transform.right, rayDistance);
                Transform hitObjectL = HitResult(hits);

                Move(moveTargetTrans.position);
            }
            else if(playerTrans.position.x - transform.position.x > 0)
            {
                minotaurMoveDirection = MinotaurMoveDirection.Right;

                hits = Physics2D.RaycastAll(transform.position, transform.right, rayDistance);
                Transform hitObjectR = HitResult(hits);

                if (moveTargetTrans.name == "AI")
                {
                    Debug.Log("Hit SELF");

                    if ((playerTrans.position.y - transform.position.y) < 0f)
                    {
                        hits = Physics2D.RaycastAll(transform.position, -transform.up, rayDistance);
                        HitResult(hits);
                    }
                    else if ((playerTrans.position.y - transform.position.y) > 0f)
                    {
                        hits = Physics2D.RaycastAll(transform.position, transform.up, rayDistance);
                        HitResult(hits);
                    }
                    else
                        moveTargetTrans = this.transform;
                }
                else
                {
                    isMyTurn = false;
                    Move(this.transform.position);
                }

                Move(moveTargetTrans.position);

            }
            else if(playerTrans.position.x - transform.position.x == 0)
            {
                if (playerTrans.position.y - transform.position.y < 0)
                {
                    minotaurMoveDirection = MinotaurMoveDirection.Down;

                    hits = Physics2D.RaycastAll(transform.position, -transform.up, rayDistance);
                    Transform hitObjectD = HitResult(hits);
                    
                    if (moveTargetTrans.name == "AI")
                    {
                        Debug.Log("Hit SELF");

                        if ((playerTrans.position.x - transform.position.x) < 0f)
                        {
                            hits = Physics2D.RaycastAll(transform.position, -transform.right, rayDistance);
                            HitResult(hits);
                        }
                        else if ((playerTrans.position.x - transform.position.x) > 0f)
                        {
                            hits = Physics2D.RaycastAll(transform.position, transform.right, rayDistance);
                            HitResult(hits);
                        }
                        else
                            moveTargetTrans = this.transform;
                    }
                    else
                    {
                        isMyTurn = false;
                        Move(this.transform.position);
                    }

                    Move(moveTargetTrans.position);
                }
                else if (playerTrans.position.y - transform.position.y > 0)
                {
                    minotaurMoveDirection = MinotaurMoveDirection.Top;

                    hits = Physics2D.RaycastAll(transform.position, transform.up, rayDistance);
                    Transform hitObjectYU = HitResult(hits);

                    if (moveTargetTrans.name == "AI")
                    {
                        Debug.Log("Hit SELF");

                        if ((playerTrans.position.x - transform.position.x) < 0f)
                        {
                            hits = Physics2D.RaycastAll(transform.position, -transform.right, rayDistance);
                            HitResult(hits);
                        }
                        else if ((playerTrans.position.x - transform.position.x) > 0f)
                        {
                            hits = Physics2D.RaycastAll(transform.position, transform.right, rayDistance);
                            HitResult(hits);
                        }
                        else
                            moveTargetTrans = this.transform;
                    }
                    else
                    {
                        isMyTurn = false;
                        Move(this.transform.position);
                    }

                    Move(moveTargetTrans.position);
                }
                else
                    moveTargetTrans = this.transform;
                
                Move(moveTargetTrans.position);
            }
        }        
    }
        
    Transform HitResult(RaycastHit2D[] _hits)
    {
        if (_hits != null)
        {
            for (int i = 0; i < _hits.Length; i++)
            {
                Debug.Log("Place [" + i + "]" + _hits[i].transform.name);
            }

            if (_hits[1].transform.tag == "Wall")
            {
                // Do Nothing
                isMyTurn = false;


                Debug.Log("Hit wall in direction of: " + minotaurMoveDirection);
                return moveTargetTrans = this.transform;
            }
            else
            {
                if (_hits[1].transform.tag == "Tile" || _hits[1].transform.tag == "Player")
                {
                    Debug.Log("Destination is Set Here " + _hits[1].transform.name);

                    isMyTurn = true;

                    return moveTargetTrans = _hits[1].transform;
                }
                else
                {
                    Debug.Log("Destination is Set Here " + transform.position);

                    isMyTurn = false;

                    return moveTargetTrans = this.transform;
                }
            }
        }
        else
        {
            isMyTurn = false;
            return moveTargetTrans = transform;
        }
    }

    public void Move(Vector3 destination, float delay = 0.25f)
    {
        StartCoroutine(MoveRoutine(destination, delay));
    }

    IEnumerator MoveRoutine(Vector3 _destination, float delay)
    {
        isMyTurn = true;
        isMoving = true;

        destination = _destination;

        yield return new WaitForSeconds(delay);
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", _destination.x,
            "y", _destination.y,
            "z", 0f,
            "delay", delay,
            "easetype", easeType,
            "speed", moveSpeed
            ));

        while (Vector3.Distance(destination, transform.position) > 0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destination;
                
        isMyTurn = false;
        isMoving = false;

        minotaurMoveDirection = MinotaurMoveDirection.None;
        moveCount++;


        if (moveCount == 1)
        {
            isMyTurn = true;
            StopAllCoroutines();

            MoveTheMinaotaur();
        }

        if (moveCount >= 2)
        {
            StopAllCoroutines();
            moveCount = 0;
            isMyTurn = false;
            GameManager.instance.PlayerTurn = true;
        }
    }
}
