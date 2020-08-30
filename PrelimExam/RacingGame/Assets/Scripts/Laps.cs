using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laps : MonoBehaviour
{
    public int numCheckpoint; 
    public int currentCheckpoint; 
    public int numLaps;
    public int currentLap; 
    public GameObject GameOverUI;
    public static bool disabled = false;


    private void Start()
    {
        numCheckpoint = GameObject.Find("Checkpoints").transform.childCount;
        currentCheckpoint = 1; 
        numLaps = 1; 
        currentLap = 1; 
    
    }

    private void Update()
    {
        if (currentCheckpoint > numCheckpoint){
            currentLap++;
            currentCheckpoint = 1;
        } 

        if(currentLap > numLaps)
        {
            GameOverUI.SetActive(true);
        } else {
            GameOverUI.SetActive(false);
        }
    }

    private void OnTriggerEnter (Collider checkCollider)
    {
        if (checkCollider.name == currentCheckpoint.ToString())
        {
            currentCheckpoint++;
        }
    }
    
}
