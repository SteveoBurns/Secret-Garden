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

    public AudioMixer GameMusic;
    public float volumeLevel;
    public float setVolumeLevel;
    public Slider volumeSlider;
    float mutedVolume = 0;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }

        timer = (int)(timerStart - Time.time);

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);

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
        Application.Quit();
    }

    public void Continue()
    {
        TimerTotal();
        nextScene++;
        SceneManager.LoadScene(nextScene);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1;
    }

    public void Music()
    {

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
