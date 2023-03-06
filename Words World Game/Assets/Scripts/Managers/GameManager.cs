using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		[Header("Scene Names")]
		[SerializeField] private string _gamaplaySceneName = "GameStage";
		[SerializeField] private string _mainMenuSceneName = "MainMenu";

		[Space][Header("Score Points")]
		[SerializeField] private int _wordFoundInLevel = 2;
		[SerializeField] private int _wordFoundButNotInLevel = 1;

		[Header("Other")]
		[SerializeField] private float _betweenLevelWaitTime = 3.0f;

		public static event Action<GameState> OnGameStateChanged;
		public static event Action<int, int> OnScoreChanged;

		public GameState State { get; private set; }
		public int LastLevelCompleted { get; private set; }
		public int JourneyScore { get; private set; }
		public int Score { get; private set; }
		public List<int> UnlockedLevels { get; } = new();
		public float BetweenLevelWaitTime { get; private set; }

		private int _currentLevel;

		public enum GameState
		{
			MainMenu = 0,
			LevelStart = 1,
			LevelCompleted = 2
		}

		protected override void Awake()
		{
			UnlockedLevels.Add(1);
			base.Awake();
		}

		private void Start()
		{
			UpdateGameState(GameState.MainMenu);
		}

		public void UpdateGameState(GameState newState)
		{
			State = newState;

			switch (newState)
			{
				case GameState.MainMenu:
					LoadMainMenu();
					break;
				case GameState.LevelStart:
					break;
				case GameState.LevelCompleted:
					HandleLevelCompleted();
					break;
			}

			OnGameStateChanged?.Invoke(newState);
		}

		public void LoadLevel(int level)
		{
			if (Application.CanStreamedLevelBeLoaded(_gamaplaySceneName))
				SceneManager.LoadScene(_gamaplaySceneName);

			SetupLevel(level);
			UpdateGameState(GameState.LevelStart);
		}

		public void LoadLastUnlockedLevel()
		{
			if (Application.CanStreamedLevelBeLoaded(_gamaplaySceneName))
			{
				SceneManager.LoadScene(_gamaplaySceneName);
				SetupLevel(LastLevelCompleted + 1);
			}
			else
			{
				UpdateGameState(GameState.MainMenu);
			}
		}

		public void PlayerScore(bool isWordInLevel)
		{
			//only receive score points first time playing level.
			if (_currentLevel != LastLevelCompleted + 1)
				return;

			Score += isWordInLevel ? _wordFoundInLevel : _wordFoundButNotInLevel;
			OnScoreChanged?.Invoke(Score, JourneyScore);
			if (LevelManager.Instance.NumberOfLevelWordDiscovered
				>= LevelManager.Instance.CurrentLevel
				.WordDatas
				.Count)
				UpdateGameState(GameState.LevelCompleted);
		}

		private void SetupLevel(int level)
		{
			_currentLevel = level;

			if (LevelManager.Instance.SetupLevel(LastLevelCompleted + 1))
			{
				UpdateGameState(GameState.LevelStart);
				return;
			}

			UpdateGameState(GameState.MainMenu);
		}

		private void HandleLevelCompleted()
		{
			TransferScoreToJourneySocore();
			LastLevelCompleted += 1;
			UnlockedLevels.Add(LastLevelCompleted + 1);
			StartCoroutine(LevelCompleteWaitToChangeLevel());
		}

		private void TransferScoreToJourneySocore()
		{
			JourneyScore += Score;
			Score = 0;
			OnScoreChanged?.Invoke(Score, JourneyScore);
		}

		private IEnumerator LevelCompleteWaitToChangeLevel()
		{
			yield return new WaitForSeconds(_betweenLevelWaitTime);

			SetupLevel(LastLevelCompleted + 1);
		}

		private void LoadMainMenu()
		{
			if (Application.CanStreamedLevelBeLoaded(_mainMenuSceneName)
				&& SceneManager.GetActiveScene().name != _mainMenuSceneName)
			{
				SceneManager.LoadScene(_mainMenuSceneName);
				Score = 0;
			}
		}
	}
}
