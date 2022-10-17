using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public void Move(Vector3 cellPosition)
    {
        transform.position = cellPosition + new Vector3(0.5f, 0.5f);
    }

    private enum Direction
    {
        Left, Right, Up, Down
    }
}
