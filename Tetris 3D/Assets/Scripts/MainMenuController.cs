using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class that has the main menu button methods
public class MainMenuController : MonoBehaviour
{
    // Changes to the "Gameplay" scene
    public void PlayButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    // Closes the application
    public void ExitButton()
    {
        Application.Quit();
    }
}
