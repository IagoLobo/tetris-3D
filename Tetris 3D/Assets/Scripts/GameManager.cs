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
    [SerializeField] private TextMeshProUGUI currentScoreText;

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
        currentScore = 0;
        UpdateCurrentScore(currentScore);
        SpawnNewBlock();
    }

    public void SpawnNewBlock()
    {
        int randomID = Random.Range(0, tetrisBlocks.Length);
        Instantiate(tetrisBlocks[randomID], spawnPoint.position, Quaternion.identity);
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
        UpdateCurrentScore(currentScore);
    }

    private void UpdateCurrentScore(int score)
    {
        currentScoreText.text = "Score: \n" + $"{score}";
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
