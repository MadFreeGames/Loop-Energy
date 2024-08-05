using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class LevelSelection : MonoBehaviour
{
    public Transform levelButtonParent;
    public List<Transform> levelButtons = new();
    public int levelNumber;
    public int unLockedAll;
    public bool unLocked;

    private void Start()
    {
        levelNumber = PlayerPrefs.GetInt("levelnumber", 2);
        unLockedAll = PlayerPrefs.GetInt("unlocked");

        Debug.Log(levelNumber);
        foreach (Transform button in levelButtonParent)
        {
            levelButtons.Add(button);
        }
        SetLevelNumbers();
        ActiveLevel(PlayerPrefs.GetInt("dummyLevelShow",1)-1);
        CheckUnlock();
    }

    public void SetLevelNumbers()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            TextMeshProUGUI levelText = levelButtons[i].GetChild(1).GetComponent<TextMeshProUGUI>();
            levelText.text = "#" + (i + 1);
           //levelButtons[i].GetChild(0).GetComponent<Image>().color = GetRandomColor();
        }
    }

    public void OnUnlockAllButtonPress()
    {
       PlayerPrefs.SetInt("unlocked",1);
        CheckUnlock();
    }
    public void OnLevelSelectButtonPress()
    {
        int selectedLevel=EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();
        ActiveLevel(selectedLevel);
        LoadLevel(selectedLevel + 2);
        PlayerPrefs.SetInt("dummyLevelShow", selectedLevel+1);
        int dummyLevelShow = (PlayerPrefs.GetInt("dummyLevelShow", 1) + 1);
        if ((dummyLevelShow + 1) < 52)
        {
            if (levelNumber <= dummyLevelShow + 1)
            {
                PlayerPrefs.SetInt("levelnumber", levelNumber + 1);
            }
        }

    }
    public void CheckUnlock()
    {
        unLockedAll = PlayerPrefs.GetInt("unlocked");
        if (unLockedAll == 1)
        {
            UnLocking(levelButtons.Count);
        }
        else
        {
            UnLocking(levelNumber-1);
        }
    }
    public void UnLocking(int count)
    {
        for (int i = 0; i < count; i++)
        {
            levelButtons[i].GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ActiveLevel(int level)
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].GetChild(3).gameObject.SetActive(false);
        }
       // Debug.Log(level);
        levelButtons[level].GetChild(3).gameObject.SetActive(true);

    }

    public void LoadLevel(int selectedLevel)
    {
        levelNumber = selectedLevel;
        Debug.Log(levelNumber + "===" + selectedLevel);

       


        if (levelNumber > SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(Random.Range(2, SceneManager.sceneCountInBuildSettings));
        }
        else
        {
            SceneManager.LoadScene(levelNumber);
        }
    }
    public void OnBackButtonPress()
    {
        LoadLevel(PlayerPrefs.GetInt("dummyLevelShow", 1)+1);
    }

    public void ButtonClickEffect()
    {
        Transform transform = EventSystem.current.currentSelectedGameObject.transform.parent;
        float punchAmount = -0.2f;
        transform.DOPunchScale(new Vector3(punchAmount, punchAmount, punchAmount), 0.1f).SetEase(Ease.OutBounce);
        if(SoundManager.instance)
            SoundManager.instance.PlaySFX("button_click");
    }
}
