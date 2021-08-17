using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDWindow : MonoBehaviour {
    private Text _movements;
    private Text _time;

    private void Start() {
        SelectPuzzleWindow.GetInstance().StartGame += GameStarted;
        GameManager.GetInstance().FinishGame += GameFinished;
        _movements = transform.Find("Movements").GetComponent<Text>();
        _time = transform.Find("Time").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void GameStarted(object sender, EventArgs e) => gameObject.SetActive(true);
    private void Update() {
        _movements.text = GameManager.GetInstance().GetGameMovements().ToString();
        _time.text = GameManager.GetInstance().GetMinutes().ToString() + ":" + GameManager.GetInstance().GetSeconds().ToString();
    }
    private void GameFinished(object sender, EventArgs e) => gameObject.SetActive(false);
}
