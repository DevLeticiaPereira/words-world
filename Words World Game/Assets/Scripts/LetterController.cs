using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class LetterController : MonoBehaviour
{
	public HashSet<LetterContainer> _word = new();

	[SerializeField] private LetterContainer _letterPrefab;
	[SerializeField] private GameObject _completedLevelWarning;

	[SerializeField] private GameObject _wordBeingFormedContainer;
	[SerializeField] private TMP_Text _wordBeingFormedText;
	[SerializeField] private float _offsetLettersFromBorder = 50.0f;
	[SerializeField] private LineRendererController _lineRendererController;

	[Space][Header("Gameplay Messages")][SerializeField]
	private TMP_Text _gameplayMessagesText;

	[SerializeField] private string _invalidWord = "Invalid Word";
	[SerializeField] private string _bonusWord = "Bonus Word";
	[SerializeField] private float _messageTimeOnScreen = 2.0f;

	private bool _isListening;
	private bool _displayingMessage;
	private GameManager _gameManager;
	private List<LetterContainer> _letters = new();
	private List<string> _wordsAlreadyDiscovered = new();
	private GameObject _gameplayMessageParentGameObject;

	private void Awake()
	{
		LetterContainer.OnLetterPressed += OnLetterPressed;
		LetterContainer.OnLetterListenBegin += OnLetterListenBegin;
		InputManager.OnTouchRelease += OnTouchRelease;
		GameManager.OnGameStateChanged += OnGameStateChanged;
		_gameManager = GameManager.Instance;
		_gameplayMessageParentGameObject
		= _gameplayMessagesText.gameObject.transform.parent.gameObject;

		if (LevelManager.Instance.CurrentLevel != null)
			GenerateLevelLetters(LevelManager.Instance.CurrentLevel.LevelLetters);
	}

	private void OnDestroy()
	{
		LetterContainer.OnLetterPressed -= OnLetterPressed;
		LetterContainer.OnLetterListenBegin -= OnLetterListenBegin;
		InputManager.OnTouchRelease -= OnTouchRelease;
		GameManager.OnGameStateChanged -= OnGameStateChanged;
	}

	/// <summary>
	/// Set up the letter controller adding and removing the level letters according to the game state.
	/// </summary>
	private void OnGameStateChanged(GameManager.GameState gameState)
	{
		if (gameState == GameManager.GameState.LevelStart)
		{
			GenerateLevelLetters(LevelManager.Instance.CurrentLevel.LevelLetters);
			return;
		}

		if (gameState == GameManager.GameState.LevelCompleted)
			StartCoroutine(DisplayCompletedLevelPopup());

		_wordsAlreadyDiscovered.Clear();

		if (_letters.Count > 0)
		{
			foreach (var letter in _letters)
			{
				Destroy(letter.gameObject);
			}

			_letters.Clear();
		}
	}

	private IEnumerator DisplayCompletedLevelPopup()
	{
		_completedLevelWarning.SetActive(true);
		yield return new WaitForSeconds(GameManager.Instance.BetweenLevelWaitTime);

		_completedLevelWarning.SetActive(false);
	}

	/// <summary>
	/// Create the level letters that will be used in the level controller and position them
	/// around a circle with equal space from each other.
	/// </summary>
	private void GenerateLevelLetters(char[] lettersToGenerate)
	{
		var image = GetComponent<Image>();
		Vector2 center = image.rectTransform.position;
		var radius = image.rectTransform.rect.width / 2f - _offsetLettersFromBorder;
		var angle = 360f / lettersToGenerate.Length;

		for (var i = 0; i < lettersToGenerate.Length; i++)
		{
			var x = center.x + Mathf.Cos(Mathf.Deg2Rad * angle * i) * radius;
			var y = center.y + Mathf.Sin(Mathf.Deg2Rad * angle * i) * radius;
			var position = new Vector3(x, y, 0f);
			var letterObject
			= Instantiate(_letterPrefab, position, Quaternion.identity, transform);

			_letters.Add(letterObject);
			letterObject.SetLetter(lettersToGenerate[i]);
		}
	}

	/// <summary>
	/// handles the touch release on the letter controlling reading the formed word and determinating if the word is a valid a
	/// </summary>
	private void OnTouchRelease()
	{
		if (_gameManager.State != GameManager.GameState.LevelStart)
			return;

		if (!_isListening)
			return;

		_isListening = false;
		var word = GetWord();
		_lineRendererController.Clear();
		_wordBeingFormedContainer.SetActive(false);
		_word.Clear();

		// If the word has already been discovered, return
		if (_wordsAlreadyDiscovered.Exists(wordAlreadyDiscovered => string.Equals
			(wordAlreadyDiscovered, word, StringComparison.OrdinalIgnoreCase)))
			return;

		// If the word is valid, add it to the list of discovered words, update the score,
		// and display a message if necessary
		if (WordManager.Instance.IsWorldValid(word))
		{
			_wordsAlreadyDiscovered.Add(word);
			bool foundWordInLevel = LevelManager.Instance.CheckForWordInLevel(word);
			GameManager.Instance.PlayerScore(foundWordInLevel);
			if (!foundWordInLevel)
				StartCoroutine(DisplayGameplayMessage(_bonusWord));
		}
		else
		{
			StartCoroutine(DisplayGameplayMessage(_invalidWord));
		}
	}

	private void OnLetterListenBegin(LetterContainer letter)
	{
		if (_displayingMessage )
			return;

		_isListening = true;
		_word.Add(letter);
		AddPointToLineRenderer();
		_wordBeingFormedText.text = letter.Letter.ToString().ToUpper();
	}

	private void AddPointToLineRenderer()
	{
		_lineRendererController.AddPointToLine();
	}

	private void OnLetterPressed(LetterContainer letter)
	{
		if (!_isListening|| _word.Contains(letter))
			return;

		_word.Add(letter);
		_wordBeingFormedContainer.SetActive(true);
		AddPointToLineRenderer();
		_wordBeingFormedText.text = GetWord().ToUpper();
	}

	private string GetWord()
	{
		var word = "";

		foreach (var l in _word)
		{
			word += l.Letter;
		}

		return word;
	}

	private IEnumerator DisplayGameplayMessage(string message)
	{
		_displayingMessage = true;
		_gameplayMessagesText.text = message;
		_gameplayMessageParentGameObject.SetActive(true);
		yield return new WaitForSeconds(_messageTimeOnScreen);

		_gameplayMessageParentGameObject.SetActive(false);
		_displayingMessage = false;
	}
}
