using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile : MonoBehaviour
{
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponentInParent<GridManager>();
    }

    private void Start()
    {
        StartCoroutine(AutoMove());
    }

    public void Move(Vector3Int cellPosition)
    {
        if (!IsValidMove(cellPosition)) return;
        transform.position = cellPosition + new Vector3(0.5f, 0.5f);
    }

    public IEnumerator AutoMove()
    {
        while (true)
        {
            var cellPosition = gridManager.blockMovingTileMap.WorldToCell(transform.position);
            if (gridManager.arrowTileMap.GetTile(cellPosition) == gridManager.upArrowTile)
            {
                yield return new WaitForSeconds(0.3f);
                Move(cellPosition + new Vector3Int(0, 1));
            }
            else if (gridManager.arrowTileMap.GetTile(cellPosition) == gridManager.leftArrowTile)
            {
                yield return new WaitForSeconds(0.3f);
                Move(cellPosition + new Vector3Int(-1, 0));
            }
            else
            {
                var cellDownPosition = cellPosition + new Vector3Int(0, -1);
                if (gridManager.CheckTile(gridManager.blockMovingTileMap, cellDownPosition))
                {
                    yield return new WaitForSeconds(0.3f);
                    Move(cellDownPosition);
                }
            }
            yield return null;
        }
    }

    public bool IsValidMove(Vector3Int cellPosition)
    {
        if (!gridManager.CheckTile(gridManager.blockMovingTileMap, cellPosition)) return false;
        if (gridManager.CheckTile(gridManager.collisionTileMap, cellPosition)) return false;
        if (gridManager.CheckForGameObjectBrushTile(gridManager.blockArrowTileMap, cellPosition)) return false;

        return true;
    }
}