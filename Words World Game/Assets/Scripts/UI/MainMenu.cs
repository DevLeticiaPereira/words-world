using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[Header("Button Start")]
		[SerializeField] private TMP_Text _startButton;
		[SerializeField] private string _start = "START";
		[SerializeField] private string _newGame = "NEW GAME";

		private GameManager _gameManager;

		private void Awake()
		{
			_gameManager = GameManager.Instance;

			if (_gameManager.LastLevelCompleted
				== LevelManager.Instance.TotalNumberOfLevels)
				_startButton.text = _newGame;
			else
				_startButton.text = _start;
		}

		public void SelectLevel(int level)
		{
			_gameManager.LoadLevel(level);
		}

		public void StartGame()
		{
			if (_startButton.text == _start)
			{
				_gameManager.LoadLastUnlockedLevel();
			}
			else if (_startButton.text == _newGame)
			{
				_gameManager.StartNewGame();
			}
		}
	}
}
