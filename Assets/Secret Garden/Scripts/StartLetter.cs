using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI letterText;
    

    private string story;

    private void Awake()
    {
        story = letterText.text;
        letterText.text = "";

        StartCoroutine("PlayText"); 
    }

    IEnumerator PlayText()
    {
        foreach(char c in story)
        {
            letterText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    


}
