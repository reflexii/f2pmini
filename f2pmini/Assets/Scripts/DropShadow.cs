using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour
{
    public float offsetAmount = 0.12f;
    public GameObject graphicsObject;
    public Color dropShadowColor = new Color (0.4f, 0.4f, 0.4f, 0.5f);

    private GameObject shadow;
    private GameObject lightObject;
    


    void Awake()
    {

        lightObject = GameObject.Find("GlobalLight");

        //Create dropshadow
        shadow = Instantiate<GameObject>(graphicsObject, gameObject.transform.position + (CalculateLightDirection() * offsetAmount), Quaternion.identity, gameObject.transform);
        shadow.name = gameObject.name + " Dropshadow";
        SpriteRenderer sr = shadow.GetComponent<SpriteRenderer>();
        sr.color = dropShadowColor;
        sr.sortingOrder = graphicsObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
        shadow.transform.rotation = graphicsObject.transform.rotation;
    }
    void Update()
    {
        shadow.transform.position = gameObject.transform.position + (CalculateLightDirection() * offsetAmount);
    }

    public Vector3 CalculateLightDirection()
    {
        Vector3 direction = (gameObject.transform.position - lightObject.transform.position).normalized;

        return direction;
    }
}
