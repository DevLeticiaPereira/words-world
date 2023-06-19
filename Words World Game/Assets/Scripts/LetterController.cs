using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class LetterController : MonoBehaviour
{
	public List<LetterContainer> _word = new();

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

		var word = GetWord();
		ResetWordFormation();

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
		AddPointToLineRenderer(letter.gameObject.transform);
		_wordBeingFormedText.text = letter.Letter.ToString().ToUpper();
	}

	private void AddPointToLineRenderer(Transform letterTransform)
	{
		_lineRendererController.AddPointToLine(letterTransform);
	}

	private void OnLetterPressed(LetterContainer letter)
	{
		if (!_isListening)
			return;

        if (_word.Contains(letter))
        {
            if (_word[_word.Count-1] == letter)
            {
				_word.Remove(letter);
				_lineRendererController.RemoveLastFixedPoint();
				_wordBeingFormedText.text = "";
                foreach (var letterInWord in _word)
                {
					_wordBeingFormedText.text += letterInWord.Letter.ToString().ToUpper();
				}
				
				//if all letters was removed cancel current word formation
                if (_word.Count == 0)
                {
                    ResetWordFormation();
                }
            }
			return;
        }

		_word.Add(letter);
		_wordBeingFormedContainer.SetActive(true);
		AddPointToLineRenderer(letter.gameObject.transform);
		_wordBeingFormedText.text = GetWord().ToUpper();
	}

    private void ResetWordFormation()
    {
        _isListening = false;
        _wordBeingFormedContainer.SetActive(false);
        _lineRendererController.Clear();
		_word.Clear();
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
