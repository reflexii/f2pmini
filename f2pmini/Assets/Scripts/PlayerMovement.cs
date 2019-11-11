using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject ballPrefab;
    public float power = 1f;
    public bool launched = false;
    public bool spawnEnabled = true;
    public float distanceTravelled = 0f;

    private Rigidbody2D rb;
    private LineRenderer line;
    private bool touching = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 endingTouchPosition;
    private Vector2 ballDirection;
    private GameObject playerBall;
    private Vector2 lastPosition;

    void Update()
    {
        
        if (!launched)
        {
            TouchMovement();
            distanceTravelled = 0f;
        } else
        {
            CalculateDistanceTravelled();
        }
        
    }

    void CalculateDistanceTravelled()
    {
        if (playerBall != null)
        {
            distanceTravelled += Vector2.Distance(playerBall.transform.position, lastPosition);
            lastPosition = playerBall.transform.position;
        }
    }

    void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (!launched && spawnEnabled)
            {
                playerBall = Instantiate(ballPrefab, Camera.main.ScreenToWorldPoint(touch.position), Quaternion.identity);
                playerBall.transform.position = new Vector3(playerBall.transform.position.x, playerBall.transform.position.y, 0f);

                rb = playerBall.GetComponent<Rigidbody2D>();
                line = playerBall.GetComponent<LineRenderer>();
                line.enabled = false;
                spawnEnabled = false;

            }
            

            //Get touch starting position once
            if (!touching)
            {
                startTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                currentTouchPosition = startTouchPosition;

                line.SetPosition(0, playerBall.transform.position);

                touching = true;
            }

            //Spawn linerenderer to show trajectory
            if (touch.phase == TouchPhase.Moved)
            {
                line.enabled = true;

                currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchDirection = currentTouchPosition - startTouchPosition;

                line.SetPosition(1, playerBall.transform.position + new Vector3(touchDirection.x, touchDirection.y, 0f));
            }

            if (touch.phase == TouchPhase.Ended && !launched)
            {
                endingTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                ballDirection = (endingTouchPosition - startTouchPosition).normalized;

                rb.AddForce(ballDirection * power, ForceMode2D.Impulse);

                lastPosition = startTouchPosition;

                touching = false;
                launched = true;
                line.enabled = false;
            }

        }
    }
}
