using UnityEngine;

namespace _Project.Scripts.Pickups
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PickupComponent : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;
        [SerializeField]
        private Rigidbody _rigidbody;
        
        [SerializeField]
        private ItemTypes _pickupType = ItemTypes.None;
        public ItemTypes PickupType => _pickupType;
        
        public void PickUp()
        {
            DisableInterraction();
        }

        public void DisableInterraction()
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
