using MangoramaStudio.Scripts.Controllers;

namespace MangoramaStudio.Scripts.Managers
{
    public class GameManager : BaseManager
    {
        public UIManager UIManager;
        public EventManager EventManager;
        public LevelManager LevelManager;
        public AnalyticsManager AnalyticsManager;
        public InputController Inputs;
        public AddressableManager AddressableManager { get; private set; }

        public static GameManager Instance;

        public void Awake()
        {
            Instance = this;

            AddressableManager = FindObjectOfType<AddressableManager>();
            EventManager.Initialize(this);
            UIManager.Initialize(this);
            LevelManager.Initialize(this);
            AnalyticsManager.Initialize(this);
            Inputs.Initialize(this);
        }

        private void OnDismiss()
        {
            
        }

        private void Start()
        {
            EventManager.StartGame();
        }
    }
}