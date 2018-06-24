using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamController : MonoBehaviour {

    ParticleSystem.EmissionModule kettleSteam;

    // Use this for initialization
    void Start () {
        kettleSteam = GameObject.FindGameObjectWithTag("Kettle").GetComponentInChildren<ParticleSystem>().emission;
        kettleSteam.enabled = false;
    }
	
	public void TurnSteamOn()
    {
        kettleSteam.enabled = true;
    }

   public  void TurnSteamOff()
    {
        kettleSteam.enabled = false;
    }
}
