using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wifi : MonoBehaviour
{
    private PowerConnector powerConnector;
    public GameObject sourceConnecter;
    public GameObject wifiConnecter;

    void Start()
    {
        powerConnector=GetComponent<PowerConnector>(); 
    }

    void LateUpdate()
    {
        if (powerConnector.isPowered)
        {
            wifiConnecter.SetActive(false);
            sourceConnecter.SetActive(true);
        }
        else
        {
            wifiConnecter.SetActive(true);
            sourceConnecter.SetActive(false);
        }
        
    }
}
