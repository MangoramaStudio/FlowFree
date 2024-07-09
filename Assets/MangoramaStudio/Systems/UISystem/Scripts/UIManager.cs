using System;
using System.Collections.Generic;
using MangoramaStudio.Systems.UISystem.Scripts.Menus;
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
        
        
        public override void Initialize()
        {
            base.Initialize();
            AddMenusToList();
            ToggleEvents(true);
            ChangeMenu(MenuType.Main);
        }

        protected override void ToggleEvents(bool isToggled)
        {
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                onChangeMenu += ChangeMenu;
            }
            else
            {
                onChangeMenu -= ChangeMenu;
            }
        }

        private void ChangeMenu(MenuType menuType)
        {
            _menus.ForEach(x=>x.gameObject.SetActive(false));
            
            switch (menuType)
            {
                case MenuType.Main:
                    mainMenu.gameObject.SetActive(true);
                    break;
                case MenuType.Gameplay:
                    gameplayMenu.gameObject.SetActive(true);
                    break;
                case MenuType.Win:
                    winMenu.gameObject.SetActive(true);
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