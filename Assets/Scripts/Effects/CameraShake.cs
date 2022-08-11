using UnityEngine;

namespace Effects
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake instance;

        public float maximumShake;
        public float shakeIntensity = 1f;
        public float dampen = 0.9f;

        private float _currentShake;
        private Vector3 _intial;

        public void Shake(float amount)
        {
            _currentShake += amount;
            if (_currentShake > maximumShake) _currentShake = maximumShake;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            _intial = transform.position;
        }

        private void Update()
        {
            transform.position += new Vector3(GetRandomShake(), GetRandomShake(), 0f);
            _currentShake *= dampen;
            transform.position = Vector3.Lerp(transform.position, _intial, Time.deltaTime * 10f);
        }

        private float GetRandomShake()
        {
            return (-0.5f + Random.value) * shakeIntensity * _currentShake;
        }
    }
}
