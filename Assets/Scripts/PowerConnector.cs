using System.Collections.Generic;
using UnityEngine;

public class PowerConnector : MonoBehaviour
{
    private GameObject glow;
    private GameObject tutorial;
    private GameObject circle;
    private SpriteRenderer bulbSpriteRenderer;
    private HashSet<PowerConnector> connectedConnectors = new HashSet<PowerConnector>();
    private int powerSourceCount = 0; 
    public bool isPowered = false; 
    private Color bulbColor;
    public bool isPowerReceiver;
    private bool isOnce;
    private void Start()
    {
        glow = transform.GetChild(0).gameObject;
        bulbSpriteRenderer = transform.parent.GetChild(1).GetComponent<SpriteRenderer>();
        tutorial = transform.parent.GetChild(2).gameObject;
        circle = transform.parent.GetChild(3).gameObject;
        bulbColor=Color.white;
        gameObject.layer = 2;
    }

    private void Update()
    {
        if (isPowered)
        {
            bulbColor.a = 1;
            if(isOnce && isPowerReceiver)
            {
                SoundManager.instance.PlaySFX("lamp_blink1_1");
                isOnce = false;
            }
        }
        else
        {
            bulbColor.a = 0.2f;
            isOnce = true;
        }
        glow.SetActive(isPowered);
        bulbSpriteRenderer.color = bulbColor;
    }

    public void ConnectPowerSource()
    {
        powerSourceCount++;
        if (powerSourceCount > 0 && !isPowered)
        {
            PropagatePower(true);
        }
    }

    public void DisconnectPowerSource()
    {
        powerSourceCount--;
        if (powerSourceCount <= 0 && isPowered)
        {
            RecheckPowerState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerConnector otherConnector = other.GetComponent<PowerConnector>();
        if (otherConnector != null && connectedConnectors.Add(otherConnector))
        {
            if (otherConnector.isPowered)
            {
                PropagatePower(true);
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PowerConnector otherConnector = other.GetComponent<PowerConnector>();
        if (otherConnector != null && connectedConnectors.Remove(otherConnector))
        {
            RecheckPowerState();
        }
    }

    public void PropagatePower(bool powerState)
    {
        if (isPowered == powerState) return;

        isPowered = powerState;
        Queue<PowerConnector> queue = new Queue<PowerConnector>();
        HashSet<PowerConnector> visited = new HashSet<PowerConnector>();

        queue.Enqueue(this);
        visited.Add(this);

        while (queue.Count > 0)
        {
            PowerConnector current = queue.Dequeue();
            current.isPowered = powerState;

            foreach (PowerConnector connector in current.connectedConnectors)
            {
                if (connector != null && !visited.Contains(connector))
                {
                    queue.Enqueue(connector);
                    visited.Add(connector);
                }
            }
        }
    }

    private void RecheckPowerState()
    {
        HashSet<PowerConnector> visited = new HashSet<PowerConnector>();
        Queue<PowerConnector> queue = new Queue<PowerConnector>();

        queue.Enqueue(this);
        visited.Add(this);

        while (queue.Count > 0)
        {
            PowerConnector current = queue.Dequeue();
            current.isPowered = false;

            foreach (PowerConnector connector in current.connectedConnectors)
            {
                if (connector != null && !visited.Contains(connector))
                {
                    queue.Enqueue(connector);
                    visited.Add(connector);
                }
            }
        }
        foreach (PowerConnector connector in visited)
        {
            if (connector.powerSourceCount > 0)
            {
                connector.PropagatePower(true);
            }
        }
    }

    public void ActiveCircleffect()
    {
        tutorial.SetActive(false);
        circle.SetActive(true);
    }
}
