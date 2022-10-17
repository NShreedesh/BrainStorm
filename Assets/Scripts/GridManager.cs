using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputSystem.InputAction;

public class GridManager : MonoBehaviour
{
    public Camera cam;

    public Tilemap arrowTileMap;
    public Tile arrowTilePrefab;
    public Tile selectedTile;
    public LayerMask tileLayerMask;

    public Input input;
    public Vector2 startPosition;
    public Vector2 endPosition;

    public float dragDistance;

    private void OnEnable()
    {
        input = new Input();
        input.Enable();

        input.Player.Click.started += OnClickStarted;
        input.Player.Click.canceled += OnClickCanceled;

        input.Player.Position.performed += OnClickPerformed;
    }

    private void Start()
    {
        var cellPosition = arrowTileMap.WorldToCell(transform.position);
        Instantiate(arrowTilePrefab, cellPosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
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
            var cellPosition = arrowTileMap.WorldToCell(selectedTile.transform.position + new Vector3(-1, 0));
            if (!arrowTileMap.HasTile(cellPosition)) return;
            selectedTile.Move(cellPosition);
            startPosition = endPosition;
        }
        else if (draggedDistance.x > dragDistance)
        {
            var cellPosition = arrowTileMap.WorldToCell(selectedTile.transform.position + new Vector3(1, 0));
            if (!arrowTileMap.HasTile(cellPosition)) return;
            selectedTile.Move(cellPosition);
            startPosition = endPosition;
        }
    }
}
