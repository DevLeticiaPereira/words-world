using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
	public class LevelManager : Singleton<LevelManager>
	{
		[SerializeField] private List<Utilities.LevelSetup> _levelsSetup = new();
		[SerializeField] private GameObject _letterPrefab;
		[SerializeField] private RectTransform _centerRectTransform;

		private Dictionary<GridPosition, GameObject> _letterObjects = new();
		public LevelSetup CurrentLevel { get; private set; }
		public int NumberOfLevelWordDiscovered { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			GameManager.OnGameStateChanged += OnGameStateChanged;
		}

		private void OnGameStateChanged(GameManager.GameState gameState)
		{
			if (gameState == GameManager.GameState.LevelCompleted
				|| gameState == GameManager.GameState.MainMenu)
			{
				ClearGrid();
				NumberOfLevelWordDiscovered = 0;
				CurrentLevel = null;
			}
		}

		public bool SetupLevel(int level)
		{
			if (level>(_levelsSetup.Count))
			{
				return false;
			}
			CurrentLevel = _levelsSetup[level - 1];
			NumberOfLevelWordDiscovered = 0;
			CreateGrid();
			return true;
		}

		/// <summary>
		/// Sets up the level grid by creating letter objects on the grid and checking for conflicts.
		/// </summary>
		/// <param name="level">The level number to set up.</param>
		private void CreateGrid()
		{
			foreach (var wordData in CurrentLevel.WordDatas)
			{
				foreach (var letterData in wordData.Word)
				{
					if (_letterObjects.ContainsKey(letterData.LetterGridPosition))
					{
						var existingLetter = _letterObjects[letterData.LetterGridPosition];

						if (existingLetter.GetComponentInChildren<TMP_Text>(true).text
							.ToCharArray()
							[0]
							!= letterData.Letter)
						{
							Debug.LogError(
								$"Found different letters at grid position ({letterData.LetterGridPosition.Row},"
								+ $"{letterData.LetterGridPosition.Column})");
						}
					}
					else
					{
						CreateNewLetterContainer(letterData, CurrentLevel.GridRow,
							CurrentLevel.GridColumn);
					}
				}
			}
		}

		private void ClearGrid()
		{
			foreach (var letterObject in _letterObjects)
			{
				Destroy(letterObject.Value);
			}

			_letterObjects.Clear();
		}

		/// <summary>
		/// Creates a new letter object based on the given letter data and positions it on a grid layout.
		/// </summary>
		/// <param name="letterData">The data for the letter object to be created.</param>
		/// <param name="gridRow">The row of the grid where the letter object should be positioned.</param>
		/// <param name="gridColumn">The column of the grid where the letter object should be positioned.</param>
		private void CreateNewLetterContainer(LetterData letterData, int gridRow,
			int gridColumn)
		{
			var rectTransform = _letterPrefab.GetComponent<RectTransform>();
			var rectWidth = rectTransform.rect.width;
			var rectHeight = rectTransform.rect.height;
			var position = new Vector3(
				letterData.LetterGridPosition.Column * rectWidth,
				-letterData.LetterGridPosition.Row * rectHeight,
				0f
			);

			position -= new Vector3(
				rectWidth * (gridColumn - 1) / 2f,
				-rectHeight * (gridRow - 1) / 2f,
				0f
			);

			var letterObject = Instantiate(_letterPrefab, _centerRectTransform);
			letterObject.GetComponent<RectTransform>().anchoredPosition = position;
			var textComponentInChildren = letterObject.GetComponentInChildren<TMP_Text>();
			textComponentInChildren.text = letterData.Letter.ToString();
			textComponentInChildren.gameObject.SetActive(false);
			_letterObjects.Add(letterData.LetterGridPosition, letterObject);
		}

		public bool CheckForWordInLevel(string word)
		{
			foreach (var wordData in CurrentLevel.WordDatas)
			{
				string wordToCompare = GetWordFromData(wordData);
				if (string.Equals(word, wordToCompare, StringComparison.OrdinalIgnoreCase))
				{
					ShowWordInScene(wordData);
					++NumberOfLevelWordDiscovered;
					return true;
				}
			}
			return false;
		}

		private string GetWordFromData(WordData wordData)
		{
			return string.Join("", wordData.Word.Select(ld => ld.Letter));
		}

		private void ShowWordInScene(WordData wordData)
		{
			foreach (var letterData in wordData.Word)
			{
				if (_letterObjects.TryGetValue(letterData.LetterGridPosition, out var letterObject))
				{
					letterObject.GetComponentInChildren<TMP_Text>(true).gameObject.SetActive(true);
				}
			}
		}
	}
}
