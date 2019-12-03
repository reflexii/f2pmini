using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO: animation
            if (collision.gameObject.GetComponent<Ball>() != null) {
                if (!collision.gameObject.GetComponent<Ball>().testBall) {
                    gameObject.SetActive(false);
                }
            }
            
        }
    }
}
