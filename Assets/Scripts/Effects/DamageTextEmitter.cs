using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace GameplaySingletons
{
    public class DamageTextEmitter : MonoBehaviour
    {
        public static DamageTextEmitter Instance { get; private set; }

        public DamageText textPrefab;
        public uint poolSize = 5;
        public Vector2 offset;

        private readonly Queue<DamageText> _textPool = new Queue<DamageText>();

        protected void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            for (var i = 0; i < poolSize; i++)
            {
                var text = Instantiate(textPrefab, transform);
                text.gameObject.SetActive(false);
                _textPool.Enqueue(text);
            }
        }

        public void Emit(Vector3 position, int number, Color color)
        {
            var text = _textPool.Dequeue();

            position += (Vector3)offset;
            text.transform.position = position;
            text.gameObject.SetActive(true);
            text.SetColor(color);
            text.SetValue(number);

            _textPool.Enqueue(text);
        }
    }
}
