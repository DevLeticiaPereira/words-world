using System;
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
		[Space]
		[Header("Score Points")]
		[SerializeField] private int _wordFoundInLevel = 2;
		[SerializeField] private int _wordFoundButNotInLevel = 1;


		public GameState State { get; private set; }
		public int LastLevelCompleted { get; private set; }
		public int Score { get; private set; }
		public List<int> UnlockedLevels { get; } = new();

		public static event Action<GameState> OnGameStateChanged;
		public static event Action<int> OnScoreChanged;

		public enum GameState
		{
			MainMenu = 0,
			LevelSelect = 1,
			LevelStart = 2,
			LevelCompleted = 3,
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
				case GameState.LevelSelect:
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
				UpdateGameState(GameState.LevelStart);
			}
			else
			{
				LoadMainMenu();
			}
		}

		public void PlayerScore(bool isWordInLevel)
		{
			Score += isWordInLevel ? _wordFoundInLevel : _wordFoundButNotInLevel;
			OnScoreChanged?.Invoke(Score);
		}

		private void SetupLevel(int level)
		{
			LevelManager.Instance.SetupLevel(level);
		}

		private void HandleLevelCompleted()
		{
			LastLevelCompleted += 1;
			UnlockedLevels.Add(LastLevelCompleted + 1);

			//if find next level in levels list
			SetupLevel(LastLevelCompleted + 1);
			//else
			//LoadMainMenu();
		}

		private void LoadMainMenu()
		{
			if (Application.CanStreamedLevelBeLoaded(_mainMenuSceneName)
				&& SceneManager.GetActiveScene().name != _mainMenuSceneName)
				SceneManager.LoadScene(_mainMenuSceneName);
		}
	}
}
