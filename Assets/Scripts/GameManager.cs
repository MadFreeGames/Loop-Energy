using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform powerSourceParent;
    public Transform powerConductParent;
    public List<PowerConnector> powerConnectors=new();
    public List<PowerSource> powerSources = new();
    public Rotator[] rotators;
    public Background background;
    public int powerConnectedCount = 0;
    public bool allPowered;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        foreach (Transform PowerConnector in powerConductParent) 
        {
            powerConnectors.Add(PowerConnector.GetChild(0).GetChild(0).GetComponent<PowerConnector>());
        }
        foreach (Transform PowerSource in powerSourceParent) 
        {
            powerSources.Add(PowerSource.GetChild(0).GetChild(0).GetComponent<PowerSource>());
        }

        rotators = FindObjectsOfType<Rotator>();
        background=FindAnyObjectByType<Background>();
        UIManager.instance.BgColor();

        Vibration.Init();
    }

    public void CheckAllPowered()
    {
        powerConnectedCount = 0;
        for (int i = 0; i < powerConnectors.Count; i++)
        {
            if (powerConnectors[i].isPowered)
            {
                powerConnectedCount++;
                if(powerConnectedCount== powerConnectors.Count)
                {
                    StopRotators();
                    AllPowered();
                    allPowered = true;
                    UIManager.instance.Levelcompleted();
                }
            }
            else
            {
                powerConnectedCount = 0;
                allPowered = false;
            }
        }
    }
    public void StopRotators()
    {
        for (int j = 0; j < rotators.Length; j++)
        {
            rotators[j].stopRotation = true;
        }
    }

    public void AllPowered()
    {
        for (int i = 0; i < powerConnectors.Count; i++)
        {
            powerConnectors[i].ActiveCircleffect();
        }
        for (int i = 0; i < powerSources.Count; i++)
        {
            powerSources[i].Circleffect();
        }
    }

    public void TapPopVibrate()
    {
        int vibrate = PlayerPrefs.GetInt("vibrate");
        if (vibrate == 0)
        {
            Vibration.VibratePop();
        }
    }
}
