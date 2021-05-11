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

    private void Awake()
    {
        nextScene = 0;
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
                timerStart = 300;
                petalIndex = 0;
                PetalGroups[0].SetActive(true);
                break;

            case 2:
                musicSource.clip = levelMusic[2];
                timerStart = 360;
                petalIndex = 3;
                PetalGroups[1].SetActive(true);
                break;

            case 3:
                musicSource.clip = levelMusic[3];
                timerStart = 420;
                petalIndex = 6;
                PetalGroups[2].SetActive(true);
                break;

            case 4:
                musicSource.clip = levelMusic[0];
                timerStart = timerTotal;
                petalIndex = 9;
                break;
        }

        musicSource.Play();

        soundEffectsIndex = 0;
        TimerTotal();

        SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);

        DontDestroyOnLoad(UIManagerObject);


        PetalPanel.SetActive(true);
        TimerPanel.SetActive(true);
        RetryPanel.SetActive(true);

        foreach(GameObject petal in UI_petals)
        {
            petal.SetActive(false);
        }

        ContinueButton.SetActive(false);
    }

    public void RestartLevel()
    {
        PauseMenu.SetActive(false);
        soundEffectsIndex = 0;
        SceneManager.LoadScene(nextScene);

        Time.timeScale = 1;
        timerOffset = Time.time;

        if(nextScene >= 1 && nextScene <= 3)
        {
            retryCounter++;
        }

        onScreenRetry.text = "Retries: " + retryCounter.ToString();

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
                    }
                    break;

                case 3:
                    if (petalIndex > 5)
                    {
                        petal.SetActive(false);
                        PlayerController.petalsCollected = 6;
                    }
                    break;
            }
        }

        switch (nextScene)
        {
            case 1:
                timerStart = 301 + timerOffset;
                break;

            case 2:
                timerStart = 361 + timerOffset;
                break;

            case 3:
                timerStart = 421+ timerOffset;
                break;

            case 4:
                timerStart = timerTotal;
                break;
        }
    }

    public void PlaySFX()
    {
        sfxsource.clip = soundEffects[soundEffectsIndex];
        sfxsource.Play();
    }

    public void MusicVolume(float musicVolume)
    {
        Audio[0].audioMixer.SetFloat("musicVolume", musicVolume);
        MusicVolumePercent.text = (Mathf.Round((musicVolume + 80) * 100f / 100)).ToString() + " %";
    }

    public void SoundEffectsVolume(float soundEffectsVolume)
    {
        Audio[1].audioMixer.SetFloat("soundEffectsVolume", soundEffectsVolume);
        SFXVolumePercent.text = (Mathf.Round((soundEffectsVolume + 80) * 100f / 100)).ToString() + " %";
    }

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

    public void TimerTotal()
    {
        timerTotal = timer + timerTotal;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public static void DisplayPetal(int petalIndex)
    {
        InGamePetals[petalIndex].SetActive(true);
    }
}
