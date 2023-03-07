using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class MainMenu : MonoBehaviour
	{
		[Header("Menu Windows")]
		[SerializeField] private GameObject _levelSelection;
		[SerializeField] private GameObject _mainMenuOptions;
		[Header("Level Selection")]
		[SerializeField] private List<Button> _levelsButtons = new();
		[SerializeField] private Sprite _levelButtonSprite;
		[SerializeField] private Sprite _levelButtonBlockedSprite;
		[Space]
		[Header("Button Start")]
		[SerializeField] private TMP_Text _startButton;
		[SerializeField] private string _start = "START";
		[SerializeField] private string _newGame = "NEW GAME";

		private GameManager _gameManager;

		private void Awake()
		{
			_gameManager = GameManager.Instance;
			UpdateLevelButtonsState();

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

		public void ShowLevelSelectionWindow(bool show)
		{
			_levelSelection.SetActive(show);
			_mainMenuOptions.SetActive(!show);
		}

		private void UpdateLevelButtonsState()
		{
			for (var i = 0; i < _levelsButtons.Count; ++i)
			{
				var active = _gameManager.UnlockedLevels.Contains(i + 1);
				_levelsButtons[i].gameObject.GetComponent<Image>().sprite
				= active ? _levelButtonSprite : _levelButtonBlockedSprite;

				_levelsButtons[i].interactable = active;
				_levelsButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(active);
			}
		}
	}
}
