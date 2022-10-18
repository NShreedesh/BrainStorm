using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class GridManager : MonoBehaviour
{
    private Input input;

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

    [Header("Inputs Values")]
    [SerializeField]
    private float dragDistance = 1;
    private Vector2 startPosition;
    private Vector2 endPosition;

    [Header("Sprites")]
    public Sprite upArrowSrpite;
    public Sprite leftArrowSrpite;

    private void OnEnable()
    {
        input = new Input();
        input.Enable();

        input.Player.Click.started += OnClickStarted;
        input.Player.Click.canceled += OnClickCanceled;

        input.Player.Position.performed += OnClickPerformed;
    }

    private void OnClickStarted(CallbackContext ctx)
    {
        var worldMousePosition = cam.ScreenToWorldPoint(input.Player.Position.ReadValue<Vector2>());
        RaycastHit2D raycastHit = Physics2D.Raycast(worldMousePosition, Vector2.zero, 10, tileLayerMask);
        if (raycastHit.collider == null) return;

        raycastHit.collider.TryGetComponent<Tile>(out Tile tile);
        if (tile == null) return;
        selectedTile = tile;

        startPosition = cam.ScreenToWorldPoint(input.Player.Position.ReadValue<Vector2>());
    }

    private void OnClickCanceled(CallbackContext obj)
    {
        if (selectedTile == null) return;
        selectedTile = null;
    }

    private void OnClickPerformed(CallbackContext ctx)
    {
        if (!input.Player.Click.IsPressed()) return;
        if (selectedTile == null) return;

        endPosition = cam.ScreenToWorldPoint(input.Player.Position.ReadValue<Vector2>());
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
