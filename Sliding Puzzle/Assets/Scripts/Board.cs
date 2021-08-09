using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private static Board _instance;
    private int _rows;
    private int _columns;
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

    public void SetRows(int rows) => _rows = rows;
    public void SetColumns(int columns) => _columns = columns;
    public static Board GetInstance() => _instance;
}
