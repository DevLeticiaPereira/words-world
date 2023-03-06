using UnityEngine;

namespace Managers
{
	public class UIManager : Singleton<UIManager>
	{
		[SerializeField] private GameObject _mainMenuPrefab;
		[SerializeField] private GameObject _hudPrefab;

		private GameObject _mainMenu;
		private GameObject _hud;
		private RectTransform _rectTransform;

		protected override void Awake()
		{
			base.Awake();
			_rectTransform = GetComponent<RectTransform>();
			GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
		}

		private void GameManager_OnGameStateChanged(GameManager.GameState state)
		{
			switch (state)
			{
				case GameManager.GameState.MainMenu:
				{
					if (_hud != null)
						Destroy(_hud.gameObject);

					if (_mainMenu == null)
						_mainMenu = Instantiate(_mainMenuPrefab, _rectTransform);
				}

					break;
				case GameManager.GameState.LevelStart:
				{
					if (_mainMenu != null)
						Destroy(_mainMenu.gameObject);

					if (_hud == null)
						_hud = Instantiate(_hudPrefab, _rectTransform);
				}

					break;
				case GameManager.GameState.LevelCompleted:
					break;
				default:
					break;
			}
		}
	}
}
