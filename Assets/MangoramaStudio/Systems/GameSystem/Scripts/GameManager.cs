using MangoramaStudio.Scripts.Controllers;
using MangoramaStudio.Systems.ReviewSystem.Scripts;

namespace MangoramaStudio.Scripts.Managers
{
    public class GameManager : BaseManager
    {
        public UIManager UIManager;
        public EventManager EventManager;
        public LevelManager LevelManager;
        public AnalyticsManager AnalyticsManager;
        public ReviewManager ReviewManager;
        public InputController Inputs;
        public AddressableManager AddressableManager { get; private set; }

        public static GameManager Instance;

        public void Awake()
        {
            Instance = this;

            AddressableManager = FindObjectOfType<AddressableManager>();
            EventManager.Initialize();
            UIManager.Initialize();
            LevelManager.Initialize();
            AnalyticsManager.Initialize();
            Inputs.Initialize();
            ReviewManager.Initialize();
        }
        

        private void Start()
        {
            EventManager.StartGame();
        }
    }
}