using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

class LetterContainer : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	[SerializeField] private TMP_Text _letterText;

	public char Letter;

	public static event Action<LetterContainer> OnLetterPressed;
	public static event Action<LetterContainer> OnLetterListenBegin;

	public void SetLetter(char letter)
	{
		Letter = letter;
		_letterText.text = Letter.ToString().ToUpper();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnLetterPressed?.Invoke(this);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnLetterListenBegin?.Invoke(this);
	}
}
