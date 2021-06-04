using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject UIManagerObject;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject PetalPanel;
    public GameObject TimerPanel;
    public GameObject ContinueButton;
    public GameObject PauseButtonObject;
    public GameObject MenubackGroundA;
    public GameObject MenubackGroundB;
    public GameObject[] PetalGroups;
    public GameObject[] UI_petals;
    public static GameObject[] InGamePetals;
    public static int petalIndex;
    public int retryCounter;
    public GameObject RetryPanel;
    public Text onScreenRetry;
    public static int loadScene;
    public GameObject LetterPanel;
    public GameObject[] Player;
    public Collider playerCollider;
    public GameObject welcome;
    public GameObject EndLetterPanel;

    public GameObject mainMenu;
    public Scene[] scenes;

    public Text MusicVolumePercent;
    public Text SFXVolumePercent;

    public static bool leverTrue;
    public GameObject Lever;
    public GameObject LeverHolder;

    public AudioMixerGroup[] Audio;
    public AudioSource musicSource;
    public AudioSource sfxsource;
    public bool muted;
    public AudioClip[] levelMusic;
    public AudioClip[] soundEffects;
    int soundEffectsIndex;

    [SerializeField] public TextMeshProUGUI letterText;
    [SerializeField] public TextMeshProUGUI letter1Text;

    public string story;
    public string story1;
    public GameObject anyKeyObject;

    public Text timerUI;
    public float timerStart;
    int timer;
    [SerializeField] int nextScene;
    public int timerTotal;
    float timerOffset;

    public float timeMarker;

    public Dropdown resolution;
    public Resolution[] resolutions;

    public static UIManager instance;
    CanvasGroup startgroup;
    CanvasGroup mainMenuGroup;

    private void Awake()
    {
        nextScene = 0;

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        story = letterText.text;
        letterText.text = "";
    }

    private void Start()
    {
        startgroup = MenubackGroundB.GetComponent<CanvasGroup>();
        mainMenuGroup = mainMenu.GetComponent<CanvasGroup>();
        startgroup.alpha = 0;
        timer = 1;
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        PauseButtonObject.SetActive(false);
        LetterPanel.SetActive(false);
        

        if (nextScene <= 2)
        {
            PetalPanel.SetActive(false);
            TimerPanel.SetActive(false);
        }

        #region playing Music
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = levelMusic[0];
        musicSource.Play();

        muted = false;
        #endregion

        #region resolution and screen options
        Screen.fullScreen = true;
        resolutions = Screen.resolutions;
        resolution.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)  && nextScene > 0)
        {
            PauseButton();
        }

        if (Input.anyKey && anyKeyObject.activeSelf == true)
        {
            anyKeyObject.SetActive(false);
            welcome.SetActive(false);
            StartCoroutine(FadeInOut( 1, startgroup));
            StartCoroutine(FadeInOut( 1, mainMenuGroup));
        }

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerUI.text = "Time Remaining: " + niceTime;

        if (nextScene >= 2 && nextScene <= 5)
        {
            timer = (int)(timerStart - Time.time);
        }
        else
        {
            timer = timerTotal;
        }

        if (timer <= 0 && nextScene >= 2)
        {
            RestartLevel();
        }

        if (leverTrue == true)
        {
            Lever.SetActive(true);
        }
        if(leverTrue == false)
        {
            Lever.SetActive(false);
        }

        loadScene = nextScene;
        InGamePetals = UI_petals;
    }
   
    public void PauseButton()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        soundEffectsIndex = 0;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void OptionsButton()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        soundEffectsIndex = 0;
        OptionsMenu.SetActive(false);
    }

    public void ExitButton()
    {
        soundEffectsIndex = 0;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PlayButton()
    {
        //doesnt destroy the UI elements needed in the game
        DontDestroyOnLoad(UIManagerObject);
        LetterPanel.SetActive(true);
        ContinueButton.SetActive(true);
        StartCoroutine(PlayText(0));
        nextScene++;
        mainMenu.SetActive(false);
        SceneManager.LoadScene(nextScene);
    }

    public void Continue()
    {
        TimeSet();
        LetterPanel.SetActive(false);
        nextScene++;
        if (nextScene > 5)
            nextScene = 0;

        //enables the in game UI elements not present in the opening letter
        //disables the UI element not present in the game
        PetalPanel.SetActive(true);
        TimerPanel.SetActive(true);
        RetryPanel.SetActive(true);
        ContinueButton.SetActive(false);

        switch (nextScene)
        {
            case 0:
                MenubackGroundB.SetActive(true);
                mainMenu.SetActive(true);
                musicSource.clip = levelMusic[0];
                break;

            case 1:
                musicSource.clip = levelMusic[0];
                ContinueButton.SetActive(true);
                break;

            case 2:
                MenubackGroundA.SetActive(false);
                MenubackGroundB.SetActive(false);
                musicSource.clip = levelMusic[1];
                timerStart = 181 + timeMarker;
                petalIndex = 0;
                PetalGroups[0].SetActive(true);
                for (int i = 0; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 3:
                musicSource.clip = levelMusic[2];
                timerStart = 241 + timeMarker;
                petalIndex = 3;
                PetalGroups[1].SetActive(true);
                for (int i = 3; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 4:
                musicSource.clip = levelMusic[3];
                timerStart = 301 + timeMarker;
                petalIndex = 6;
                LeverHolder.SetActive(true);
                Lever.SetActive(false);
                PetalGroups[2].SetActive(true);
                for (int i = 6; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 5:
                //plays the opening music on the final level and displays the timer total on the timer
                musicSource.clip = levelMusic[0];
                timerStart = timerTotal;
                EndLetterPanel.SetActive(true);
                StartCoroutine(PlayText(1));
                petalIndex = 9;
                ContinueButton.SetActive(true);
                break;
        }
        
        //when the new scene is loaded plays the new music
        musicSource.Play();

        //run the click sound effect
        soundEffectsIndex = 0;

        //run the total timer method to add the remaining time to the timer total
        TimerTotal();

        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);

        //doesnt destroy the UI elements needed in the game
        if (nextScene > 0)
        DontDestroyOnLoad(UIManagerObject);
    }

    /// <summary>
    /// the script that runs when the level is restarted
    /// </summary>
    public void RestartLevel()
    {
        //without this the menu remains open
        PauseMenu.SetActive(false);

        //when clicked it will play the click sound effect
        soundEffectsIndex = 0;

        //reload the scene
        SceneManager.LoadScene(nextScene);

        //unpauses the time of the game
        Time.timeScale = 1;

        //removes excess time from the timer so that it displays the correct time
        timerOffset = Time.time;

        //if you restart the final and start letter it does not add to your retry attempts
        if(nextScene >= 2 && nextScene <= 4)
        {
            retryCounter++;
        }

        //this only changes when the retry button is used
        onScreenRetry.text = "Retries: " + retryCounter.ToString();

        //this section resets the number of petals in the UI,
        //0 in the first level, 6 in the second. 
        //the petals are not displayed in the end letter
        //sets the timer for each level
        //the final level displays a total time
        foreach (GameObject petal in UI_petals)
        {
            switch (nextScene)
            {
                case 2:
                    petal.SetActive(false);
                    PlayerController.petalsCollected = 0;
                    timerStart = 181 + timerOffset;
                    break;

                case 3:
                    if(petalIndex > 2)
                    {
                        petal.SetActive(false);
                        PlayerController.petalsCollected = 3;
                        for (int i = 0; i < 3; i++)
                        {
                            UI_petals[i].SetActive(true);
                        }
                        timerStart = 241 + timerOffset;
                    }
                    break;

                case 4:
                    if (petalIndex > 5)
                    {
                        petal.SetActive(false);
                        PlayerController.petalsCollected = 6;
                        for (int i = 0; i < 6; i++)
                        {
                            UI_petals[i].SetActive(true);
                        }
                        timerStart = 301 + timerOffset;
                    }
                    break;


                case 5:
                    timerStart = timerTotal;
                    break;
            }
        }
    }

    /// <summary>
    /// plays the sound effect as dictated by the sound effect index
    /// </summary>
    public void PlaySFX()
    {
        sfxsource.clip = soundEffects[soundEffectsIndex];
        sfxsource.Play();
    }

    /// <summary>
    /// the sound effects can be changed independently of the music volume.
    /// Audio[0] is the game music mixed
    /// </summary>
    /// <param name="musicVolume"></param>
    public void MusicVolume(float musicVolume)
    {
        Audio[0].audioMixer.SetFloat("musicVolume", musicVolume);
        MusicVolumePercent.text = (Mathf.Round((musicVolume + 80) * 100f / 100)).ToString() + " %";
    }

    /// <summary>
    /// the sound effects can be changed independently of the music volume.
    /// Audio[1] is the game sound effect mixer
    /// </summary>
    /// <param name="soundEffectsVolume"></param>
    public void SoundEffectsVolume(float soundEffectsVolume)
    {
        Audio[1].audioMixer.SetFloat("soundEffectsVolume", soundEffectsVolume);

        //gives a percentage of the volume by rounding the float to two decimals x100
        SFXVolumePercent.text = (Mathf.Round((soundEffectsVolume + 80) * 100f / 100)).ToString() + " %";
    }

    /// <summary>
    /// reduces the volume by 80 (80 is the minimum value) if selected
    /// covers both the sfx and music
    /// </summary>
    /// <param name="isMuted"></param>
    public void Mute(bool isMuted)
    {
        if (isMuted)
        {
            Audio[0].audioMixer.SetFloat("isMutedVolume", -80);
        }
        else
        {
            Audio[0].audioMixer.SetFloat("isMutedVolume", 0 );
        }
    }

    /// <summary>
    /// displays the final timer score buy adding the timer each level
    /// </summary>
    public void TimerTotal()
    {
        timerTotal = timer + timerTotal;
    }

    /// <summary>
    /// will set the resolution of the screen in width x height pixels
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    /// <summary>
    /// selects if the game plays in windowed or fullscreen
    /// </summary>
    /// <param name="isFullScreen"></param>
    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Lets the player switch between high medium and low settings as set in Unity.
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// when a petal is picked up interacts with the player script and turns on the UIPetals accordingly
    /// </summary>
    /// <param name="petalIndex"></param>
    public static void DisplayPetal(int petalIndex)
    {
        InGamePetals[petalIndex].SetActive(true);
    }

    public IEnumerator PlayText(int textIndex)
    {
        if(textIndex == 0)
        {
            foreach (char c in story)
            {
                letterText.text += c;
                yield return new WaitForSeconds(0.05f);
            }
        }
        if (textIndex == 1)
        {
            foreach (char c in story1)
            {
                letter1Text.text += c;
                yield return new WaitForSeconds(0.05f);
            }
        }

    }

    public IEnumerator FadeInOut(float end, CanvasGroup target)
    {
        while (target.alpha < end)
        {
            target.alpha = target.alpha + 0.01f;
            if (target.alpha >= 1)
            {
                target.alpha = 1;
                yield return 0;
            }

            yield return 0;
        }
        yield return 0;
    }
    
    public void TimeSet()
    {
        timeMarker = Time.time;
    }
}
