using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Pickups;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class PlayerItemsComponent : MonoBehaviour
    {
        private const string ItemTag = "Item";
        
        private const string MaskAnimationPropertyName = "ForwardMask";
        private int _MaskAnimationPropertyRef;
        
        private Stack<GameObject> _forwardItems = new Stack<GameObject>();
        private Stack<GameObject> _backwardItems = new Stack<GameObject>();
        
        [SerializeField]
        private Animator _animator;

        [SerializeField] 
        [Min(0)]
        [Tooltip("Use 0 to remove stack count constraint")]
        private int _MaxItemsPerStack = 20;
        [SerializeField]
        private float _nextItemYOffset = .5f;
        
        [Space(5)]
        [SerializeField] 
        private Transform _forwardItemSocket; 
        [SerializeField]
        private ItemTypes _forwardItemType;
        [SerializeField]
        private Transform _backwardItemSocket;
        [SerializeField]
        private ItemTypes _backwardItemType;


        private void Start()
        {
            _MaskAnimationPropertyRef = Animator.StringToHash(MaskAnimationPropertyName);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(ItemTag))
            {
                bool pickedUp = TryPickupItem(other.gameObject);
                if (!pickedUp)
                {
                    PickupComponent pickupComponent = other.gameObject.GetComponent<PickupComponent>();
                    if(!pickupComponent) return;
                    
                    pickupComponent.ApplyCollisionReaction(gameObject);
                }
            }
        }


        private bool TryPickupItem(GameObject item)
        {
            PickupComponent pickupComponent = item.GetComponent<PickupComponent>();
            if(!pickupComponent) return false;

            if (pickupComponent.PickupType == _forwardItemType)
            {
                if (_forwardItems.Count < _MaxItemsPerStack)
                {
                    _forwardItems.Push(item);
                    SetItemPositionAndParent(pickupComponent, _forwardItemSocket, _forwardItems);
                    
                    _animator.SetBool(_MaskAnimationPropertyRef, true);
                    
                    pickupComponent.PickUp();
                    
                    return true;
                }
            }
            else
            {
                if (_forwardItems.Count < _MaxItemsPerStack)
                {
                    _backwardItems.Push(item);
                    SetItemPositionAndParent(pickupComponent, _backwardItemSocket, _backwardItems);
                    
                    pickupComponent.PickUp();
                    
                    return true;
                }
            }
            return false;
        }

        private void SetItemPositionAndParent(PickupComponent item, Transform parent, Stack<GameObject> itemStack)
        {
            item.transform.SetParent(parent);
            
            item.transform.localPosition = new Vector3(0, _nextItemYOffset * (itemStack.Count - 1), 0);
            item.transform.localRotation = Quaternion.identity;
        }

        public bool TryDeliverItem(ItemTypes itemType)
        {
            if (itemType == _forwardItemType && _forwardItems.Count > 0)
            {   
                GameObject item = _forwardItems.Pop();
                PickupComponent pickupComponent = item.GetComponent<PickupComponent>();
                pickupComponent.Deliver();
                
                if (_forwardItems.Count <= 0)
                {
                    _animator.SetBool(_MaskAnimationPropertyRef, false);
                }
                return true;
            }
            if (itemType == _backwardItemType && _backwardItems.Count > 0)
            {   
                GameObject item = _backwardItems.Pop();
                PickupComponent pickupComponent = item.GetComponent<PickupComponent>();
                pickupComponent.Deliver();
                
                return true;
            }

            return false;
        }

        public bool TryAddItem(GameObject prefab)
        {
            PickupComponent pickupComponent = prefab.GetComponent<PickupComponent>();
            if (pickupComponent.PickupType == _forwardItemType)
            {
                if (_forwardItems.Count < _MaxItemsPerStack || _MaxItemsPerStack == 0)
                {
                    GameObject item = Instantiate(prefab);
                    
                    TryPickupItem(item);
                    
                    return true;
                }
            }
            if (pickupComponent.PickupType == _backwardItemType)
            {   
                if (_backwardItems.Count < _MaxItemsPerStack || _MaxItemsPerStack == 0)
                {
                    GameObject item = Instantiate(prefab);
                    
                    TryPickupItem(item);
                    
                    return true;
                }
            }
            return false;
        }
    }
}
