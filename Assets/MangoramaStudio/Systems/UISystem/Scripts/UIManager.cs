using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MangoramaStudio.Systems.UISystem.Scripts.Menus;
using Sirenix.OdinInspector;
using UnityEngine;


namespace MangoramaStudio.Scripts.Managers
{
    public class UIManager : BaseManager
    {
        
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameplayMenu gameplayMenu;
        [SerializeField] private WinMenu winMenu;

        public Action<MenuType> onChangeMenu;

        private List<BaseMenu> _menus = new();
        
        private BaseMenu _currentMenu;

        public GameplayMenu GameplayMenu() => gameplayMenu;
        
        
        public override void Initialize()
        {
            base.Initialize();
            AddMenusToList();
            ChangeMenu(MenuType.Main);
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                GameManager.EventManager.OnLevelStarted += StartLevel;
                GameManager.EventManager.OnOpenWinMenu += CompleteLevel;
                GameManager.EventManager.OnRestartLevel += StartLevel;
            }
            else
            {
                GameManager.EventManager.OnLevelStarted -= StartLevel;
                GameManager.EventManager.OnOpenWinMenu -= CompleteLevel;
                GameManager.EventManager.OnRestartLevel -= StartLevel;
            }
        }

        private void StartLevel()
        {
            ChangeMenu(MenuType.Gameplay);
        }

        private void CompleteLevel()
        {
            ChangeMenu(MenuType.Win);
        }

        private void ChangeMenu(MenuType menuType)
        {
    
            switch (menuType)
            {
                case MenuType.Main:
                    winMenu.gameObject.SetActive(false);
                    gameplayMenu.gameObject.SetActive(false);
                    mainMenu.Initialize();     
                    mainMenu.gameObject.SetActive(true);
                    _currentMenu = mainMenu;
                    break;
                case MenuType.Gameplay:
                    mainMenu.gameObject.SetActive(false);
                    winMenu.gameObject.SetActive(false);
                    gameplayMenu.Initialize();
                    gameplayMenu.gameObject.SetActive(true);
                    _currentMenu = gameplayMenu;
                    break;
                case MenuType.Win:
                    mainMenu.gameObject.SetActive(false);
                    gameObject.gameObject.SetActive(false);
                    winMenu.Initialize();
                    winMenu.gameObject.SetActive(true);
                    _currentMenu = winMenu;
                    break;
            }
        }

        private void AddMenusToList()
        {
            _menus.Add(mainMenu);
            _menus.Add(gameplayMenu);
            _menus.Add(winMenu);
        }
        
    }
    
}