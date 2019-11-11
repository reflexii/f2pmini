using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapProperties currentMapProperties;
    public PlayerMovement playerMovement;
   
    public void ResetLevel()
    {
        currentMapProperties.ResetLevel();
        playerMovement.spawnEnabled = true;
        playerMovement.launched = false;
    }
}
