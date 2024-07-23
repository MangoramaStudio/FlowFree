using System;
using System.Collections.Generic;
using System.Linq;
using MangoramaStudio.Scripts.Controllers;
using MangoramaStudio.Systems.AdsSystem.Scripts;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using MangoramaStudio.Systems.ReviewSystem.Scripts;
using MangoramaStudio.Systems.SoundSystem.Scripts;
using MangoramaStudio.Systems.VibrationSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace MangoramaStudio.Scripts.Managers
{
    public class GameManager : BaseManager
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private EventManager eventManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private AnalyticsManager analyticsManager;
        [SerializeField] private ReviewManager reviewManager;
        [SerializeField] private InputController inputController;
        [SerializeField] private PopupManager popupManager;
        [SerializeField] private VibrationManager vibrationManager;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private AdsManager adsManager;
        public EventManager EventManager => eventManager;
        public LevelManager LevelManager => levelManager;
        public AddressableManager AddressableManager { get; private set; }

        public VibrationManager VibrationManager => vibrationManager;
        public SoundManager SoundManager => soundManager;

        public AdsManager AdsManager => adsManager;
        
        public static GameManager Instance;

        private List<BaseManager> _managers = new();

        public void Awake()
        {
            Instance = this;

            AddressableManager = FindObjectOfType<AddressableManager>();
            GatherManagers();
            InitializeManagers();
        }
        
        private void Start()
        {
            EventManager.StartGame();
        }
        
        
        private void GatherManagers()
        {
            _managers = GetComponentsInChildren<BaseManager>().ToList();
        }

        private void InitializeManagers()
        {
            _managers.ForEach(x=>x.Initialize());   
        }
    }
}