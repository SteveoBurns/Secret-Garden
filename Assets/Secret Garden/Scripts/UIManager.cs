using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject OptionsMenu;
    public GameObject PetalPanel;
    public GameObject TimerPanel;

    public GameObject[] petals;
    int petalIndex;

    public Text MusicVolumePercent;
    public Text SFXVolumePercent;

    public AudioMixerGroup[] Audio;
    public AudioSource musicSource;
    public AudioSource sfxsource;
    public bool muted;
    public AudioClip[] levelMusic;
    public AudioClip[] soundEffects;
    public Slider masterMixer;

    public Text timerUI;
    int minutes;
    int seconds;
    float timerStart;
    int timer;
    int nextScene;
    string niceTime;
    int timerTotal;

    public Dropdown resolution;
    public Resolution[] resolutions;

    private void Start()
    {
        Screen.fullScreen = true;

        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = levelMusic[0];
        musicSource.Play();

        nextScene = 0;

        muted = false;

        if (nextScene == 0)
        {
            PetalPanel.SetActive(false);
            TimerPanel.SetActive(false);
        }

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
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }

    public void OptionsButton()
    {
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Continue()
    {

        TimerTotal();
        nextScene++;
        SceneManager.LoadScene(nextScene);

        if (nextScene >= 1)
        {
            PetalPanel.SetActive(true);
            TimerPanel.SetActive(true);
        }

        switch (nextScene)
        {
            case 1:
                musicSource.clip = levelMusic[1];
                timerStart = 300;
                petalIndex = 0;
                break;

            case 2:
                musicSource.clip = levelMusic[2];
                timerStart = 360;
                petalIndex = 3;
                break;

            case 3:
                musicSource.clip = levelMusic[3];
                timerStart = 420;
                petalIndex = 6;
                break;
            case 4:
                musicSource.clip = levelMusic[0];
                timerStart = timerTotal;
                petalIndex = 9;
                break;
        }

        musicSource.Play();

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1;
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

    void OnCollisionPetal(Collision2D collision)
    {
        

        if(collision.gameObject.tag == "Petal")
        {
            petals[petalIndex].SetActive(true);
        }
        petalIndex++;
        
    }

}
