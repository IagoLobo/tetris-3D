using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that controls the Tetris blocks during their fall
public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float lastTimeBlockFell = 0f;
    private float fallTimer = 1f;

    private void Start()
    {
        // Sets this block's fall speed
        fallTimer = GameManager.Instance.SetBlockFallSpeed();
    }

    private void Update()
    {
        // While game isn't over, keep block movement
        if(!GameManager.Instance.IsGameOver)
        {
            MoveBlock();
            BlockFallingDown();
        }
    }

    // Method that implements the block movement:
    // -> A or LeftArrow moves to the left
    // -> D or RightArrow moves to the right
    // -> W or UpArrow rotates the block to the right
    private void MoveBlock()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!IsMoveValid())  transform.position -= new Vector3(-1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if(!IsMoveValid())  transform.position -= new Vector3(1, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateBlock();
        }
    }

    // Method that controls the block's descent over time
    // The block falls faster if the player presses S or DownArrow
    private void BlockFallingDown()
    {
        lastTimeBlockFell += Time.deltaTime;
        if(lastTimeBlockFell >
        ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) ? fallTimer / 20 : fallTimer))
        {
            transform.position += new Vector3(0, -1, 0);
            lastTimeBlockFell = 0f;

            // If the block can't go down anymore, another block needs to spawn and this block needs to stop
            if(!IsMoveValid())
            {
                transform.position -= new Vector3(0, -1, 0);

                // Adds this block to the game grid as a static block
                GameManager.Instance.AddTetrisBlockToGrid(this.gameObject);

                // Checks if there is any rows that are complete
                GameManager.Instance.CheckRowCleared();

                // Spawns new block to fall
                GameManager.Instance.SpawnNewBlock();

                // Disables this object so it doesn't move anymore
                this.enabled = false;
            }
        }
    }

    // Method that rotates the tetris block
    private void RotateBlock()
    {
        // RotationPoint is local and RotateAround() only works with world positions, so we need to change to that first
        Vector3 globalRotationPoint = transform.TransformPoint(rotationPoint);
        transform.RotateAround(globalRotationPoint, new Vector3(0, 0, 1), -90);
        if(!IsMoveValid())  transform.RotateAround(globalRotationPoint, new Vector3(0, 0, 1), 90);
    }

    // Method that checks if the block's move is still in the grid
    // If it isn't, the move needs to be undone
    private bool IsMoveValid()
    {
        foreach(Transform block in transform)
        {
            int xInt = Mathf.RoundToInt(block.transform.position.x);
            int yInt = Mathf.RoundToInt(block.transform.position.y);

            if(xInt < 0 || xInt >= GameManager.Instance.GridWidth || yInt < 0 || yInt >= GameManager.Instance.GridHeight)
                return false;

            if(GameManager.Instance.Grid[xInt, yInt] != null)   return false;
        }

        return true;
    }
}
