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
    private Vector2 ballDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }
    void Update()
    {
        
        if (!launched)
        {
            TouchMovement();
        }
        
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
                ballDirection = (endingTouchPosition - startTouchPosition).normalized;

                rb.AddForce(ballDirection * power, ForceMode2D.Impulse);

                touching = false;
                launched = true;
                line.enabled = false;
            }

        }
    }

    void UpdateTraveledAmount()
    {

    }

}
