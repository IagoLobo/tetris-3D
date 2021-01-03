using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class that has the main menu button methods and spawns 3D view tetris blocks
public class MainMenuController : MonoBehaviour
{
    // Array that stores the tetris blocks that spawn in the main menu just for fun
    public GameObject[] tetrisBlocks3DView;

    private void Start()
    {
        Spawn3DViewTetrisBlock();
    }

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

    // Spawns a random tetris block to rotate in the main menu just to look nice and fancy
    public void Spawn3DViewTetrisBlock()
    {
        int randomID = Random.Range(0, tetrisBlocks3DView.Length);
        Instantiate(tetrisBlocks3DView[randomID], Vector3.zero, Quaternion.identity);
    }
}
