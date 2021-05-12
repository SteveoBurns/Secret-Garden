using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject UIManagerObject;
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject PetalPanel;
    public GameObject TimerPanel;
    public GameObject ContinueButton;
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

    public Scene[] scenes;

    public Text MusicVolumePercent;
    public Text SFXVolumePercent;

    public AudioMixerGroup[] Audio;
    public AudioSource musicSource;
    public AudioSource sfxsource;
    public bool muted;
    public AudioClip[] levelMusic;
    public AudioClip[] soundEffects;
    int soundEffectsIndex;

    public Text timerUI;
    int minutes;
    int seconds;
    public float timerStart;
    int timer;
    [SerializeField] int nextScene;
    string niceTime;
    public int timerTotal;
    float timerOffset;

    public Dropdown resolution;
    public Resolution[] resolutions;
    string[] sceneList = new string[] { "Start Letter", "Level 1 Test", "Level 2 Test", "Level 3 Test", "End Letter" };

    public static UIManager instance;


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
    }

    private void Start()
    {
        timer = 1;
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = levelMusic[0];
        musicSource.Play();

        muted = false;

        if (nextScene == 0)
        {
            PetalPanel.SetActive(false);
            TimerPanel.SetActive(false);
        }

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

        if (nextScene <= 3)
        {
            timer = (int)(timerStart - Time.time);
        }
        else
        {
            timer = (int)timerStart;
        }

        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerUI.text = "Time Remaining: " + niceTime;

        if (timer <= 0 && nextScene >= 1)
        {
            RestartLevel();
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

    public void Continue()
    {
        LetterPanel.SetActive(false);
        nextScene++;

        switch (nextScene)
        {
            case 1:
                musicSource.clip = levelMusic[1];
                timerStart = 181 + Time.time;
                petalIndex = 0;
                PetalGroups[0].SetActive(true);
                for (int i = 0; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 2:
                musicSource.clip = levelMusic[2];
                timerStart = 241;
                petalIndex = 3;
                PetalGroups[1].SetActive(true);
                for (int i = 3; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 3:
                musicSource.clip = levelMusic[3];
                timerStart = 301;
                petalIndex = 6;
                PetalGroups[2].SetActive(true);
                for (int i = 6; i < UI_petals.Length; i++)
                {
                    UI_petals[i].SetActive(false);
                }
                break;

            case 4:
                //plays the opening music on the final level and displays the timer total on the timer
                musicSource.clip = levelMusic[0];
                timerStart = timerTotal;
                // 9 
                petalIndex = 9;
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
        DontDestroyOnLoad(UIManagerObject);

       //enables the in game UI elements not present in the opening letter
       //disables the UI element not present in the game
        PetalPanel.SetActive(true);
        TimerPanel.SetActive(true);
        RetryPanel.SetActive(true);
        ContinueButton.SetActive(false);
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
        if(nextScene >= 1 && nextScene <= 3)
        {
            retryCounter++;
        }

        //this only changes when the retry button is used
        onScreenRetry.text = "Retries: " + retryCounter.ToString();

        //this section resets the number of petals in the UI,
        //0 in the first level, 6 in the second. 
        //the petals are not displayed in the end letter
        foreach (GameObject petal in UI_petals)
        {
            switch (nextScene)
            {
                case 1:
                    petal.SetActive(false);
                    PlayerController.petalsCollected = 0;
                    break;

                case 2:
                    if(petalIndex > 2)
                    {
                        petal.SetActive(false);
                        PlayerController.petalsCollected = 3;
                        for (int i = 0; i < 3; i++)
                        {
                            UI_petals[i].SetActive(true);
                        }
                    }
                    break;

                case 3:
                    if (petalIndex > 5)
                    {
                        petal.SetActive(false);
                        PlayerController.petalsCollected = 6;
                        for (int i = 0; i < 6; i++)
                        {
                            UI_petals[i].SetActive(true);
                        }
                    }
                    break;
            }
        }

        //sets the timer for each level
        //the final level displays a total time
        switch (nextScene)
        {
            case 1:
                timerStart = 181 + timerOffset;
                break;

            case 2:
                timerStart = 241 + timerOffset;
                break;

            case 3:
                timerStart = 301+ timerOffset;
                break;

            case 4:
                timerStart = timerTotal;
                break;
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
}
