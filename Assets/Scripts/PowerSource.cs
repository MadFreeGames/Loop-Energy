using UnityEngine;

public class PowerSource : MonoBehaviour
{
    private GameObject glow;
    private GameObject tutorial;
    private GameObject circle;
   
    public void Start()
    {
        glow = transform.GetChild(0).gameObject;
        tutorial = transform.parent.GetChild(2).gameObject;
        circle = transform.parent.GetChild(3).gameObject;
        glow.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerConnector connector = other.GetComponent<PowerConnector>();
        if (connector != null)
        {
            connector.ConnectPowerSource();
            connector.PropagatePower(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PowerConnector connector = other.GetComponent<PowerConnector>();
        if (connector != null)
        {
            connector.DisconnectPowerSource();
            connector.PropagatePower(false);
        }
    }

    public void Circleffect()
    {
        tutorial.SetActive(false);
        circle.SetActive(true);
    }
}
