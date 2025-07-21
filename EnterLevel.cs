using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    public LevelManager levelManager;
    public bool isEnter;

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (isEnter)
            {
                levelManager.playerEnteredLevel = true;
            }
            else
            {
                levelManager.playerExitedLevel = true;
            }
        }

       
    }

    public void OnPlayerTeleportThrough(GameObject player)
    {
        if (player.CompareTag("Player"))
        {
            if (isEnter)
            {
                levelManager.playerEnteredLevel = true;
            }
            else
            {
                levelManager.playerExitedLevel = true;
            }

        }
    }



}
