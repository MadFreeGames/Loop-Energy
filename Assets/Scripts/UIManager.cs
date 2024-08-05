using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject levelCompletePanel;
    public GameObject winTransition;
    public GameObject help;
    public GameObject topPanel;
    public TextMeshProUGUI levelnumberText;

    [Header("UI Buttons")]
    public Transform menuButton;
    public Transform SoundButton;
    public Transform BrushButton;
    public Transform HomeButton;
    public Transform RestartButton;
    public Transform VibrateButton;

    public List<Transform> allButtons = new();
    private List<Vector2> originalPositions = new();
    public List<BgColors> bgColors = new();

    [Header("Sprites")]
    public List<Sprite> audioSprites=new List<Sprite>();
    public Sprite vibrateOn;
    public Sprite vibrateOff;
    public int levelNumber;
    private int dummyLevelShow;
    private void Awake()
    {
        if (!instance)
            instance = this;
    }
    void Start()
    {
        levelNumber = PlayerPrefs.GetInt("levelnumber", 2);
        dummyLevelShow= PlayerPrefs.GetInt("dummyLevelShow", 1);
        //if (dummyLevelShow > 11)
        //{
        //    PlayerPrefs.SetInt("levelnumber", 100);
        //}
        levelnumberText.text = "#" + dummyLevelShow.ToString();
        foreach (var button in allButtons)
        {
            originalPositions.Add(button.GetComponent<RectTransform>().anchoredPosition);
        }
        AudioUpdate(PlayerPrefs.GetInt("saveAudio"));
        vibrationUpdate(PlayerPrefs.GetInt("vibrate"));
    }

    private void LateUpdate()
    {
        if(help!=null)
        {
            if (Input.GetMouseButton(0) && help.activeInHierarchy)
            {
                help.SetActive(false);
            }
        }
    }

    public void OnNextButtonPress()
    {

        Debug.Log("levelNumber " + levelNumber + "== >= == " + (SceneManager.sceneCountInBuildSettings-1));

        if (levelNumber >= SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(Random.Range(2, SceneManager.sceneCountInBuildSettings));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if ((dummyLevelShow + 1) < 52)
        {
            if (levelNumber <= dummyLevelShow + 1)
            {
                PlayerPrefs.SetInt("levelnumber", levelNumber + 1);
            }
            PlayerPrefs.SetInt("dummyLevelShow", dummyLevelShow + 1);
        }
        
    }

    public void OnHomeButtonPress()
    {
        SceneManager.LoadScene(1);
    }
    public void OnRetryButtonPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Levelcompleted()
    {
        StartCoroutine(WinDelay());
    }

    public IEnumerator WinDelay()
    {
        menuButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);
        topPanel.SetActive(false);   
        winTransition.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        levelCompletePanel.SetActive(true);
    }

    bool open;
    bool isMenuOpen;
    public void OnMenuButtonPress()
    {
        open = !open;
        if (!isMenuOpen)
        {
            StartCoroutine(MenuOpenAnim(open));
            menuButton.GetChild(0).GetComponent<Animator>().SetBool("menuButton", open);
            menuButton.parent.GetComponent<Button>().interactable = open;
            levelnumberText.transform.parent.GetComponent<Animator>().SetBool("open", open);
            SoundManager.instance.PlaySFX("sfx_energy_settings_gameplay_menu2_button");
            isMenuOpen=true;

        }
    }

    public IEnumerator MenuOpenAnim(bool isOpen)
    {
        if (isOpen)
        {
            for (int i = 0; i < allButtons.Count; i++)
            {
                allButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-88);   
                allButtons[i].gameObject.SetActive(true);
                allButtons[i].GetComponent<RectTransform>().DOAnchorPos(originalPositions[i], .5f).SetEase(Ease.OutElastic, 1f, .5f);
                yield return new WaitForSeconds(0.03f);
            }
        }
        else
        {
            for (int i =allButtons.Count-1; i>=0 ; i--)
            {
                int index = i;
                allButtons[index].GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -88), .5f).SetEase(Ease.InElastic, 1f, .5f).OnComplete(() => 
                {
                    allButtons[index].gameObject.SetActive(false);
                    //Debug.Log("deative");
                });
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.15f);
        isMenuOpen = false;

    }

    public void ButtonClickEffect()
    {
        Transform transform =EventSystem.current.currentSelectedGameObject.transform;
        float punchAmount = -0.2f;
        transform.DOPunchScale(new Vector3(punchAmount, punchAmount, punchAmount), 0.1f).SetEase(Ease.OutBounce);
        SoundManager.instance.PlaySFX("button_click");
    }
    public void OnBrushButtonPress()
    {
        int randomColor = Random.Range(0, bgColors.Count);
        PlayerPrefs.SetInt("savebgColor", randomColor);
        BgColor();
    }
    public void OnSoundButtonPress()
    {
        int saveAudio = PlayerPrefs.GetInt("saveAudio");
        saveAudio++;
        if (saveAudio > 3)
        {
            saveAudio = 0;
        }
        AudioUpdate(saveAudio);
    }
    public void OnVibrateButtonPress()
    {
        int vibrate = PlayerPrefs.GetInt("vibrate");
        vibrate++;
        if (vibrate > 1)
        {
            vibrate = 0;
        }
        vibrationUpdate(vibrate);
    }
    public void vibrationUpdate(int vibrate)
    {
      
        if (vibrate == 1)
        {
            VibrateButton.GetComponent<Image>().sprite = vibrateOff;
            PlayerPrefs.SetInt("vibrate", 1);
            vibrate = 0;
        }
        else
        {
            VibrateButton.GetComponent<Image>().sprite = vibrateOn;
            PlayerPrefs.SetInt("vibrate", 0);
        }

    }
    public void AudioUpdate(int saveID)
    {
        Image audioImage = SoundButton.GetComponent<Image>();
        audioImage.sprite= audioSprites[saveID];
        PlayerPrefs.SetInt("saveAudio", saveID);
        SoundManager.instance.SetVolume(saveID);
    }
    public void BgColor()
    {
        int savebgColor = PlayerPrefs.GetInt("savebgColor");
        GameManager.Instance.background.BackgroundColorUpdate(bgColors[savebgColor].color1, bgColors[savebgColor].color2);
    }
    
}

[System.Serializable]
public class BgColors
{
    public Color color1;
    public Color color2;
}
