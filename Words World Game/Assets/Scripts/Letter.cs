using System;
using UnityEngine;
using UnityEngine.EventSystems;

class Letter : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
	public string _letter;

	public static event Action<Letter> OnLetterPressed;
	public static event Action<Letter> OnLetterListenBegin;

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnLetterPressed?.Invoke(this);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnLetterListenBegin?.Invoke(this);
	}
}
