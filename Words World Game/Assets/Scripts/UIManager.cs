using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private GameObject _mainMenuPrefab;
        [SerializeField]
        private GameObject _hudPrefab;

        private GameObject _mainMenu;
        private GameObject _hud;
        private GameManager _gameManager;
        private RectTransform _rectTransform;

        override protected void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
            _gameManager = GameManager.Instance;
            _gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        }

        private void GameManager_OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    {
                        if (_hud != null)
                        {
                            Destroy(_hud);
                        }
                        _mainMenu = Instantiate<GameObject>(_mainMenuPrefab, _rectTransform);
                    }
                    break;
                case GameState.LevelSelect:
                    break;
                case GameState.LevelStart:
                    {
                        if (_mainMenu != null)
                        {
                            Destroy(_mainMenu);
                        }
                        _hud = Instantiate<GameObject>(_hudPrefab, _rectTransform);
                    }
                    break;
                case GameState.LevelCompleted:
                    break;
                default:
                    break;
            }
        }
    }
}