using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public bool rotate = true;
    public float rotationSpeed = 5f;
    public enum RotateDirection {Clockwise, CounterClockwise};
    public RotateDirection rotateDirection;


    void Update()
    {
        if (rotate)
        {
            switch(rotateDirection)
            {
                case RotateDirection.Clockwise:
                    transform.Rotate(Vector3.forward * Time.deltaTime * -rotationSpeed);
                    break;
                case RotateDirection.CounterClockwise:
                    transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
                    break;
            }
        }
    }
}
