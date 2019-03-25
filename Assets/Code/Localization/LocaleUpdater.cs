using UnityEngine;
using UnityEngine.UI;

namespace Code.Localization
{
    public class LocaleUpdater : MonoBehaviour
    {
        public string Key;
        
        private void Start()
        {
            var lc = LocalizationController.Instance;
            lc.LocaleChanged += UpdateText;
            GetComponent<Text>().text = lc[Key];
        }

        private void UpdateText()
        {
            GetComponent<Text>().text = LocalizationController.Instance[Key];
        }
    }
}