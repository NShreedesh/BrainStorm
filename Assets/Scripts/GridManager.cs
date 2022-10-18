using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class GridManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private Camera cam;

    [Header("TileMaps")]
    [SerializeField]
    public Tilemap blockMovingTileMap;
    public Tilemap collisionTileMap;
    public Tilemap blockArrowTileMap;
    public Tilemap arrowTileMap;
    public Tile arrowTilePrefab;
    public Tile selectedTile;
    public LayerMask tileLayerMask;

    [Header("Sprites")]
    public Sprite upArrowSrpite;
    public Sprite leftArrowSrpite;

    [Header("Input")]
    [SerializeField]
    private InputController inputController;

    [Header("Inputs Values")]
    [SerializeField]
    private float dragDistance = 1;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void OnEnable()
    {
        inputController.OnClickStartedAction += OnClickStarted;
        inputController.OnClickPerformedAction += OnClickPerformed;
        inputController.OnClickCanceledAction += OnClickCanceled;
    }
    private void OnDisable()
    {
        inputController.OnClickStartedAction -= OnClickStarted;
        inputController.OnClickPerformedAction -= OnClickPerformed;
        inputController.OnClickCanceledAction -= OnClickCanceled;
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

    private void OnClickPerformed()
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
}
