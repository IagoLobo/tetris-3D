using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Class that manages the game's grid, move validation and conditions
public class GameManager : MonoBehaviour
{
    // Game Manager instance so other scripts can call its methods more easily
    public static GameManager Instance;

    // Grid's dimensions
    private int gridHeight = 20;
    private int gridWidth = 10;

    // Random block spawn point
    [SerializeField] private Transform spawnPoint;

    // The variety of blocks that can be spawned during the gameplay
    [SerializeField] private GameObject[] tetrisBlocks;

    // Tetris block's fall speed
    private float blockFallSpeed = 1f;

    private bool changedBlockFallSpeed = false;

    // Game's grid, used to determine what block is static or not
    private Transform[,] grid = new Transform[10, 20];

    // Game's current score value
    private int currentScore;

    // Boolean that determines if the game is over or not
    private bool isGameOver = false;

    // Score Canvas and the score text that appears in this canvas
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    // Game Over Canvas and the score text that appears in this canvas
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    // Reference for the game's sound effects
    [SerializeField] private SFXManager sfxManager;

    public bool IsGameOver
    {
        get {   return isGameOver;   }
    }

    public int GridHeight
    {
        get {   return gridHeight;   }
    }

    public int GridWidth
    {
        get {   return gridWidth;   }
    }

    public Transform[,] Grid
    {
        get {   return grid;    }
    }

    private void Awake()
    {
        if(Instance == null)    Instance = this;
    }

    private void Start()
    {
        // Activate score canvas and disable game over canvas, it's play time
        scoreCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);

        // Reset game variables, update canvas text and spawn new block to start the game
        isGameOver = false;
        currentScore = 0;
        blockFallSpeed = 1f;
        changedBlockFallSpeed = false;
        UpdateScoreText(currentScore);
        SpawnNewBlock();
    }

    // Method that sets the tetris block fall speed
    // After every 50 points, the blocks fall a bit faster until it's really hard to keep up with their natural fall speed
    public float SetBlockFallSpeed()
    {
        if(currentScore != 0 && currentScore % 50 == 0 && blockFallSpeed > 0.15f && !changedBlockFallSpeed)
        {
            blockFallSpeed -= 0.1f;
            changedBlockFallSpeed = true;
        }

        return blockFallSpeed;
    }

    // Method that spawns a new block to keep the game going
    // If the new block spawns on top of a static block, it's GAME OVER!
    public void SpawnNewBlock()
    {
        int randomID = Random.Range(0, tetrisBlocks.Length);
        GameObject newTetrisBlock = Instantiate(tetrisBlocks[randomID], spawnPoint.position, Quaternion.identity);

        CheckBlockOverlap(newTetrisBlock);
    }

    // Method that adds a block to the game's grid, turning the block into a static one
    public void AddTetrisBlockToGrid(GameObject tetrisBlock)
    {
        foreach(Transform block in tetrisBlock.transform)
        {
            int xInt = Mathf.RoundToInt(block.transform.position.x);
            int yInt = Mathf.RoundToInt(block.transform.position.y);

            grid[xInt, yInt] = block;
        }
    }

    // Method that checks if any rows have been completed yet
    // If so, clears row, brings down the blocks above it and adds value to player's score
    public void CheckRowCleared()
    {
        for(int j = gridHeight - 1; j >= 0; --j)
        {
            if(IsRowFull(j))
            {
                ClearRow(j);
                BringRowsDown(j);
                AddScore();
            }
        }
    }

    // Method that checks if there's any overlap between a newly spawned block and a static one
    // If true, the game is over and the game over sequence is called
    private void CheckBlockOverlap(GameObject newTetrisBlock)
    {
        bool hasBlockOverlap = false;

        foreach(Transform block in newTetrisBlock.transform)
        {
            int xInt = Mathf.RoundToInt(block.transform.position.x);
            int yInt = Mathf.RoundToInt(block.transform.position.y);

            if(grid[xInt, yInt] != null)
            {
                hasBlockOverlap = true;
                break;
            }
        }

        if(hasBlockOverlap) GameOver();
    }

    // Method that checks if a given row is complete
    private bool IsRowFull(int j)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            if(grid[i,j] == null) return false;
        }

        return true;
    }

    // Method that destroys the blocks in a given row
    private void ClearRow(int j)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            Destroy(grid[i,j].gameObject);
            grid[i,j] = null;
        }
    }

    // Method that brings down blocks that are above an empty row
    private void BringRowsDown(int j)
    {
        for(int y = j; y < gridHeight; y++)
        {
            for(int i = 0; i < gridWidth; i++)
            {
                if(grid[i,y] != null)
                {
                    grid[i, y-1] = grid[i, y];
                    grid[i, y] = null;
                    grid[i, y-1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    // Adds value to player's score, updates canvas score text and plays a SFX
    private void AddScore()
    {
        currentScore += 10;
        UpdateScoreText(currentScore);
        sfxManager.PlayClearRowSFX();
        changedBlockFallSpeed = false;
    }

    // Updates the player's score text
    private void UpdateScoreText(int score)
    {
        string scoreText = "Score: \n" + $"{score}";
        currentScoreText.text = scoreText;
        gameOverScoreText.text = scoreText;
    }

    // Method that sets game over state, updates player's score and shows game over screen
    private void GameOver()
    {
        isGameOver = true;
        UpdateScoreText(currentScore);
        scoreCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    // Button method to reload scene after game over
    public void PlayAgainButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    // Button method to go back to main menu scene
    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
