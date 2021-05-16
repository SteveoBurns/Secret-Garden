using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Color32 mainMenuColor;

    public void TheAnyKey()
    {
        if (Input.anyKey)
        {
            StartCoroutine("FadeIn");
        }
    }

    public IEnumerator FadeIn()
    {
        Color32 Opaque = new Color32(255, 255, 255, 255);
        Color32 Transparent = new Color32(0, 0, 0, 0);

        while (true)
        {
            yield return mainMenuColor = Color32.Lerp(Transparent, Opaque, 3);
        }
    }
}
