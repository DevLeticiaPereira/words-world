using UnityEngine;
using UnityEngine.UI;

public class ResizeImageToScreen : MonoBehaviour
{
	private RectTransform _rectTransform;
	private Image _image;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		_image = GetComponent<Image>();
	}

	private void Start()
	{
		Resize();
	}

	private void Resize()
	{
		Vector2 screenSize = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
		_rectTransform.sizeDelta = screenSize;
		_image.preserveAspect = false;
	}
}
