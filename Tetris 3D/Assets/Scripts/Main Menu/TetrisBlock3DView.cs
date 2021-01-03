using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock3DView : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RotateBlockSlowlyCoroutine());
    }

    public IEnumerator RotateBlockSlowlyCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.01f);

            transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), -1);
        }
    }
}
