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
    public int ballPoolSize = 0;
    
    public float testBallDelay = 0.1f;

    private float smallestRotationSpeed = 0f;
    private bool movingObjectsInScene = false;
    private int amountOfBallsFromOneSpawn = 0;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartTesting();
        }
    }

    public void StartTesting() {
        CreateSpawnPoints();
        CheckMovingObjects();
        CreateBallPool();
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
                GameObject g = Instantiate(spawnPointPrefab, new Vector3(startPointx + (x * xPadding), startPointy + (y * yPadding), 0f), Quaternion.identity);
                g.transform.parent = gameObject.transform;
                spawnPointList.Add(g);
            }
            
        }
    }

    public void CheckMovingObjects() {
        Rotator[] movingObjects = FindObjectsOfType<Rotator>();

        List<Rotator> rotList = new List<Rotator>();

        foreach (Rotator rot in movingObjects) {
            if (rot.gameObject.layer == LayerMask.NameToLayer("WhiteWalls") || rot.gameObject.layer == LayerMask.NameToLayer("BlackWalls")) {
                rotList.Add(rot);
            }
        }

        //Check rotation speeds to determine amount of balls spawned in testing
        if (rotList[0] != null) {
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

    public void CreateBallPool() {
        for (int i = 0; i < ballPoolSize; i++) {
            GameObject g = Instantiate(ballPrefab, transform.position, Quaternion.identity, ballPool.transform);
            g.GetComponent<Ball>().testBall = true;
            g.SetActive(false);
            ballList.Add(g);
        }
    }
    public void LaunchBalls() {
        //Add Coroutine?
    }

    public GameObject GetOrAddBallFromPool() {
        for (int i = 0; i < ballList.Count; i++) {
            if (!ballList[i].activeInHierarchy) {
                return ballList[i];
            }
        }

        GameObject g = Instantiate(ballPrefab, transform.position, Quaternion.identity, ballPool.transform);
        g.GetComponent<Ball>().testBall = true;
        g.SetActive(false);
        ballList.Add(g);

        return g;
    }

    public void CheckAmountOfBallsFromOneSpawn() {
        if (movingObjectsInScene) {
            float amount = (360f / smallestRotationSpeed) / testBallDelay;
            amountOfBallsFromOneSpawn = (int) amount;
        }
    }
}
