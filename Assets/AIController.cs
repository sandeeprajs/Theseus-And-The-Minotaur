using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.5f;

    public Vector3 destination;
    public bool isMoving = false;
    public iTween.EaseType easeType = iTween.EaseType.linear;

    public float delay = 0f;

    public Transform targetTrans;

    [SerializeField]
    private bool isMyTurn = false;
    public float rayRange = 1f;
    private RaycastHit2D[] hits;

    public bool canHeMoveVertical;

    public enum HorizontalMoveStatus
    {
        Left, Right, NoMove
    }

    public HorizontalMoveStatus horzMove;

    public enum VerticalMoveStatus
    {
        Up, Down, NoMove
    }

    public VerticalMoveStatus vertzMove;

    public int moveCount;

    public bool IsMyTurn { get => isMyTurn; set => isMyTurn = value; }

    public float startMoveDelay = 0.25f;

    void Start()
    {
        horzMove = HorizontalMoveStatus.NoMove;
        vertzMove = VerticalMoveStatus.NoMove;

        canHeMoveVertical = false;

        StartCoroutine(MoveTheAI());
    }


    IEnumerator MoveTheAI()
    {
        while(IsMyTurn)
        {
            if (canHeMoveVertical)
            {
                // Move Vertical
                IsPlayerOnTop();

                switch (vertzMove)
                {
                    case VerticalMoveStatus.Up:
                        hits = Physics2D.RaycastAll(transform.position, transform.up, rayRange);

                        HitResult(hits);
                        Move(destination);
                        break;
                    case VerticalMoveStatus.Down:
                        hits = Physics2D.RaycastAll(transform.position, -transform.up, rayRange);

                        HitResult(hits);
                        Move(destination);
                        break;
                    case VerticalMoveStatus.NoMove:
                        canHeMoveVertical = false;
                        IsMyTurn = false;
                        break;
                    default:
                        break;
                }

                canHeMoveVertical = false;
            }
            else
            {
                // Move Horizontal
                IsPlayerOnLeft();
                switch (horzMove)
                {
                    case HorizontalMoveStatus.Left:
                        hits = Physics2D.RaycastAll(transform.position, -transform.right, rayRange);

                        HitResult(hits);
                        Move(destination);
                        break;
                    case HorizontalMoveStatus.Right:

                        hits = Physics2D.RaycastAll(transform.position, transform.right, rayRange);

                        HitResult(hits);
                        Move(destination);
                        break;
                    case HorizontalMoveStatus.NoMove:
                        IsMyTurn = false;
                        break;
                    default:
                        break;
                }
            }
            IsMyTurn = false;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(MoveTheAI());
    }

  
    void IsPlayerOnLeft()
    {
        float xDiff = targetTrans.position.x - transform.position.x;

        if (xDiff < 0)
        {
            Debug.Log("Move Left" + xDiff);
            horzMove = HorizontalMoveStatus.Left;
        }
        else if (xDiff == 0)
        {
            Debug.Log("ZERO");
            horzMove = HorizontalMoveStatus.NoMove;
            canHeMoveVertical = true;
            StopCoroutine(MoveTheAI());
            StartCoroutine(MoveTheAI());
        }
        else
        {
            Debug.Log("Move Right" + xDiff);
            horzMove = HorizontalMoveStatus.Right;
        }
    }

    void IsPlayerOnTop()
    {
        float yDiff = targetTrans.position.y - transform.position.y;

        if (yDiff > 0)
        {
            Debug.Log("Move Top" + yDiff);
            vertzMove = VerticalMoveStatus.Up;
        }
        else if (yDiff == 0)
        {
            Debug.Log("ZERO");
            vertzMove = VerticalMoveStatus.NoMove;
            canHeMoveVertical = false;
        }
        else
        {
            Debug.Log("Move Down" + yDiff);
            vertzMove = VerticalMoveStatus.Down;
        }
    }

    void HitResult(RaycastHit2D[] _hits)
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
                IsMyTurn = false;
                destination = transform.position;
            }
            else
            {
                if (_hits[1].transform.tag == "Tile" || _hits[1].transform.tag == "Player")
                {
                    Debug.Log("Destination is Set Here " + _hits[1].transform.name);

                    destination = _hits[1].transform.position;

                    IsMyTurn = true;
                    moveCount++;
                }
                else
                {
                    Debug.Log("Destination is Set Here " + transform.position);

                    destination = transform.position;
                    IsMyTurn = false;
                }
            }
        }
        else
            IsMyTurn = false;
    }

    public void Move(Vector3 destination, float delay = 0.35f)
    {
        StartCoroutine(MoveRoutine(destination, delay));
    }

    IEnumerator MoveRoutine(Vector3 _destination, float delay)
    {
        IsMyTurn = true;
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

        IsMyTurn = false;
        isMoving = false;
        
        horzMove = HorizontalMoveStatus.NoMove;
        canHeMoveVertical = false;

        if (moveCount == 1)
        {
            IsMyTurn = true;
            StopAllCoroutines();
            StartCoroutine(MoveTheAI());
        }
    }

    void StopMovement()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        switch (horzMove)
        {
            case HorizontalMoveStatus.Left:
                Debug.DrawRay(transform.position, -transform.right * rayRange);
                break;
            case HorizontalMoveStatus.Right:
                Debug.DrawRay(transform.position, transform.right * rayRange);
                break;
            case HorizontalMoveStatus.NoMove:
                break;
            default:
                break;
        }

        switch (vertzMove)
        {
            case VerticalMoveStatus.Up:
                Debug.DrawRay(transform.position, transform.up * rayRange);
                break;
            case VerticalMoveStatus.Down:
                Debug.DrawRay(transform.position, -transform.up * rayRange);
                break;
            case VerticalMoveStatus.NoMove:
                break;
            default:
                break;
        }

        if (IsMyTurn)
        {
            GameManager.instance.PlayerTurn = true;

            if (moveCount >= 2)
            {
                IsMyTurn = false;
                moveCount = 0;
            }


            StopMovement();
            StartCoroutine(MoveTheAI());
        }
        else
        {
            if (GameManager.instance.PlayerTurn == false)
            {
                startMoveDelay -= Time.deltaTime;
                if (startMoveDelay <= 0)
                {
                    startMoveDelay = 0.25f;
                    IsMyTurn = true;
                }
            }
            else
                IsMyTurn = false;
        }
       
        

        rayRange = moveCount == 1 ? rayRange = 1 : rayRange = 2;
    }
}
