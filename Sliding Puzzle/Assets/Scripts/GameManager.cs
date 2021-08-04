using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject emptySpace;
    private Camera _camera;

    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit) {
                var distance = Vector2.Distance(emptySpace.transform.position, hit.transform.position);
                if(distance <= 6) {
                    Vector2 lastEmptySpacePosition = emptySpace.transform.position;
                    Tile tile = hit.transform.GetComponent<Tile>();
                    emptySpace.transform.position = tile.GetTargetPosition();
                    tile.SetTargetPosition(lastEmptySpacePosition);
                }
            }
        }
    }
}
