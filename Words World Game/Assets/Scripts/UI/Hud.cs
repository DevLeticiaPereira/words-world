using System;
using Managers;
using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _journeyScoreText;
    [SerializeField] private Canvas _canvas;

    private void Awake()
    {
        GameManager.OnScoreChanged += GameManagerOnOnScoreChanged;
        _scoreText.text = GameManager.Instance.Score.ToString();
        _journeyScoreText.text = GameManager.Instance.JourneyScore.ToString();
    }

    private void OnDestroy()
    {
        GameManager.OnScoreChanged -= GameManagerOnOnScoreChanged;
    }

    public void ExitToMainMenu()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.MainMenu);
    }

    public void SetCanvasCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }

    private void GameManagerOnOnScoreChanged(int newScore, int newJourneyScore)
    {
        _scoreText.text = newScore.ToString();
        _journeyScoreText.text = newJourneyScore.ToString();
    }
}
