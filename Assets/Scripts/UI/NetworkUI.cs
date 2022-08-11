using TMPro;
using UnityEngine;

namespace UI
{
    public class NetworkUI : MonoBehaviour
    {
        public TextMeshProUGUI inputText;

        public string GetJoinCodeText()
        {
            return inputText.text;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
