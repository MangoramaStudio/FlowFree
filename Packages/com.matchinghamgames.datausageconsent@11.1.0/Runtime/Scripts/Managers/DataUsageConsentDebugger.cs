using System.Linq;
using MatchinghamGames.Handyman;
using UnityEngine;

namespace MatchinghamGames.DataUsageConsent
{
	public class DataUsageConsentDebugger: ModuleDebugger<DataUsageConsentDebugger, DataUsageConsentManager, DataUsageConsentConfig>
	{

		protected override void InitializeRequiredParameters()
		{
			base.InitializeRequiredParameters();

			if (DataUsageConsentManager.Service != null)
			{
				AddRequiredView(DataUsageConsentManager.Service.GetType().Name, () => DataUsageConsentManager.Service.Ready, null, int.MinValue + 5);
			}
			
			AddRequiredView(nameof(DataUsageConsentManager.UserAdTracking), () => DataUsageConsentManager.UserAdTracking.ToString(), null, int.MinValue + 11);
			AddRequiredView(nameof(DataUsageConsentManager.CurrentArea), () => DataUsageConsentManager.CurrentArea.ToString(), null, int.MinValue + 12);
			AddRequiredView(nameof(DataUsageConsentManager.GeoLocationFetchingFailed), () => DataUsageConsentManager.GeoLocationFetchingFailed, null, int.MinValue + 14);
			AddRequiredView(nameof(DataUsageConsentManager.IsConsentRequiredInThisLocationFromCmp), () => DataUsageConsentManager.IsConsentRequiredInThisLocationFromCmp, null, int.MinValue + 15);
			AddRequiredView(nameof(DataUsageConsentManager.ShouldCollectConsentFromCmp), () => DataUsageConsentManager.ShouldCollectConsentFromCmp, null, int.MinValue + 16);
		}

		protected override void InitializeOptionalParameters()
		{
			AddOptionalButton("SetUserConsentToYes", () => DataUsageConsentManager.SetUserConsent(UserResponse.Yes));
			AddOptionalButton("SetUserConsentToNo", () => DataUsageConsentManager.SetUserConsent(UserResponse.No));
			AddOptionalButton("SetUserConsentToUnknown", () => DataUsageConsentManager.SetUserConsent(UserResponse.Unknown));
			AddOptionalView(nameof(ModuleConfig.GeoLocationDetectionMethod), () => ModuleConfig.GeoLocationDetectionMethod.ToString(), null, int.MinValue + 100);
			AddOptionalView(nameof(ModuleConfig.PreATTPopupEnabled), () => ModuleConfig.PreATTPopupEnabled, null, int.MinValue + 101);

			if (DataUsageConsentManager.Service == null) return;
			
			AddOptionalView(nameof(DataUsageConsentManager.CountryCode), () => DataUsageConsentManager.CountryCode, null, int.MinValue + 17);
			AddOptionalView(nameof(DataUsageConsentManager.RegionCode), () => DataUsageConsentManager.RegionCode, null, int.MinValue + 18);

			AddOptionalButton(nameof(DataUsageConsentManager.Service.LogTCFData), DataUsageConsentManager.Service.LogTCFData, int.MinValue + 300);
			AddOptionalButton(nameof(DataUsageConsentManager.Service.LogACData), DataUsageConsentManager.Service.LogACData, int.MinValue + 301);
			AddOptionalButton(nameof(DataUsageConsentManager.Service.ShowBasicConsentForm), DataUsageConsentManager.Service.ShowBasicConsentForm, int.MinValue + 302);
			AddOptionalButton(nameof(DataUsageConsentManager.Service.ShowAdvancedConsentForm), DataUsageConsentManager.Service.ShowAdvancedConsentForm, int.MinValue + 303);
			
			Module.WhenReady(_ =>
			{
				if (DataUsageConsentManager.ConsentMap == null) return;
			
				var i = 0;
				foreach (var pair in DataUsageConsentManager.ConsentMap)
				{
					AddOptionalView(pair.Key, () => pair.Value, null, int.MinValue + 300 + i++);
				}

				if (DataUsageConsentManager.TcfMap == null) return;

				var vcString = DataUsageConsentManager.TcfMap.Core.VendorConsents.Aggregate("", (current, consent) => current + $"{consent.Key};");
				AddOptionalView("VendorConsents", () => vcString, null, int.MinValue + 400);
			
				var vliString = DataUsageConsentManager.TcfMap.Core.VendorLegitimateInterests.Aggregate("", (current, interest) => current + $"{interest.Key};");
				AddOptionalView("VendorLegitimateInterests", () => vliString, null, int.MinValue + 401);
			
				var pcString = DataUsageConsentManager.TcfMap.Core.PurposesConsents.Aggregate("", (current, consent) => current + $"{consent.Key};");
				AddOptionalView("PurposeConsents", () => pcString, null, int.MinValue + 402);
			
				var liString = DataUsageConsentManager.TcfMap.Core.PurposesLegitimateInterests.Aggregate("", (current, interest) => current + $"{interest.Key};");
				AddOptionalView("PurposeLegitimateInterests", () => liString, null, int.MinValue + 403);	
			});
		}
	}
}
