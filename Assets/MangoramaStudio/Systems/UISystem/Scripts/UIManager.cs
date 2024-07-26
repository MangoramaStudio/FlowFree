using System;
using System.Collections.Generic;
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

        public GameplayMenu GameplayMenu() => gameplayMenu;
        
        
        public override void Initialize()
        {
            base.Initialize();
            AddMenusToList();
            ToggleEvents(true);
            ChangeMenu(MenuType.Gameplay);
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                onChangeMenu += ChangeMenu;
                GameManager.EventManager.OnLevelStarted += StartLevel;
                GameManager.EventManager.OnLevelFinished += CompleteLevel;
            }
            else
            {
                onChangeMenu -= ChangeMenu;
                GameManager.EventManager.OnLevelStarted -= StartLevel;
                GameManager.EventManager.OnLevelFinished -= CompleteLevel;
            }
        }

        private void StartLevel()
        {
            ChangeMenu(MenuType.Gameplay);
        }

        private void CompleteLevel(bool isSuccess)
        {
            ChangeMenu(MenuType.Win);
        }

        private void ChangeMenu(MenuType menuType)
        {
            _menus.ForEach(x=>x.gameObject.SetActive(false));
            
            switch (menuType)
            {
                case MenuType.Main:
                    mainMenu.gameObject.SetActive(true);
                    mainMenu.Initialize();
                    break;
                case MenuType.Gameplay:
                    gameplayMenu.gameObject.SetActive(true);
                    gameplayMenu.Initialize();
                    break;
                case MenuType.Win:
                    winMenu.gameObject.SetActive(true);
                    winMenu.Initialize();
                    break;
            }
        }

        private void AddMenusToList()
        {
            _menus.Add(mainMenu);
            _menus.Add(gameplayMenu);
            _menus.Add(winMenu);
        }

        [Button]
        private void Test(MenuType type)
        {
            ChangeMenu(type);
        }
    }
    
}