using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

namespace Code.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        private static LocalizationController _instance;
        public static LocalizationController Instance
        {
            get
            {
                if (_instance == null) 
                    throw new Exception("initialization order has been violated");
                return _instance;
            }
        }

        private void Start() 
        {
            if (_instance!=null) throw new Exception("second singleton");
            _instance = this;
            LoadLocalizedText(Locale.EN);
        }

        public enum Locale
        {
            EN,
            RU,
        }

        private Dictionary<string, string> _localizedText;
        private const string MissingTextString = "TEXT MISSED";
        private const string Path = "Texts/";
        public Action LocaleChanged;

        public void ChangeLocale(Locale locale)
        {
            LoadLocalizedText(locale);
        }
        
        private void LoadLocalizedText(Locale locale)
        {
            _localizedText = new Dictionary<string, string> ();
            var json = Resources.Load<TextAsset>(Path + locale).text;
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData> (json);
            foreach (var item in loadedData.Items)
            {
                _localizedText.Add (item.k, item.v);
            }
        }

        public string this[string key] 
        {
            get
            {
                if (_localizedText.ContainsKey(key))
                    return _localizedText[key];
                return MissingTextString;
            }
        }


    }
}