using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (collision.gameObject.GetComponent<Ball>() != null) {
                if (collision.gameObject.GetComponent<Ball>().testBall) {
                    //Save test information
                } else {
                    Debug.Log("Goal reached!");
                }
            }

            Destroy(collision.gameObject);
        }
    }
}
