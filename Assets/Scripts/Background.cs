using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Material Material;

    public void Awake()
    {
        Material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    public void BackgroundColorUpdate(Color color1,Color color2)
    {
        Material.SetColor("_TopColor", color1);
        Material.SetColor("_BottomColor", color2);
    }
}
