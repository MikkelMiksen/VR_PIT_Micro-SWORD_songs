using UnityEngine;
using PIW_assets.Sword_interaction;

namespace PIW_assets.worldBehaviour
{
    /// <summary>Two-minute survival objective: survive until time expires; zero health is a loss.</summary>
    public sealed class SurvivalSliceGameManager : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float survivalDurationSeconds = 120f;
        public float RemainingSeconds { get; private set; }
        public bool IsFinished { get; private set; }
        public bool DidWin { get; private set; }

        private void Start() => RemainingSeconds = survivalDurationSeconds;

        private void Update()
        {
            if (IsFinished) return;

            if (PlayerStats.Instance != null && PlayerStats.Instance.currentHealth <= 0f)
            {
                Finish(false);
                return;
            }

            RemainingSeconds = Mathf.Max(0f, RemainingSeconds - Time.deltaTime);
            if (RemainingSeconds <= 0f) Finish(true);
        }

        private void Finish(bool won)
        {
            IsFinished = true;
            DidWin = won;
            Debug.Log(won ? "Survival slice complete: you survived!" : "Survival slice complete: player died.");
        }
    }
}
