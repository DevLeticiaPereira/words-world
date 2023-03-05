using System.Collections;
using System.Collections.Generic;
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

		public void SetupLevel(int level)
		{
			LevelSetup levelSetup = _levelsSetup[level - 1];

			// Loop through each letter in the level setup
			foreach (var wordData in levelSetup.WordDatas)
			{
				foreach (var letterData in wordData.Word)
				{
					// Check if a letter already exists at the grid position
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
						CreateNewLetterContainer(letterData,levelSetup.GridRow, levelSetup.GridColumn);
					}
				}
			}
		}

		private void CreateNewLetterContainer(LetterData letterData, int gridRow, int gridColumn)
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
				rectWidth * (gridColumn-1) / 2f,
				-rectHeight * (gridRow-1) / 2f,
				0f
			);

			var letterObject = Instantiate(_letterPrefab, _centerRectTransform);
			letterObject.GetComponent<RectTransform>().anchoredPosition = position;
			var textComponentInChildren = letterObject.GetComponentInChildren<TMP_Text>();
			textComponentInChildren.text = letterData.Letter.ToString();
			textComponentInChildren.gameObject.SetActive(false);
			_letterObjects.Add(letterData.LetterGridPosition, letterObject);
		}
	}
}
