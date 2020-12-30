using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
