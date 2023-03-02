using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
	public class GameManager : Singleton<GameManager>
	{
		public event Action<GameState> OnGameStateChanged;

		public GameState State { get; private set; }
		public int LastLevelCompleted { get; private set; }
		public int Score { get; private set; }
		public List<int> UnlockedLevels { get; } = new();

		[SerializeField] private string GamaplaySceneName = "GameStage";

		[SerializeField] private string MainMenuSceneName = "MainMenu";

		protected override void Awake()
		{
			UnlockedLevels.Add(1);
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
			if (Application.CanStreamedLevelBeLoaded(GamaplaySceneName))
				SceneManager.LoadScene(GamaplaySceneName);

			SetupLevel(level);
			UpdateGameState(GameState.LevelStart);
		}

		public void LoadLastUnlockedLevel()
		{
			if (Application.CanStreamedLevelBeLoaded(GamaplaySceneName))
			{
				SceneManager.LoadScene(GamaplaySceneName);
				SetupLevel(LastLevelCompleted + 1);
				UpdateGameState(GameState.LevelStart);
			}
			else
			{
				LoadMainMenu();
			}
		}

		private void SetupLevel(int level)
		{
			//todo: Ready a scriptable object that contains the level info and handle the level setup
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
			if (Application.CanStreamedLevelBeLoaded(MainMenuSceneName)
				&& SceneManager.GetActiveScene().name != MainMenuSceneName)
				SceneManager.LoadScene(MainMenuSceneName);
		}
	}

	public enum GameState
	{
		MainMenu = 0,
		LevelSelect = 1,
		LevelStart = 2,
		LevelCompleted = 3,
	}
}
