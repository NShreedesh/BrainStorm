using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class GridManager : MonoBehaviour
{

    [Header("Input")]
    [SerializeField]
    private InputController inputController;

    [Header("Inputs Values")]
    [SerializeField]
    private float dragDistance = 1;
    private Vector2 startPosition;
    private Vector2 endPosition;

    [Header("Camera")]
    [SerializeField]
    private Camera cam;

    [Header("TileMaps")]
    [SerializeField]
    public Tilemap blockMovingTileMap;
    public Tilemap collisionTileMap;
    public Tilemap blockArrowTileMap;
    public Tilemap arrowTileMap;

    [Header("Tiles")]
    public Tile arrowTilePrefab;
    public LayerMask tileLayerMask;
    private Tile selectedTile;

    [Header("Sprites")]
    public Sprite upArrowSrpite;
    public Sprite leftArrowSrpite;

    private void OnEnable()
    {
        inputController.OnClickStartedAction += OnClickStarted;
        inputController.OnClickCanceledAction += OnClickCanceled;
        inputController.OnPositionChangedAction += SwipeControl;
    }

    private void OnDisable()
    {
        inputController.OnClickStartedAction -= OnClickStarted;
        inputController.OnClickCanceledAction -= OnClickCanceled;
        inputController.OnPositionChangedAction -= SwipeControl;
    }

    private void OnClickStarted()
    {
        var worldMousePosition = cam.ScreenToWorldPoint(inputController.Position);
        RaycastHit2D raycastHit = Physics2D.Raycast(worldMousePosition, Vector2.zero, 10, tileLayerMask);
        if (raycastHit.collider == null) return;

        raycastHit.collider.TryGetComponent<Tile>(out Tile tile);
        if (tile == null) return;
        selectedTile = tile;

        startPosition = cam.ScreenToWorldPoint(inputController.Position);
    }

    private void OnClickCanceled()
    {
        if (selectedTile == null) return;
        selectedTile = null;
    }

    private void SwipeControl()
    {
        if (!inputController.IsPressed) return;
        if (selectedTile == null) return;

        endPosition = cam.ScreenToWorldPoint(inputController.Position);
        var draggedDistance = endPosition - startPosition;

        if (draggedDistance.x < -dragDistance)
        {
            var cellPosition = blockMovingTileMap.WorldToCell(selectedTile.transform.position + new Vector3(-1, 0));
            MoveToNextCell(cellPosition);
        }
        else if (draggedDistance.x > dragDistance)
        {
            var cellPosition = blockMovingTileMap.WorldToCell(selectedTile.transform.position + new Vector3(1, 0));
            if (arrowTileMap.GetSprite(cellPosition) == leftArrowSrpite) return;
            MoveToNextCell(cellPosition);
        }
    }

    private void MoveToNextCell(Vector3Int cellPosition)
    {
        var cellDownPosition = blockMovingTileMap.WorldToCell(selectedTile.transform.position + new Vector3(0, -1));
        if (!collisionTileMap.HasTile(cellDownPosition) && blockMovingTileMap.HasTile(cellDownPosition))
        {
            selectedTile = null;
            return;
        }
        selectedTile.Move(cellPosition);
        startPosition = endPosition;
    }

    public bool CheckTile(Tilemap tilemap, Vector3Int cellPosition)
    {
        if (!tilemap.HasTile(cellPosition)) return false;
        return true;
    }

    public bool CheckForGameObjectBrushTile(Tilemap tilemap, Vector3Int cellPosition)
    {
        for (int i = 0; i < tilemap.transform.childCount; i++)
        {
            if (tilemap.WorldToCell(tilemap.transform.GetChild(i).transform.position) == cellPosition)
                return true;
        }

        return false;
    }
}
