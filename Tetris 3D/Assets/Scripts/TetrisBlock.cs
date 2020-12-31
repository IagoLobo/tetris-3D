using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float lastTimeBlockFell = 0f;
    private float fallTimer = 1f;

    private void Update()
    {
        MoveBlock();
        BlockFallingDown();
    }

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
                GameManager.Instance.AddTetrisBlockToGrid(this.gameObject);
                GameManager.Instance.CheckRowCleared();
                GameManager.Instance.SpawnNewBlock();
                this.enabled = false;
            }
        }
    }

    private void RotateBlock()
    {
        Vector3 globalRotationPoint = transform.TransformPoint(rotationPoint);
        transform.RotateAround(globalRotationPoint, new Vector3(0, 0, 1), -90);
        if(!IsMoveValid())  transform.RotateAround(globalRotationPoint, new Vector3(0, 0, 1), 90);
    }

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
