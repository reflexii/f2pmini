using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public float distanceTravelled = 0f;
    public Vector2 ballDirection;
    public Vector2 launchedBallDirection;
    public bool testBall = false;
    public GameObject raycastLeft;
    public GameObject raycastRight;

    private GameObject targetedObject;
    private Vector2 lastPosition;
    private GameManager gm;

    private List<int> CollidedObjects = new List<int>();


    private void Awake() {
        lastPosition = transform.position;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, ballDirection);

        UpdateDistance();

        if (testBall) {
            BallTestRaycast();
        }

    }

    void UpdateDistance() {
        if (!testBall) {
            gm.currentMapProperties.distanceTravelled = distanceTravelled;
        }
    }

    void BallTestRaycast() {
        Debug.DrawRay(raycastLeft.transform.position, ballDirection * 0.65f);
        Debug.DrawRay(raycastRight.transform.position, ballDirection * 0.65f);
        Debug.DrawRay(transform.position, -ballDirection * 0.65f);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastLeft.transform.position, ballDirection, 0.65f, 1 << LayerMask.NameToLayer("WhiteWalls"));
        RaycastHit2D hitRight = Physics2D.Raycast(raycastRight.transform.position, ballDirection, 0.65f, 1 << LayerMask.NameToLayer("WhiteWalls"));
        RaycastHit2D back = Physics2D.Raycast(transform.position, -ballDirection, 0.65f, 1 << LayerMask.NameToLayer("WhiteWalls"));


        if (hitLeft && hitRight) {
            float distanceLeft = Vector2.Distance(raycastLeft.transform.position, hitLeft.transform.position);
            float distanceRight = Vector2.Distance(raycastRight.transform.position, hitRight.transform.position);

            if (distanceLeft <= distanceRight) {
                targetedObject = hitLeft.transform.gameObject;
            } else {
                targetedObject = hitRight.transform.gameObject;
            }
        } else if (hitLeft && !hitRight) {
            targetedObject = hitLeft.transform.gameObject;
        } else if (!hitLeft && hitRight) {
            targetedObject = hitRight.transform.gameObject;
        }
        
        if (targetedObject != null) {
            
            if (CollidedObjects.Contains(targetedObject.transform.GetInstanceID())) {
                gameObject.layer = LayerMask.NameToLayer("NoWallCollision");
                targetedObject = null;
            } else {
                gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }

        if (!back && !hitLeft && !hitRight && gameObject.layer == LayerMask.NameToLayer("NoWallCollision")) {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (testBall) {
            if (collision.gameObject.GetComponent<BlockDestroyer>() != null) {

                if (!CollidedObjects.Contains(collision.gameObject.transform.GetInstanceID())) {
                    CollidedObjects.Add(collision.gameObject.transform.GetInstanceID());
                } else {
                    //gameObject.layer = LayerMask.NameToLayer("NoWallCollision");
                }

            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("WhiteWalls") || collision.gameObject.layer == LayerMask.NameToLayer("BlackWalls")) {
                ballDirection = Vector2.Reflect(ballDirection, collision.contacts[0].normal);
            }
            
        }
    }
}
