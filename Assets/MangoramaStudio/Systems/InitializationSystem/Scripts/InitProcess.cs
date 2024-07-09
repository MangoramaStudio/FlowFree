using System;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.GameUtilities.Managers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class InitProcess : MonoBehaviour
{
    [FormerlySerializedAs("_delayForLoadingScreen")] [SerializeField] private int delayForLoadingScreen;
    [FormerlySerializedAs("_addressableManager")] [SerializeField] private AddressableManager addressableManager;

    [FormerlySerializedAs("_gameInitializeManagerPrefab")] [SerializeField] private GameInitializeManager gameInitializeManagerPrefab;

    private void Awake()
    {
        addressableManager.FirstLevelLoaded += AfterFirstLevelLoadProcess;
        
    }

    private void OnDestroy()
    {
        addressableManager.FirstLevelLoaded -= AfterFirstLevelLoadProcess;

    }

    private async void AfterFirstLevelLoadProcess()
    {
        try
        {
            await Task.Delay(1000*delayForLoadingScreen,destroyCancellationToken);
            Instantiate(gameInitializeManagerPrefab);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
       
    }
}
