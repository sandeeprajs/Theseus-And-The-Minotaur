using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheseusController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Vector3 destPos;
    [SerializeField] bool canMoveLeft, canMoveRight, canMoveUp, canMoveDown;

    [Header("RayCast")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask wallLayerMask;
    private RaycastHit2D castLeft, castRight, castUp, castDown;

    void Start()
    {
        destPos.x = transform.position.x;
        destPos.y = transform.position.y;

        //canMoveUp = true;
        //canMoveDown = true;
        //canMoveLeft = true;
        //canMoveRight = true;
    }

    void Update()
    {
        if (Time.timeScale == 0 || GameManager.instance.isGameOver)
            return; 

        transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destPos) <= 0.05f)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && canMoveLeft)
            {
                destPos.x -= 1f;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && canMoveRight)
            {
                destPos.x += 1f;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && canMoveUp)
            {
                destPos.y += 1f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && canMoveDown)
            {
                destPos.y -= 1f;
            }

            //if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) // Move in X
            //{
                //if (Input.GetAxisRaw("Horizontal") == 1 && canMoveRight)
                //    destPos.x += 1f;
                //else if (Input.GetAxisRaw("Horizontal") == -1 && canMoveLeft)
                //    destPos.x -= 1f;
                //else
                //    destPos.x = transform.position.x;
                //}
                //else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1) // Move in Y
                //{
                //    if (Input.GetAxisRaw("Vertical") == 1 && canMoveUp)
                //        destPos.y += 1f;
                //    else if (Input.GetAxisRaw("Vertical") == -1 && canMoveDown)
                //        destPos.y -= 1f;
                //    else
                //        destPos.y = transform.position.y;
                //}
            //}
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            GameManager.instance.PlayerTurn = false;
        }

    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, -transform.right * rayDistance);
        //Debug.DrawRay(transform.position, transform.right * rayDistance);
        //Debug.DrawRay(transform.position, transform.up * rayDistance);
        //Debug.DrawRay(transform.position, -transform.up * rayDistance);

        castLeft = Physics2D.Raycast(transform.position, -transform.right, rayDistance, wallLayerMask);
        castRight= Physics2D.Raycast(transform.position, transform.right, rayDistance, wallLayerMask);
        castUp = Physics2D.Raycast(transform.position, transform.up, rayDistance, wallLayerMask);
        castDown = Physics2D.Raycast(transform.position, -transform.up, rayDistance, wallLayerMask);

        if (castLeft.collider != null)
        {
            canMoveLeft = false;
        }
        else
            canMoveLeft = true;

        if (castRight.collider != null)
        {
            canMoveRight = false;
        }
        else
            canMoveRight = true;

        if (castUp.collider != null)
        {
            canMoveUp = false;
        }
        else
            canMoveUp = true;

        if (castDown.collider != null)
        {
            canMoveDown = false;
        }
        else
            canMoveDown = true;
    }
}
