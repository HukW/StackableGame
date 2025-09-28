using System;
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
        
        private Queue<GameObject> _forwardItems = new Queue<GameObject>();
        private Queue<GameObject> _backwardItems = new Queue<GameObject>();
        
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
                TryPickupItem(other.gameObject);
            }
        }


        private void TryPickupItem(GameObject item)
        {
            PickupComponent pickupComponent = item.GetComponent<PickupComponent>();
            if(!pickupComponent) return;

            if (pickupComponent.PickupType == _forwardItemType)
            {
                if (_forwardItems.Count < _MaxItemsPerStack)
                {
                    _forwardItems.Enqueue(item);
                    SetItemPositionAndParent(pickupComponent, _forwardItemSocket, _forwardItems);
                    
                    _animator.SetBool(_MaskAnimationPropertyRef, true);
                    
                    pickupComponent.PickUp();
                }
            }
            else
            {
                if (_forwardItems.Count < _MaxItemsPerStack)
                {
                    _backwardItems.Enqueue(item);
                    SetItemPositionAndParent(pickupComponent, _backwardItemSocket, _backwardItems);
                    
                    pickupComponent.PickUp();
                }
            }
        }

        private void SetItemPositionAndParent(PickupComponent item, Transform parent, Queue<GameObject> itemQueue)
        {
            item.transform.rotation = Quaternion.identity;
            
            item.transform.SetParent(parent);
            
            item.transform.localPosition = new Vector3(0, _nextItemYOffset * (itemQueue.Count - 1), 0);
        }

        public bool TryDeliverItem(ItemTypes itemType)
        {
            if (itemType == _forwardItemType && _forwardItems.Count > 0)
            {   
                GameObject item = _forwardItems.Dequeue();
                Destroy(item);
                RecalculateItemsPosition(_forwardItems);
                
                if (_forwardItems.Count <= 0)
                {
                    _animator.SetBool(_MaskAnimationPropertyRef, false);
                }
                return true;
            }
            if (itemType == _backwardItemType && _backwardItems.Count > 0)
            {   
                GameObject item = _backwardItems.Dequeue();
                Destroy(item);
                RecalculateItemsPosition(_backwardItems);
                
                return true;
            }

            return false;
        }

        private void RecalculateItemsPosition(Queue<GameObject> itemQueue)
        {
            GameObject[] queueArray = itemQueue.ToArray();
            for (int i = 0; i < itemQueue.Count; i++)
            {
                queueArray[i].transform.localPosition = new Vector3(0, _nextItemYOffset * i, 0);
            }
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
