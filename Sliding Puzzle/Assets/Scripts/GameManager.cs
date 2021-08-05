using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public EventHandler FinishGame;
    private static GameManager _instance;
    private List<Tile> _tiles = new List<Tile>();
    private GameObject emptySpace;
    private int _emptySpaceIndex;
    private Camera _camera;
    private bool _gameOver = false;
    private float _gameTime = 0.0f;
    private int  _gameMovements = 0;

    #region Unity Methods

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        _camera = Camera.main;
        emptySpace = GameObject.Find("Empty Space");
        _tiles = GameObject.Find("Tiles").GetComponentsInChildren<Tile>().ToList();
        _emptySpaceIndex = _tiles.Count;
        _tiles.Add(null);
        Suffle();
    }
    private void Update() {
        if(!_gameOver){
            _gameTime += Time.deltaTime;
            if(Input.GetMouseButtonDown(0)) {
                MoveTile();
            }
            CheckGameOver();
        }
    }

    #endregion

    #region Utility Methods

    private void Suffle() {
        int invertion;
        do{
            for(int x = 0; x < _tiles.Count - 1; x++) {
                if(_tiles[x] != null) {
                    var lastPost = _tiles[x].GetTargetPosition();
                    int randomIndex = Random.Range(0, _tiles.Count - 1);
                    _tiles[x].SetTargetPosition(_tiles[randomIndex].GetTargetPosition());
                    _tiles[randomIndex].SetTargetPosition(lastPost);
                    Tile tile = _tiles[x];
                    _tiles[x] = _tiles[randomIndex];
                    _tiles[randomIndex] = tile;
                }
            }
            invertion = GetInversions();
            //Debug.Log("Puzzle suffled");
        } while(invertion % 2 == 0);
    }

    // If GetInversions%2 == 0, the game is solvable
    private int GetInversions() {
        int inversions = 0;
        for(int x = 0; x < _tiles.Count; x++) {
            int thisTileInvertion = 0;
            for(int y = x; y < _tiles.Count; y++) {
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

    private void MoveTile() {
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

                _gameMovements++;
            }
        }
    }

    private int FindIndex(Tile tile) {
        for(int x = 0; x < _tiles.Count; x++) {
            if(_tiles[x] != null) {
                if(_tiles[x] == tile) {
                    return x;
                }
            }
        }
        return -1;
    }

    private void CheckGameOver() {
        int correctTiles = 0;
        for(int x = 0; x < _tiles.Count; x++) {
            if(_tiles[x] != null) {
                if (_tiles[x].IsInRightPlace()) {
                    correctTiles++;
                }
            }
        }
        
        if(correctTiles == _tiles.Count - 1) {
            _gameOver = true;
            if(FinishGame != null) FinishGame(this, EventArgs.Empty);
            Debug.Log("Game Over");
        }

        if(Input.GetMouseButton(1)) {
            _gameOver = true;
            if(FinishGame != null) FinishGame(this, EventArgs.Empty);
        }
    }

    #endregion

    public static GameManager GetInstance() => _instance;
    public float GetGameTime() => _gameTime;

    public string GetMinutes() {
        float minutes = Mathf.Floor(_gameTime / 60);
        return (minutes < 10 ? "0" : "") + minutes.ToString();
    }

    public string GetSeconds() {
        float seconds = Mathf.Floor(_gameTime % 60);
        return (seconds < 10 ? "0" : "") + seconds.ToString();
    }
    public int GetGameMovements() => _gameMovements;
}
