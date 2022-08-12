using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NetworkUI : MonoBehaviour
    {
        public Text inputText;

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
