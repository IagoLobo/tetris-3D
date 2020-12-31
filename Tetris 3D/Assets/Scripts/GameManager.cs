using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int gridHeight = 20;
    private int gridWidth = 10;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] tetrisBlocks;
    private Transform[,] grid = new Transform[10, 20];

    private int currentScore;
    private bool isGameOver = false;
    [SerializeField] private GameObject scoreCanvas;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
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
        scoreCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);

        isGameOver = false;
        currentScore = 0;
        UpdateScoreText(currentScore);
        SpawnNewBlock();
    }

    public void SpawnNewBlock()
    {
        int randomID = Random.Range(0, tetrisBlocks.Length);
        GameObject newTetrisBlock = Instantiate(tetrisBlocks[randomID], spawnPoint.position, Quaternion.identity);

        CheckBlockOverlap(newTetrisBlock);
    }

    public void AddTetrisBlockToGrid(GameObject tetrisBlock)
    {
        foreach(Transform block in tetrisBlock.transform)
        {
            int xInt = Mathf.RoundToInt(block.transform.position.x);
            int yInt = Mathf.RoundToInt(block.transform.position.y);

            grid[xInt, yInt] = block;
        }
    }

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

    private bool IsRowFull(int j)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            if(grid[i,j] == null) return false;
        }

        return true;
    }

    private void ClearRow(int j)
    {
        for(int i = 0; i < gridWidth; ++i)
        {
            Destroy(grid[i,j].gameObject);
            grid[i,j] = null;
        }
    }

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

    private void AddScore()
    {
        currentScore += 10;
        UpdateScoreText(currentScore);
        sfxManager.PlayClearRowSFX();
    }

    private void UpdateScoreText(int score)
    {
        string scoreText = "Score: \n" + $"{score}";
        currentScoreText.text = scoreText;
        gameOverScoreText.text = scoreText;
    }

    private void GameOver()
    {
        isGameOver = true;
        UpdateScoreText(currentScore);
        scoreCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
