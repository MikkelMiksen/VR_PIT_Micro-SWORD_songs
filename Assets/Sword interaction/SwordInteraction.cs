using Oculus.Interaction;
using UnityEngine;

namespace PIW_assets.Sword_interaction
{
    public class SwordInteraction : MonoBehaviour
    {
        [Header("Sword Data")]
        public SwordData swordData;

        [Header("Meta Interaction Settings")]
        public Transform leftHand;
        public Transform rightHand;
        [SerializeField] private Grabbable _grabbable;
        [SerializeField] private Rigidbody _rb;

        private bool _isEquipped = false;

        public bool IsEquipped
        {
            get
            {
                if (_grabbable != null)
                    return _grabbable.SelectingPointsCount > 0;
                return _isEquipped;
            }
        }

        private void Awake()
        {
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            if (_grabbable == null) _grabbable = GetComponent<Grabbable>();
        }

        private void Update()
        {
            _isEquipped = IsEquipped;
        }

        public void Interact() { }
        public bool IsSheathed => false;
        public void Drop() { }
    }
}
