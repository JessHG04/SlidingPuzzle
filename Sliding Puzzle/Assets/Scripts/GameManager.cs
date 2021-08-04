using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject emptySpace;
    private int _emptySpaceIndex = 15;
    private Camera _camera;
    [SerializeField] private Tile[] _tiles;

    private void Start() {
        _camera = Camera.main;
        Suffle();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if(hit) {
                //If not rounded, when the tile is moved, the distance is 5'9999999 or 6'000002 and dont move again
                int distance = Mathf.RoundToInt(Vector2.Distance(emptySpace.transform.position, hit.transform.position));

                if(distance <= 6) {
                    // Move tile
                    Vector2 lastEmptySpacePosition = emptySpace.transform.position;
                    Tile tile = hit.transform.GetComponent<Tile>();
                    emptySpace.transform.position = tile.GetTargetPosition();
                    tile.SetTargetPosition(lastEmptySpacePosition);
                    
                    // Move tile in the array
                    int tileIndex = FindIndex(tile);
                    _tiles[_emptySpaceIndex] = _tiles[tileIndex];
                    _tiles[tileIndex] = null;
                    _emptySpaceIndex = tileIndex;
                }
            }
        }
    }

    public void Suffle() {
        for(int x = 0; x < _tiles.Length - 1; x++) {
            if(_tiles[x] != null) {
                var lastPost = _tiles[x].GetTargetPosition();
                int randomIndex = Random.Range(0, _tiles.Length - 1);
                _tiles[x].SetTargetPosition(_tiles[randomIndex].GetTargetPosition());
                _tiles[randomIndex].SetTargetPosition(lastPost);
                Tile tile = _tiles[x];
                _tiles[x] = _tiles[randomIndex];
                _tiles[randomIndex] = tile;
            }
        }
    }

    public int FindIndex(Tile tile) {
        for(int x = 0; x < _tiles.Length; x++) {
            if(_tiles[x] != null) {
                if(_tiles[x] == tile) {
                    return x;
                }
            }
        }
        return -1;
    }
}
