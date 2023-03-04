using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject _levelSelection;
        [SerializeField]
        private GameObject _mainMenuOptions;
        [SerializeField]
        private List<Button> _levelsButtons = new();
        [SerializeField]
        private Sprite _levelButtonSprite;
        [SerializeField]
        private Sprite _levelButtonBlockedSprite;

        private Managers.GameManager _gameManager;

        private void Awake()
        {
            _gameManager = Managers.GameManager.Instance;
            UpdateLevelButtonsState();
        }

        private void UpdateLevelButtonsState()
        {
            for (int i = 0; i < _levelsButtons.Count; ++i)
            {
                bool active = _gameManager.UnlockedLevels.Contains(i + 1);
				_levelsButtons[i].gameObject.GetComponent<Image>().sprite = active ? _levelButtonSprite : _levelButtonBlockedSprite;

                _levelsButtons[i].interactable = active;
                _levelsButtons[i].gameObject.transform.GetChild(0).gameObject.SetActive(active);
            }
        }

        public void SelectLevel(int level)
        {
            _gameManager.LoadLevel(level);
        }

        public void LoadCurrentLevel()
        {
            _gameManager.LoadLastUnlockedLevel();
        }

        public void ShowLevelSelectionWindow(bool show)
        {
            _levelSelection.SetActive(show);
            _mainMenuOptions.SetActive(!show);
        }
    }
}
