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
    public Text MasterVolumePercent;
    public Text MusicVolumePercent;

    public AudioMixer GameMusic;
    public float volumeLevel;
    public float setVolumeLevel;
    public Slider volumeSlider;
    float mutedVolume = 0;
    float masterVolume;
    float musicVolume;
    public bool muted;

    public Text timerUI;
    int minutes;
    int seconds;
    float timerStart;
    int timer;
    int nextScene;
    string niceTime;
    int timerTotal;

    private void Start()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);

        timerStart = 360f;

        nextScene = 0;

        muted = false;
        masterVolume = 1;
        musicVolume = 1;

        if (nextScene == 0)
        {
            PetalPanel.SetActive(false);
            TimerPanel.SetActive(false);
        }
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
                timerStart = 300;
                break;

            case 2:
                timerStart = 360;
                break;

            case 3:
                timerStart = 420;
                break;
            case 4:
                timerStart = timerTotal;
                break;
        }

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1;
    }

    public void Music()
    {
        MasterVolumePercent.text = (masterVolume / 1f).ToString() + "%";
        MusicVolumePercent.text  = (musicVolume / 1f).ToString() + "%";
    }

    public void Mute()
    {
        if(muted == false)
        {
            volumeLevel = mutedVolume;
            muted = true;
        }
        
        else if(muted == true)
        {
            volumeLevel = setVolumeLevel;
            muted = false;
        }
    }

    public void TimerTotal()
    {
        timerTotal = timer + timerTotal;
    }
}
