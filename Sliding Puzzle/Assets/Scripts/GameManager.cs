using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public EventHandler FinishGame;
    public enum PuzzleType {
        Puzzle3x3,
        Puzzle4x4,
        Puzzle5x5,
    }
    public enum State{
        WaitingToStart,
        Playing,
        ShowingResults,
    }
    private static GameManager _instance;
    private List<Tile> _tiles = new List<Tile>();
    private GameObject _emptySpace;
    private GameObject _background;
    private int _emptySpaceIndex;
    private Camera _camera;
    private float _gameTime = 0.0f;
    private int _gameMovements = 0;
    private State _gameState;
    private PuzzleType _puzzleType;

    #region Unity Methods

    private void Awake() {
        _instance = this;
    }
    private void Start() {
        SelectPuzzleWindow.GetInstance().StartGame += GameStarted;
        _camera = Camera.main;
        _gameState = State.WaitingToStart;
    }
    private void Update() {
        switch(_gameState) {
            case State.WaitingToStart:
                break;
            case State.Playing:
                _gameTime += Time.deltaTime;
                if(Input.GetMouseButtonDown(0)) {
                    MoveTile();
                }
                CheckGameOver();
                break;
            case State.ShowingResults:
                break;
        }
    }

    #endregion

    #region Utility Methods

    private void GameStarted(object sender, EventArgs e) {
        _emptySpace = GameObject.Find("Empty Space");
        _background = GameObject.Find("Background Square");
        
        switch(_puzzleType) {
            case PuzzleType.Puzzle3x3:
                _tiles = GameObject.Find("Tiles 3x3").GetComponentsInChildren<Tile>().ToList();
                Board.GetInstance().SetRows(3);
                Board.GetInstance().SetColumns(3);
                GameObject.Find("Tiles 4x4").SetActive(false);
                GameObject.Find("Tiles 5x5").SetActive(false);
                _emptySpace.transform.position = new Vector3(6.0f, -6.0f, -1.0f);
                _background.transform.localScale = new Vector3(3.15f, 3.15f, 1.0f);
                break;

            case PuzzleType.Puzzle4x4:
                _tiles = GameObject.Find("Tiles 4x4").GetComponentsInChildren<Tile>().ToList();
                Board.GetInstance().SetRows(4);
                Board.GetInstance().SetColumns(4);
                GameObject.Find("Tiles 3x3").SetActive(false);
                GameObject.Find("Tiles 5x5").SetActive(false);
                _emptySpace.transform.position = new Vector3(9.0f, -9.0f, -1.0f);
                _background.transform.localScale = new Vector3(4.15f, 4.15f, 1.0f);
                break;

            case PuzzleType.Puzzle5x5:
                _tiles = GameObject.Find("Tiles 5x5").GetComponentsInChildren<Tile>().ToList();
                Board.GetInstance().SetRows(5);
                Board.GetInstance().SetColumns(5);
                GameObject.Find("Tiles 3x3").SetActive(false);
                GameObject.Find("Tiles 4x4").SetActive(false);
                _emptySpace.transform.position = new Vector3(12.0f, -12.0f, -1.0f);
                _background.transform.localScale = new Vector3(5.15f, 5.15f, 1.0f);
                break;
        }
        _emptySpaceIndex = _tiles.Count;
        _tiles.Add(null);
        _gameState = State.Playing;
        Suffle();
        Board.GetInstance().InitList(_tiles);
        //Debug.Log("Min moves: " + _minMoves);
    }
    public void Suffle() {
        int inversions;
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
            inversions = GetInversions();
            //Debug.Log("Puzzle suffled");
        } while(inversions % 2 != 0);
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
        //Debug.Log("Inversions:" + inversions);
        return inversions;
    }

    private void MoveTile() {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if(hit) {
            //If not rounded, when the tile is moved, the distance is 5'9999999 or 6'000002 and dont move again
            int distance = Mathf.RoundToInt(Vector2.Distance(_emptySpace.transform.position, hit.transform.position));

            if(distance <= 6) {
                // Move tile
                Vector2 lastEmptySpacePosition = _emptySpace.transform.position;
                Tile tile = hit.transform.GetComponent<Tile>();
                _emptySpace.transform.position = tile.GetTargetPosition();
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
        if(CheckTiles()) {
            _gameState = State.ShowingResults;
            if(FinishGame != null) FinishGame(this, EventArgs.Empty);
            Debug.Log("Game Over");
        }

        if(Input.GetMouseButton(1)) {
            _gameState = State.ShowingResults;
            if(FinishGame != null) FinishGame(this, EventArgs.Empty);
        }
    }

    public bool CheckTiles() {
        int correctTiles = 0;
        for(int x = 0; x < _tiles.Count; x++) {
            if(_tiles[x] != null) {
                if (_tiles[x].IsInRightPlace()) {
                    correctTiles++;
                }
            }
        }
        return correctTiles == _tiles.Count - 1;
    }
    //  Calculate the min moves to solve the puzzle
    private int CalculateMinMoves() {
        int minMoves = 0;
        Queue<List<List<int>>> q = new Queue<List<List<int>>>();
        for(int x = 0; x < _tiles.Count; x++) {
            

        }


        return minMoves;
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
    public State GetGameState() => _gameState;
    public void SetPuzzleType(PuzzleType type) => _puzzleType = type;
}
