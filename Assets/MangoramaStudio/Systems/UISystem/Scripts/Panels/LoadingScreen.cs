using MatchinghamGames.GameUtilities.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider _loadingBar;
    private GameInitializeManager _gameInitializeManager;
    private Tween _loadingTween;

    public void Initialize(GameInitializeManager gameInitializeManager)
    {
        _gameInitializeManager = gameInitializeManager;
        _gameInitializeManager.InitializationProgressChanged += FillLoadingBar;
    }


    private void FillLoadingBar(float value)
    {
        if (_loadingTween != null)
        {
            _loadingTween.Kill();
        }
        _loadingTween = _loadingBar.DOValue(Mathf.Clamp(value + 0.5f, 0f, 1f), 1f).OnComplete(() =>
        {
            _loadingTween = null;
        });
    }

    private void OnDestroy()
    {
        _gameInitializeManager.InitializationProgressChanged -= FillLoadingBar;

        if (_loadingTween != null)
        {
            _loadingTween.Kill();
        }
    }

    private void OnDisable()
    {
        if (_loadingTween != null)
        {
            _loadingTween.Kill();
        }
    }
}
