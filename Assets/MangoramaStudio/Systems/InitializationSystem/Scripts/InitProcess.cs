using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.GameUtilities.Managers;
using System.Threading.Tasks;
using UnityEngine;

public class InitProcess : MonoBehaviour
{
    [SerializeField] private int _delayForLoadingScreen;
    [SerializeField] private AddressableManager _addressableManager;

    [SerializeField] private GameInitializeManager _gameInitializeManagerPrefab;
    [SerializeField] private LoadingScreen _loadingScreen;

    private void Awake()
    {
        _addressableManager.FirstLevelLoaded += AfterFirstLevelLoadProcess;
        
    }

    private void OnDestroy()
    {
        _addressableManager.FirstLevelLoaded -= AfterFirstLevelLoadProcess;

    }

    private async void AfterFirstLevelLoadProcess()
    {
        await Task.Delay(1000*_delayForLoadingScreen);
        var initializerPrefab = Instantiate(_gameInitializeManagerPrefab);
        _loadingScreen.gameObject.SetActive(true);
        _loadingScreen.Initialize(initializerPrefab);
    }
}
