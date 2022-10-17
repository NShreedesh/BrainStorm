using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GetComponentInParent<GridManager>();
        StartCoroutine(MoveDownward(gridManager.blockMovingTileMap, gridManager.collisionTileMap, gridManager.blockArrowTileMap));
    }

    public void Move(Vector3Int cellPosition, Tilemap arrowTileMap, Tilemap collisionTileMap, Tilemap blockArrowTileMap)
    {
        if (!IsValidMove(cellPosition, arrowTileMap, collisionTileMap, blockArrowTileMap)) return;
        transform.position = cellPosition + new Vector3(0.5f, 0.5f);
    }

    public IEnumerator MoveDownward(Tilemap arrowTileMap, Tilemap collisionTileMap, Tilemap blockArrowTileMap)
    {
        while (true)
        {
            var cellPosition = arrowTileMap.WorldToCell(transform.position + new Vector3(0, -1));
            if (arrowTileMap.HasTile(cellPosition))
            {
                Move(cellPosition, arrowTileMap, collisionTileMap, blockArrowTileMap);
                yield return new WaitForSeconds(0.3f);
            }
            yield return null;
        }
    }

    public bool IsValidMove(Vector3Int cellPosition, Tilemap arrowTileMap, Tilemap collsionTileMap, Tilemap blockArrowTileMap)
    {
        if (!arrowTileMap.HasTile(cellPosition)) return false;
        if (collsionTileMap.HasTile(cellPosition)) return false;

        for (int i = 0; i < blockArrowTileMap.transform.childCount; i++)
        {
            if (blockArrowTileMap.WorldToCell(blockArrowTileMap.transform.GetChild(i).transform.position) == cellPosition)
            {
                return false;
            }
        }

        return true;
    }

    private enum Direction
    {
        Left, Right, Up, Down
    }
}
