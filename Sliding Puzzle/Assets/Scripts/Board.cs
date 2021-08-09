using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private static Board _instance;
    private int _rows;
    private int _columns;
    private int _minMoves = 0;
    private int[][] _dir = new int[][] { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 } };
    private List<List<Tile>> _boardList = new List<List<Tile>>();

    private void Awake() {
        _instance = this;
    }

    public void InitList(List<Tile> list) {
        for(int i = 0; i < _rows; i++) {
            _boardList.Add(new List<Tile>());
            for(int j = 0; j < _columns; j++) {
                _boardList[i].Add(list[i * _columns + j]);
            }
        }

        CalculateMinMoves();
        Debug.Log("Min moves: " + _minMoves);
        
        // See List content
        //for(int i = 0; i < _rows; i++) {
        //    Debug.Log("Fila: " + i);
        //    for(int j = 0; j < _columns; j++) {
        //        if(_boardList[i][j] != null) {
        //            Debug.Log(_boardList[i][j].id);
        //        }else{
        //            Debug.Log(0);
        //        }
        //    }
        //}
    }

    public void CalculateMinMoves(){
        Queue <List<List<Tile>>> q = new Queue<List<List<Tile>>>();
        q.Enqueue(_boardList);
        SortedSet <List<List<Tile>>> visited = new SortedSet<List<List<Tile>>>();
        visited.Add(_boardList);

        for(int lvl = 1; q.Count != 0; lvl++) {
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

            //Hasta aqui, nice

            for(int k = 0; k < _rows; k++) {
                int nx = x + _dir[k][0];
                int ny = y + _dir[k][1];
                //Debug.Log(nx + " " + ny);
                if(nx < 0 || ny < 0 || nx >= _rows || ny >= _columns){
                    Debug.Log("a");
                    continue;
                }
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
                visited.Add(node);
                if(GameManager.GetInstance().CheckTiles()){
                    _minMoves = lvl;
                    break;
                }
                //Add to queue
                q.Enqueue(node);
                //Swap back
                node[x][y] = node[nx][ny];
                node[nx][ny] = temp;
            }
        }
    }

    public void SetRows(int rows) => _rows = rows;
    public void SetColumns(int columns) => _columns = columns;
    public static Board GetInstance() => _instance;
}
