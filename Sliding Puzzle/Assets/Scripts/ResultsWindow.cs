using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsWindow : MonoBehaviour {
    private Text _timeText;
    private Text _movementsText;
    private Text _scoreText;

    private void Start() {
        GameManager.GetInstance().FinishGame += GameFinished;
        _timeText = transform.Find("Time").GetComponent<Text>();
        _movementsText = transform.Find("Movements").GetComponent<Text>();
        _scoreText = transform.Find("Score").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void GameFinished(object sender, EventArgs e) {
        _timeText.text = "Time: " + GameManager.GetInstance().GetMinutes().ToString() + ":" + GameManager.GetInstance().GetSeconds().ToString();
        _movementsText.text = "Movements: " + GameManager.GetInstance().GetGameMovements().ToString();
        _scoreText.text = "Score: " + GameManager.GetInstance().GetScore().ToString();
        gameObject.SetActive(true);
    }

    public void RetryButton() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void ShareButton() {
        Debug.Log("Share button clicked");
    }

    public void BackButton() {
        //SceneManager.LoadScene("Initial scene", LoadSceneMode.Single);
    }
}
