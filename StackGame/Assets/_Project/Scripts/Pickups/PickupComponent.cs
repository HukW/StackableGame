using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

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

        [SerializeField] 
        private float _pushForce = 2f;
        
        
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

        public void PlayAppearAnimation()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, AnimationDuration).SetEase(Ease.OutBounce);
        }

        public void PlayDisappearAnimation()
        {
            transform.localScale = Vector3.one;
            transform.DOScale(0, AnimationDuration).SetEase(Ease.OutCirc);
        }

        private IEnumerator DestroyRoutine()
        {
            yield return new WaitForSeconds(AnimationDuration);
            Destroy(gameObject);
        }

        public void ApplyCollisionReaction(GameObject reactionCauser)
        {
            ApplyPush(reactionCauser.transform);
        }
        private void ApplyPush(Transform otherTransform)
        {
            Vector3 pushDir = (transform.position - otherTransform.position).normalized;
            pushDir.y = 0.2f;
            _rigidbody.AddForce(pushDir * _pushForce, ForceMode.Impulse);
        }
    }
}
