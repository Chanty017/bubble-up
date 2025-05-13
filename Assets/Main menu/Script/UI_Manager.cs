using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    public void startbutton()
    {
        if (AudioOfmainemenu.instance != null)
        {
            AudioOfmainemenu.instance.PlaySFX("Button", 1f);
        }

        SceneManager.LoadScene(1);
    }
    public void ExitButton()
    {
        if (AudioOfmainemenu.instance != null)
        {
            AudioOfmainemenu.instance.PlaySFX("Button", 1f);
        }
        Application.Quit();
    }
    public void PressStart()
    {
        if (AudioOfmainemenu.instance!=null)
        {
            AudioOfmainemenu.instance.PlaySFX("Button", 1f);
        }
    }
     
   
}
