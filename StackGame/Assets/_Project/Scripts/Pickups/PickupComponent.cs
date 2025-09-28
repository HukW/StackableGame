using System.Collections;
using DG.Tweening;
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
        
        [SerializeField]
        private float _animationDuration = 0.5f;
        public float AnimationDuration => _animationDuration;
        
        public void PickUp()
        {
            DisableInterraction();
            PlayAppearAnimation();
        }

        public void DisableInterraction()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Deliver()
        {
            PlayDisappearAnimation();
            StartCoroutine(DestroyRoutine());
        }

        private void PlayAppearAnimation()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, AnimationDuration).SetEase(Ease.OutBounce);
        }

        private void PlayDisappearAnimation()
        {
            transform.localScale = Vector3.one;
            transform.DOScale(0, AnimationDuration).SetEase(Ease.OutCirc);
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(AnimationDuration);
            Destroy(gameObject);
        }
    }
}
