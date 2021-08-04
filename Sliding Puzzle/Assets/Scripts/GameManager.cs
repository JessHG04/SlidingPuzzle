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

    private void Suffle() {
        /*
        if(_emptySpaceIndex != 15) {
            Debug.Log("no se xd");
            var tileOn15LastPos = _tiles[15].GetTargetPosition();
            _tiles[15].SetTargetPosition(emptySpace.transform.position);
            emptySpace.transform.position = tileOn15LastPos;
            _tiles[_emptySpaceIndex] = _tiles[15];
            _tiles[15] = null;
            _emptySpaceIndex = 15;
        }*/
        int invertion;
        do{
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
            invertion = GetInversions();
            Debug.Log("Puzzle suffled");
        } while(invertion % 2 == 0);
    }

    private int FindIndex(Tile tile) {
        for(int x = 0; x < _tiles.Length; x++) {
            if(_tiles[x] != null) {
                if(_tiles[x] == tile) {
                    return x;
                }
            }
        }
        return -1;
    }

    // If GetInversions%2 == 0, the game is solvable
    private int GetInversions() {
        int inversions = 0;
        for(int x = 0; x < _tiles.Length; x++) {
            int thisTileInvertion = 0;
            for(int y = x; y < _tiles.Length; y++) {
                if(_tiles[y] != null) {
                    if(_tiles[x].id > _tiles[y].id) {
                        thisTileInvertion++;
                    }
                }
            }
            inversions += thisTileInvertion;
        }
        return inversions;
    }
}
