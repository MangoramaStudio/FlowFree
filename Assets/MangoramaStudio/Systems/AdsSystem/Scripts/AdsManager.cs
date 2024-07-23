using System;
using MangoramaStudio.Scripts.Managers;
using MatchinghamGames.VegasModule;
using UnityEngine;

namespace MangoramaStudio.Systems.AdsSystem.Scripts
{
    public class AdsManager : BaseManager
    {

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
        
        public void ShowInterstitial(string adTag = null)
        {
            if (!IsReady())
            {
                Debug.LogError("Vegas is not ready");
                return;
            }
            Vegas.Interstitial.Show(adTag);
        }

        public void ShowRewarded(Action onRewardedSuccess,Action onRewardedFailure,string adTag = null)
        {
            if (!IsReady())
            {
                Debug.LogError("Vegas is not ready");
                return;
            }
     
            Vegas.RewardedVideo.Show(onRewardedSuccess,onRewardedFailure,adTag);
            
        }
    }
}