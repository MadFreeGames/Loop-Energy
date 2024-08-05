using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class MainMenu : MonoBehaviour
{
   
    void Start()
    {
        Invoke(nameof(LevelToLoad),2);
    }
    public void LevelToLoad()
    {

        if (PlayerPrefs.GetInt("levelnumber", 2) > SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings-1));
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("levelnumber", 2));
        }
    }

}
