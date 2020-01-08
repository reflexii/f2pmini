using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour
{

    public bool testModeOn = false;
    public List<GameObject> spawnPointList = new List<GameObject>();
    public GameObject spawnPointPrefab;
    public GameObject ballPrefab;
    public GameObject ballPool;
    public List<GameObject> ballList = new List<GameObject>();

    public float testingAngleIncrease = 5f;
    public float testBallDelay = 0.1f;

    private float smallestRotationSpeed = 0f;
    private bool movingObjectsInScene = false;
    private int amountOfBallsFromOneSpawn = 0;
    private float currentSpawnAngle = 0;
    private bool startSpawning = false;
    private int currentIndexInSpawnList = 0;
    private PlayerMovement pm;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartTesting();
        }

        if (startSpawning && currentIndexInSpawnList <= spawnPointList.Count-1) {

        }
    }

    public void StartTesting() {
        CreateSpawnPoints();
        CheckMovingObjects();
        CheckAmountOfBallsFromOneSpawn();
        LaunchBalls();
    }

    public void CreateSpawnPoints() {

        Camera camera = Camera.main;

        float height = camera.orthographicSize * 2f;
        float width = camera.aspect * height;

        float xPadding = 1f;
        float yPadding = 1f;

        float startPointx = -width / 2f;
        float startPointy = -height / 2f;

        float howManyX = width / xPadding;
        float howManyY = (height / yPadding) + 1;

        for (int x = 0; x < howManyX; x++) {
            for (int y = 0; y < howManyY; y++) {

                if (!Physics2D.OverlapCircle(new Vector3(startPointx + (x * xPadding), startPointy + (y * yPadding), 0f), 1f, 1 << LayerMask.NameToLayer("WhiteWalls")) &&
                    !Physics2D.OverlapCircle(new Vector3(startPointx + (x * xPadding), startPointy + (y * yPadding), 0f), 1f, 1 << LayerMask.NameToLayer("BlackWalls"))) {
                    GameObject g = Instantiate(spawnPointPrefab, new Vector3(startPointx + (x * xPadding), startPointy + (y * yPadding), 0f), Quaternion.identity);
                    g.transform.parent = gameObject.transform;
                    spawnPointList.Add(g);
                }
                
            }
            
        }
    }

    public void CheckMovingObjects() {
        Rotator[] movingObjects = FindObjectsOfType<Rotator>();

        List<Rotator> rotList = new List<Rotator>();

        foreach (Rotator rot in movingObjects) {
            if (rot.gameObject.layer == LayerMask.NameToLayer("WhiteWalls") || rot.gameObject.layer == LayerMask.NameToLayer("BlackWalls")) {
                if (rot.gameObject.activeInHierarchy) {
                    rotList.Add(rot);
                }
                
            }
        }

        //Check rotation speeds to determine amount of balls spawned in testing
        if (rotList.Count != 0) {
            movingObjectsInScene = true;
            foreach (Rotator rot in rotList) {
                if (smallestRotationSpeed == 0f) {
                    smallestRotationSpeed = rot.rotationSpeed;
                }

                if (rot.rotationSpeed < smallestRotationSpeed) {
                    smallestRotationSpeed = rot.rotationSpeed;
                }
            }

        }
    }

    public void CheckAmountOfBallsFromOneSpawn() {
        if (movingObjectsInScene) {
            float amount = (360f / smallestRotationSpeed) / testBallDelay;
            amountOfBallsFromOneSpawn = (int)amount;
        }
    }

    public void LaunchBalls() {
        if (!movingObjectsInScene) {
            StartCoroutine("BallLaunchCoroutine");
        }
    }

    public IEnumerator BallLaunchCoroutine() {

        while (currentIndexInSpawnList < spawnPointList.Count-1) {

            if (pm == null) {
                pm = GameObject.Find("PlayerMovement").GetComponent<PlayerMovement>();
            }

            //New spawnpoint!
            if (currentSpawnAngle >= 360) {
                currentSpawnAngle = 0;
                currentIndexInSpawnList++;
            }


            GameObject g = Instantiate(ballPrefab, spawnPointList[currentIndexInSpawnList].transform.position, Quaternion.identity, ballPool.transform);
            Ball ball = g.GetComponent<Ball>();
            ball.testBall = true;
            ball.ballDirection = Quaternion.Euler(0f, 0f, currentSpawnAngle) * Vector2.right;
            ball.launchedBallDirection = ball.ballDirection;
            ball.GetComponent<Rigidbody2D>().AddForce(ball.ballDirection * pm.power, ForceMode2D.Impulse);

            currentSpawnAngle += testingAngleIncrease;

            yield return new WaitForSeconds(testBallDelay);
        }
        
    }

    
}
