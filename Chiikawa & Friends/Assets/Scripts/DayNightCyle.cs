using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace WorldTime {
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour {
        public float duration = 5f;

        [SerializeField] private Gradient gradient;
        private Light2D _light;
        private float _startTime;

        private void Awake() {
            _light = GetComponent<Light2D>();
            _startTime = Time.time;
        }

        // Update is called once per frame
        void Update() {
            // Calculate the time elapsed since the start time
            var timeElapsed = Time.time - _startTime;
            // Calculate the percentage based on the sine of the time elapsed
            var percentage = Mathf.Sin (timeElapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;
            // Clamp the percentage to be between 0 and 1
            percentage = Mathf.Clamp01(percentage);

            _light.color = gradient.Evaluate(percentage);  
        }
    }
}