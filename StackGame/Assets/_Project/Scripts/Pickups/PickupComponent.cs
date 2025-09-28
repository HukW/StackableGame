using UnityEngine;

namespace _Project.Scripts.Pickups
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PickupComponent : MonoBehaviour
    {
        private Collider _collider;
        private Rigidbody _rigidbody;
        
        [SerializeField]
        private PickupsEnum _pickupType = PickupsEnum.None;
        public PickupsEnum PickupType => _pickupType;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public void PickUp()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Deliver()
        {
            Destroy(gameObject);
        }
    }
}
