using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

class Letter : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	[SerializeField] private TMP_Text _letterText;

	public string _letter;

	public static event Action<Letter> OnLetterPressed;
	public static event Action<Letter> OnLetterListenBegin;

	private void Awake()
	{
		_letterText.text = _letter;
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
