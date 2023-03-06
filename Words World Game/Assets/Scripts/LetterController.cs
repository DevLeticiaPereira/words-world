﻿using System;
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
	[SerializeField] private GameObject _wordBeingFormedContainer;
	[SerializeField] private GameObject _completedLevelWarning;
	[SerializeField] private TMP_Text _wordBeingFormedText;
	[SerializeField] private float _offsetLettersFromBorder = 50.0f;

	private bool _isListening;
	private GameManager _gameManager;
	private List<LetterContainer> _letters = new();
	private List<string> _wordsAlreadyDiscovered = new();

	private void Awake()
	{
		LetterContainer.OnLetterPressed += OnLetterPressed;
		LetterContainer.OnLetterListenBegin += OnLetterListenBegin;
		InputManager.OnTouchRelease += OnTouchRelease;
		GameManager.OnGameStateChanged += OnGameStateChanged;
		_gameManager = GameManager.Instance;

		if(LevelManager.Instance.CurrentLevel != null)
			GenerateLevelLetters(LevelManager.Instance.CurrentLevel.LevelLetters);
	}

	private void OnDestroy()
	{
		LetterContainer.OnLetterPressed -= OnLetterPressed;
		LetterContainer.OnLetterListenBegin -= OnLetterListenBegin;
		InputManager.OnTouchRelease -= OnTouchRelease;
		GameManager.OnGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameManager.GameState gameState)
	{
		if (gameState == GameManager.GameState.LevelStart)
		{
			GenerateLevelLetters(LevelManager.Instance.CurrentLevel.LevelLetters);
			return;
		}

		if (gameState == GameManager.GameState.LevelCompleted)
		{
			StartCoroutine(DisplayCompletedLevelPopup());
		}

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

	private void OnTouchRelease()
	{
		if (_gameManager.State != GameManager.GameState.LevelStart)
			return;

		if (!_isListening)
			return;

		_isListening = false;
		var word = GetWord();

		_wordBeingFormedContainer.SetActive(false);
		_word.Clear();

		if (_wordsAlreadyDiscovered.Exists(wordAlreadyDiscovered => string.Equals
		(wordAlreadyDiscovered, word, StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}

		if (WordManager.Instance.IsWorldValid(word))
		{
			_wordsAlreadyDiscovered.Add(word);
			bool foundWordInLevel = LevelManager.Instance.CheckForWordInLevel(word);
			GameManager.Instance.PlayerScore(foundWordInLevel);
		}
		else
		{
			//todo: display message word is not valid
		}
	}

	private void OnLetterListenBegin(LetterContainer letter)
	{
		_isListening = true;
		_word.Add(letter);

		_wordBeingFormedText.text = letter.Letter.ToString().ToUpper();
	}

	private void OnLetterPressed(LetterContainer letter)
	{
		if (!_isListening)
			return;

		_word.Add(letter);
		_wordBeingFormedContainer.SetActive(true);
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
}
