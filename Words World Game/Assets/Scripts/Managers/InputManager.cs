using System;
using UnityEngine;

namespace Managers
{
	public class InputManager : Singleton<InputManager>
	{
		public static event Action OnTouchRelease;

		private GameManager _gameManager;

		private void Start()
		{
			_gameManager = GameManager.Instance;
		}

		private void Update()
		{
			if (_gameManager.State != GameManager.GameState.LevelStart)
				return;

			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Ended)
					OnTouchRelease?.Invoke();
			}
		}
	}
}
