using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private static Board _instance;
    private int _rows;
    private int _columns;
    private int _minMoves = 0;
    //private int[][] _dir = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
    private int[] _dir = new int[] { 1, 0, -1, 0, 0, 1, 0, -1 };
    private int[][] _boardList;
    private string _sGoal;
    private string _sStart;
    private void Awake() {
        _instance = this;
    }

    public void InitList(List<Tile> list) {
        _boardList = new int[_rows][];
        for (int i = 0; i < _rows; i++) {
            _boardList[i] = new int[_columns];
        }

        for(int i = 0; i < _rows; i++) {
            for(int j = 0; j < _columns; j++) {
                if(list[i * _columns + j] != null){
                    _boardList[i][j] = list[i * _columns + j].id;
                }else{
                    _boardList[i][j] = 0;
                }
            }
        }

        //SeeBoardlist();
        
        //CalculateMinMoves();
        //Debug.Log("Min moves: " + _minMoves);
    }

    private void SeeList(List<Tile> board) {
        // See List content
        for(int i = 0; i < board.Count; i++) {
            if(board[i] != null) {
                Debug.Log(board[i].id);
            }else{
                Debug.Log(0);
            }
        }
    }

    private void SeeBoardlist() {
        // See Board List
        //Debug.Log("Board list");
        for(int i = 0; i < _boardList.Length; i++) {
            Debug.Log("Fila: " + i);
            for(int j = 0; j < _boardList[i].Length; j++) {
                Debug.Log(_boardList[i][j]);
            }
        }
    }

    private void CalculateMinMoves() {
        Queue<int> queue = new Queue<int>();
    }


    /*public void CalculateMinMoves(){
        Queue <List<List<Tile>>> q = new Queue<List<List<Tile>>>();
        q.Enqueue(_boardList);
        SortedSet <List<List<Tile>>> visited = new SortedSet<List<List<Tile>>>();
        visited.Add(_boardList);

        for(int lvl = 1; q.Count != 0; lvl++) {
            int sz = q.Count;
            Debug.Log("Sz: " + sz);
            while(sz-- != 0){
                List<List<Tile>> node = q.Dequeue();
                int x = -1;
                int y = -1;
                for (int i = 0; i < _boardList.Count; i++) {
                    for (int j = 0; j < _boardList[i].Count; j++) {
                        if(node[i][j] == null){
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }

                for(int k = 0; k < _rows; k++) {
                    int nx = x + _dir[k][0];
                    int ny = y + _dir[k][1];
                    //Debug.Log(nx + " " + ny);
                    if(nx < 0 || ny < 0 || nx >= _rows || ny >= _columns) continue;
                    //Swap contents
                    Tile temp = node[x][y];
                    node[x][y] = node[nx][ny];
                    node[nx][ny] = temp;
                    //Add to visited
                    if(visited.Contains(node)){
                        //Swap back
                        node[x][y] = node[nx][ny];
                        node[nx][ny] = temp;
                        continue;
                    }
                    Debug.Log("a");
                    visited.Add(node);
                    if(GameManager.GetInstance().CheckTiles()){
                        _minMoves = lvl;
                        break;
                    }
                    //Add to queue
                    q.Enqueue(node);
                    
                    Debug.Log(q.Count);
                    //Swap back
                    node[x][y] = node[nx][ny];
                    node[nx][ny] = temp;
                }
            }
            
        }
    }*/
    /*
    public int CalculateMinMoves() {
        int movements = 0;
        if(GameManager.GetInstance().CheckTiles()) return movements;
        List<List<Tile>> q = _boardList;
        //Debug.Log("Q");
        //SeeList(q);
        //List<List<Tile>> visited = new List<List<Tile>>();
        List<Tile> visited = new List<Tile>();
        //Debug.Log("visited");
        //SeeList(visited);
        Debug.Log(q.Count + " " + q[0].Count);
        Debug.Log("Elements: " + q.Count * q[0].Count);
        int sz = _rows * _columns;
        for(int lvl = 1; sz != 0; lvl++) {
            sz = (_rows * _columns) - ((lvl - 1) * _rows);
            Debug.Log("Sz: " + sz);
            while(sz != 0) {
                //Dequeue
                int x = -1;
                int y = -1;
                for (int i = 0; i < q.Count; i++) {
                    for (int j = 0; j < q[i].Count; j++) {
                        if(q[i][j] == null) {
                            x = i;
                            y = j;
                            break;
                        }
                    }
                }
                Debug.Log("x: " + x + " y: " + y);
                
                for(int k = 0; k < _rows; k++) {
                    int nx = x + _dir[k][0];
                    int ny = y + _dir[k][1];
                    //Debug.Log(nx + " " + ny);
                    if(nx < 0 || ny < 0 || nx >= _rows || ny >= _columns) continue;
                    //Swap contents
                    Tile temp = q[x][y];
                    q[x][y] = q[nx][ny];
                    q[nx][ny] = temp;
                    //Add to visited
                    if(visited.Contains(q[x][y])){
                        //Swap back
                        q[x][y] = q[nx][ny];
                        q[nx][ny] = temp;
                        continue;
                    }
                    visited.Add(q[x][y]);
                    Debug.Log("Visited: " + visited.Count);
                    SeeList(visited);

                    if(GameManager.GetInstance().CheckTiles()) return lvl;

                }
            }
        }

        return movements;

    }
    */

    public void SetRows(int rows) => _rows = rows;
    public void SetColumns(int columns) => _columns = columns;
    public static Board GetInstance() => _instance;
}
