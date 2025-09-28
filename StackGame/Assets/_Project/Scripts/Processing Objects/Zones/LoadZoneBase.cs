using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Pickups;
using UnityEngine;

namespace _Project.Scripts.Processing_Objects.Zones
{
    public class LoadZoneBase : MonoBehaviour
    {
        public Action<ItemTypes> OnItemPickedUp;
        
        private ItemTypes _itemType;
        private PlayerItemsComponent _playerItemsComponent;
        private GameObject _prefab;
        private ProcessingObject _owningProcessingObject;
        
        [SerializeField] 
        [Min(0)]
        private float _delayBetweenActions = .2f;
        private float _currentDelay = 0f;

        private void Start()
        {
            _playerItemsComponent = FindObjectOfType<PlayerItemsComponent>();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_currentDelay <= 0)
                {
                    _currentDelay = _delayBetweenActions;

                    if (_owningProcessingObject.ProducedItemsCount >= 1)
                    {
                        if (_playerItemsComponent.TryAddItem(_prefab))
                        {
                            OnItemPickedUp?.Invoke(_itemType);
                        }
                    }
                }
            }
        }
        
        private void Update()
        {
            _currentDelay -= Time.deltaTime;
        }

        public void SetItemType(ItemTypes itemType)
        {
            _itemType = itemType;
        }

        public void SetItemPrefab(GameObject prefab)
        {
            _prefab = prefab;   
        }

        public void SetOwningProcessingObject(ProcessingObject processingObject)
        {
            _owningProcessingObject = processingObject;
        }
    }
}
