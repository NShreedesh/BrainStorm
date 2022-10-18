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
            if (gridManager.arrowTileMap.GetSprite(cellPosition) == gridManager.upArrowSrpite)
            {
                Move(gridManager.blockMovingTileMap.WorldToCell(transform.position + new Vector3(0, 1)));
                yield return new WaitForSeconds(0.3f);
            }
            else if (gridManager.arrowTileMap.GetSprite(cellPosition) == gridManager.leftArrowSrpite)
            {
                Move(gridManager.blockMovingTileMap.WorldToCell(transform.position + new Vector3(-1, 0)));
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                var cellDownPosition = gridManager.blockMovingTileMap.WorldToCell(transform.position + new Vector3(0, -1));
                if (gridManager.blockMovingTileMap.HasTile(cellDownPosition))
                {
                    Move(cellDownPosition);
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return null;
        }
    }

    public bool IsValidMove(Vector3Int cellPosition)
    {
        if (!gridManager.blockMovingTileMap.HasTile(cellPosition)) return false;
        if (gridManager.collisionTileMap.HasTile(cellPosition)) return false;

        for (int i = 0; i < gridManager.blockArrowTileMap.transform.childCount; i++)
        {
            if (gridManager.blockArrowTileMap.WorldToCell(gridManager.blockArrowTileMap.transform.GetChild(i).transform.position) == cellPosition)
            {
                return false;
            }
        }

        return true;
    }
}