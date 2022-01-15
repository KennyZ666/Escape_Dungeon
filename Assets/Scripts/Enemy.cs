using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 1;
    public float chaseLength = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Random move purpose
    public Transform movePos;
    public Transform leftDownPos;
    public Transform rightUpPos;
    public float waitTime;
    private float remainWaitTime;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        remainWaitTime = waitTime;
        movePos.position = GetRandomPos();
        
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(startingPosition, new Vector3(movePos.position.x, movePos.position.y, 0), Color.white, 5.0f, true);
        // Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            //chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;
            //when distance < triggerlength, stop chasing(wrong logic)
            if (Vector3.Distance(playerTransform.position, transform.position) < triggerLength)
            {       // the correct logic is that the enemy will stop chasing only if the player is out of the chaseLength
                chasing = true;
            }

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                //Debug.DrawLine(startingPosition, new Vector3(movePos.position.x, movePos.position.y, 0), Color.white, 5.0f, true);
                if (Vector2.Distance(transform.position, movePos.position) < 0.1f)
                {
                    if (remainWaitTime <= 0)
                    {
                        movePos.position = GetRandomPos();
                        UpdateMotor(movePos.position - transform.position);
                        remainWaitTime = waitTime;
                    }
                    else
                    {
                        UpdateMotor(movePos.position - transform.position);
                        remainWaitTime -= Time.deltaTime;
                    }
                }
                UpdateMotor(movePos.position - transform.position);
            }
        }
        else //the player is out of chasing range: back to the startingPos & start wandering
        {
            //UpdateMotor(startingPosition - transform.position);
            chasing = false;
            if (Vector2.Distance(transform.position, movePos.position) < 0.1f)
            {
                if (remainWaitTime <= 0)
                {
                    movePos.position = GetRandomPos();
                    UpdateMotor(movePos.position - transform.position);
                    remainWaitTime = waitTime;
                }
                else
                {
                    UpdateMotor(movePos.position - transform.position);
                    remainWaitTime -= Time.deltaTime;
                }
            }
            UpdateMotor(movePos.position - transform.position);
        }
        

        // check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }

            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            // The array is not cleaned up, so we do it ourself;
            hits[i] = null;
        }
        //UpdateMotor(Vector3.zero);
    }

    Vector2 GetRandomPos()
    {
        Vector2 res = new Vector2(Random.Range(leftDownPos.position.x, rightUpPos.position.x), Random.Range(leftDownPos.position.y, rightUpPos.position.y));
        return res;
    }

    //private void FixedUpdate()
    //{
    //    // Is the player in range?
    //    if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
    //    {
    //        //chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;
    //        //when distance < triggerlength, stop chasing(wrong logic)
    //        if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
    //        {       // the correct logic is that the enemy will stop chasing only if the player is out of the chaseLength
    //            chasing = true;
    //        }

    //        if (chasing)
    //        {
    //            if (!collidingWithPlayer)
    //            {
    //                UpdateMotor((playerTransform.position - transform.position).normalized);
    //            }
    //        }
    //        else
    //        {
    //            UpdateMotor(startingPosition - transform.position);
    //        }
    //    }
    //    else
    //    {
    //        UpdateMotor(startingPosition - transform.position);
    //        chasing = false;
    //    }

    //    // check for overlaps
    //    collidingWithPlayer = false;
    //    boxCollider.OverlapCollider(filter, hits);
    //    for (int i = 0; i < hits.Length; i++)
    //    {
    //        if (hits[i] == null)
    //        {
    //            continue;
    //        }

    //        if (hits[i].tag == "Fighter" && hits[i].name == "Player")
    //        {
    //            collidingWithPlayer = true;
    //        }

    //        // The array is not cleaned up, so we do it ourself;
    //        hits[i] = null;
    //    }
    //    //UpdateMotor(Vector3.zero);
    //}
    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}
