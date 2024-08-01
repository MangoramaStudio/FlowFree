using System;
using MangoramaStudio.Scripts.Managers;
using MangoramaStudio.Systems.PopupSystem.Scripts;
using MatchinghamGames.VegasModule;
using UnityEngine;

namespace MangoramaStudio.Systems.AdsSystem.Scripts
{
    public class AdsManager : BaseManager
    {
        
        protected override void ToggleEvents(bool isToggled)
        {
            var eventManager = GameManager.Instance.EventManager;
            
            base.ToggleEvents(isToggled);
            if (isToggled)
            {
                eventManager.OnShowInterstitial += ShowInterstitial;
                eventManager.OnShowRewarded += ShowRewarded;
            }
            else
            {
                eventManager.OnShowInterstitial -= ShowInterstitial;
                eventManager.OnShowRewarded -= ShowRewarded;
            }
        }

        private bool IsReady()
        {
            return Vegas.Instance.Ready;
        }

        public void ShowBanner(string adTag = null)
        {
            Vegas.Banner.Show(adTag);
        }

        public void HideBanner(string adTag = null)
        {
            Vegas.Banner.Hide();
        }

        private void ShowInterstitial(string adTag = null)
        {
            if (!IsReady())
            {
                Debug.LogError("Vegas is not ready");
                return;
            }
            Vegas.Interstitial.Show(adTag);
            Debug.Log($"AdsManager : Interstitial show send");
        }

        private void ShowRewarded(Action onRewardedSuccess,Action onRewardedFailure,string adTag = null)
        {
            if (!IsReady())
            {
                GameManager.Instance.EventManager.OpenPopup(PopupType.AdsNotReady);
                Debug.LogError("Vegas is not ready");
                return;
            }
     
            Vegas.RewardedVideo.Show(onRewardedSuccess,onRewardedFailure,adTag);
            Debug.Log($"AdsManager : Rewarded show send");
        }
    }
}