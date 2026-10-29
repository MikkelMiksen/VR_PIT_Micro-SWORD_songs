using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Sword_interaction
{
    public class SwordSwingTelemetry
    {
        private readonly Queue<Vector3> _samples = new Queue<Vector3>();
        public int SampleWindow { get; set; } = 30;
        public float AverageSpeed { get; private set; }
        public Vector3 LastSample { get; private set; }

        public void Sample(Vector3 position, float deltaTime)
        {
            if (deltaTime <= 0f) return;

            _samples.Enqueue(position);
            while (_samples.Count > SampleWindow)
                _samples.Dequeue();

            if (_samples.Count < 2)
            {
                AverageSpeed = 0f;
                LastSample = position;
                return;
            }

            Vector3[] sampleArray = _samples.ToArray();
            float totalDistance = 0f;
            for (int i = 1; i < sampleArray.Length; i++)
                totalDistance += Vector3.Distance(sampleArray[i - 1], sampleArray[i]);

            AverageSpeed = totalDistance / (sampleArray.Length - 1) / deltaTime;
            LastSample = position;
        }

        public void Clear()
        {
            _samples.Clear();
            AverageSpeed = 0f;
            LastSample = Vector3.zero;
        }
    }
}
