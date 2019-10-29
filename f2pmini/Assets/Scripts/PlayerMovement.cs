using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float power = 1f;
    public bool launched = false;

    private Rigidbody2D rb;
    private LineRenderer line;
    private bool touching = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 endingTouchPosition;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        rb.gravityScale = 0f;
    }
    void Update()
    {
        
        if (launched)
        {
            rb.gravityScale = 1f;
        }

        TouchMovement();
        
    }

    void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //Get touch starting position once
            if (!touching)
            {
                startTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                currentTouchPosition = startTouchPosition;

                line.SetPosition(0, transform.position);

                touching = true;
            }

            //Spawn linerenderer to show trajectory
            if (touch.phase == TouchPhase.Moved)
            {
                line.enabled = true;

                currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchDirection = currentTouchPosition - startTouchPosition;

                line.SetPosition(1, transform.position + new Vector3(touchDirection.x, touchDirection.y, 0f));
            }

            if (touch.phase == TouchPhase.Ended && !launched)
            {
                endingTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 direction = (endingTouchPosition - startTouchPosition).normalized;

                rb.AddForce(power * direction, ForceMode2D.Impulse);


                touching = false;
                launched = true;
                line.enabled = false;
            }

        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(currentTouchPosition, new Vector3(1f, 1f, 0f));
    }

    //For testing, not used in actual game
    void MouseMovement()
    {

    }
}
