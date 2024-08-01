#if UNITY_IOS
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace MatchinghamGames.DataUsageConsent
{
    public class DUCMNativeDialog
    {
#pragma warning disable 0414
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern IntPtr _DUCMNativeDialog_AlertDialog(string paragraph1, string ppText, string tcText,
            string ppLink, string tcLink, string pressOkText, string okText, string gameobject, string method);
    
        private readonly string _paragraph1;
        private readonly string _ppText;
        private readonly string _tcText;
        private readonly string _ppLink;
        private readonly string _tcLink;
        private readonly string _pressOkText;
        private readonly string _okText;
#pragma warning restore 0414

        public DUCMNativeDialog(string paragraph1, string ppText, string tcText, string ppLink, string tcLink, string pressOkText, string okText)
        {
            _paragraph1 = paragraph1;
            _ppText = ppText;
            _tcText = tcText;
            _ppLink = ppLink;
            _tcLink = tcLink;
            _pressOkText = pressOkText;
            _okText = okText;
        }

        public void AlertDialog()
        {
            Marshal.PtrToStringAuto(_DUCMNativeDialog_AlertDialog(_paragraph1, _ppText, _tcText, _ppLink, _tcLink,
                _pressOkText, _okText, nameof(DataConsentLocalizedTexts), nameof(DataConsentLocalizedTexts.OnPreAttCallback)));
        }
    
    }
}
#endif
