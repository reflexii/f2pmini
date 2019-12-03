using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MapProperties : MonoBehaviour
{

    public List<GameObject> wallList;
    public GameObject deadZonePrefab;
    public float deadZonePadding = 2f;
    public float goalAmount = 80f;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI currentText;
    public float distanceTravelled = 0f;

    private GameManager gm;


    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        AddWallObjectsToList();
        CreateDeadzonesAroundLevel();

        goalText.text = "/ " + goalAmount.ToString();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddWallObjectsToList()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.layer == LayerMask.NameToLayer("WhiteWalls"))
            {
                wallList.Add(go);
            }
        }
    }

    public void ResetLevel()
    {
        foreach (GameObject g in wallList)
        {
            g.SetActive(true);
        }
    }

    public void CreateDeadzonesAroundLevel()
    {
        Camera camera = Camera.main;

        float height = camera.orthographicSize * 2f;
        float width = camera.aspect * height;

        GameObject bottom = Instantiate(deadZonePrefab, new Vector3(0, (-height/2f) - 0.5f - deadZonePadding, 0), Quaternion.identity);
        bottom.transform.localScale = new Vector3(width + 2f + deadZonePadding*2f, 1f, 1f);
        GameObject top = Instantiate(deadZonePrefab, new Vector3(0f, height/2f + 0.5f + deadZonePadding, 0), Quaternion.identity);
        top.transform.localScale = new Vector3(width + 2f + deadZonePadding*2f, 1f, 1f);
        GameObject left = Instantiate(deadZonePrefab, new Vector3((-width/2f) - 0.5f - deadZonePadding, 0f, 0), Quaternion.identity);
        left.transform.localScale = new Vector3(1f, height + deadZonePadding*2f, 1f);
        GameObject right = Instantiate(deadZonePrefab, new Vector3((width/2f) + 0.5f + deadZonePadding, 0f, 0), Quaternion.identity);
        right.transform.localScale = new Vector3(1f, height + deadZonePadding*2f, 1f);
    }

    public void UpdateUI()
    {
        if (gm.playerMovement.launched)
        {
            currentText.text = Mathf.Floor(distanceTravelled).ToString();
        } else
        {
            currentText.text = "0";
        }
    }
}
