using System;
using System.Collections.Generic;
using UnityEngine;

class LetterController : MonoBehaviour
{
	public HashSet<Letter> _word = new HashSet<Letter>();

	private bool _isListening;

	private void Awake()
	{
		Letter.OnLetterPressed += OnLetterPressed;
		Letter.OnLetterListenBegin += OnLetterListenBegin;
		InputManager.OnTouchRelease += OnTouchRelease;
	}

	private void OnTouchRelease()
	{
		if (!_isListening)
			return;

		_isListening = false;

		string newWordContainer = "";

		foreach (var l in _word)
		{
			newWordContainer += l._letter;
		}

		bool wordIsInTheDictionary = WordManager.Instance.IsWorldValid(newWordContainer);
		Debug.Log("the word: " + newWordContainer);
		Debug.Log("is in the dictionary: " + wordIsInTheDictionary);
		_word.Clear();
	}

	private void OnDestroy()
	{
		Letter.OnLetterPressed -= OnLetterPressed;
		Letter.OnLetterListenBegin -= OnLetterListenBegin;
		InputManager.OnTouchRelease -= OnTouchRelease;
	}

	private void OnLetterListenBegin(Letter letter)
	{
		_isListening = true;
		_word.Add(letter);
	}

	private void OnLetterPressed(Letter letter)
	{
		if (!_isListening)
		{
			return;
		}

		_word.Add(letter);
	}
}
