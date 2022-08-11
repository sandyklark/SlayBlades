using System.Collections;
using TMPro;
using UnityEngine;

namespace Effects
{
    public class DamageText : MonoBehaviour
    {
        public float fadeSeconds = 1f;
        public float speed = 1f;

        private TextMeshPro _text;
        private Color _start;
        private Color _clear;

        public void SetValue(int number)
        {
            _text.text = number.ToString();
        }

        public void SetColor(Color color)
        {
            _text.color = color;
            SetFadeColors();
        }

        private void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnEnable()
        {
            StartCoroutine(Fade());
        }

        private void SetFadeColors()
        {
            _start = _text.color;
            _clear = _start;
            _clear.a = 0f;
        }

        private IEnumerator Fade()
        {
            var time = 0f;
            while (time < fadeSeconds)
            {
                yield return null;

                var delta = time / fadeSeconds;
                var easeOutBack = EaseOutBack(delta);
                var easeInExpo = EaseInExpo(delta);
                _text.color = Color.Lerp( _start, _clear, easeInExpo);

                var transform1 = transform;
                var transformPosition = transform1.position;
                transformPosition.y += Time.deltaTime * speed * (1f - delta);
                transform1.position = transformPosition;
                transform1.localScale = Vector3.one * (1f - easeOutBack);

                time += Time.deltaTime;
            }

            gameObject.SetActive(false);
        }

        private static float EaseInExpo(float x) {
            return x == 0f ? 0f : Mathf.Pow(2, 10 * x - 10);
        }

        private static float EaseOutBack(float x) {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;

            return c3 * x * x * x - c1 * x * x;
        }
    }
}
