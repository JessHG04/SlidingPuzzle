using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectPuzzleWindow : MonoBehaviour {
    public event EventHandler StartGame;
    private static SelectPuzzleWindow _instance;

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        gameObject.SetActive(true);
    }

    public void Puzzle3x3(){
        GameManager.GetInstance().SetPuzzleType(GameManager.PuzzleType.Puzzle3x3);
        gameObject.SetActive(false);
        if(StartGame != null) StartGame(this, EventArgs.Empty);
    }

    public void Puzzle4x4(){
        GameManager.GetInstance().SetPuzzleType(GameManager.PuzzleType.Puzzle4x4);
        gameObject.SetActive(false);
        if(StartGame != null) StartGame(this, EventArgs.Empty);
    }

    public void Puzzle5x5(){
        GameManager.GetInstance().SetPuzzleType(GameManager.PuzzleType.Puzzle5x5);
        gameObject.SetActive(false);
        if(StartGame != null) StartGame(this, EventArgs.Empty);
    }

    public void BackButton() {
        Application.Quit();
    }

    public static SelectPuzzleWindow GetInstance() => _instance;
}
