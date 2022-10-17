using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    public void Move(Vector3Int cellPosition, Tilemap arrowTileMap, Tilemap collisionTileMap)
    {
        if (!IsValidMove(cellPosition, arrowTileMap, collisionTileMap)) return;
        transform.position = cellPosition + new Vector3(0.5f, 0.5f);
    }

    public IEnumerator MoveDownward(Tilemap arrowTileMap, Tilemap collisionTileMap, bool isTileSelected)
    {
        while (!isTileSelected)
        {
            var cellPosition = arrowTileMap.WorldToCell(transform.position + new Vector3(0, -1));
            if (arrowTileMap.HasTile(cellPosition))
            {
                Move(cellPosition, arrowTileMap, collisionTileMap);
                yield return new WaitForSeconds(0.3f);
            }
            yield return null;
        }
    }

    public bool IsValidMove(Vector3Int cellPosition, Tilemap arrowTileMap, Tilemap collsionTileMap)
    {
        if (!arrowTileMap.HasTile(cellPosition)) return false;
        if (collsionTileMap.HasTile(cellPosition)) return false;

        return true;
    }

    private enum Direction
    {
        Left, Right, Up, Down
    }
}
