using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap arrowTileMap;

    [SerializeField]
    private Tile arrowTilePrefab;

    [SerializeField]
    private Tile spawnedTile;

    private void Start()
    {
        var cellPosition = arrowTileMap.WorldToCell(transform.position);
        spawnedTile = Instantiate(arrowTilePrefab, cellPosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var cellPosition = arrowTileMap.WorldToCell(spawnedTile.transform.position + new Vector3(-1, 0));
            if (!arrowTileMap.HasTile(cellPosition))
            {
                return;
            }

            spawnedTile.Move(cellPosition);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            var cellPosition = arrowTileMap.WorldToCell(spawnedTile.transform.position + new Vector3(1, 0));
            if (!arrowTileMap.HasTile(cellPosition))
            {
                return;
            }

            spawnedTile.Move(cellPosition);
        }
    }
}
