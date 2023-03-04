using System;
using Managers;
using TMPro;
using UnityEngine;

public class Hud : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;

	private void Awake()
	{
		GameManager.OnScoreChanged += GameManagerOnOnScoreChanged;
	}

	private void GameManagerOnOnScoreChanged(int newScore)
	{
		_scoreText.text = newScore.ToString();
	}
}
