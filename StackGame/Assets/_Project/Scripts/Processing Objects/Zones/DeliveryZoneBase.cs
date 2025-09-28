using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Pickups;
using UnityEngine;

namespace _Project.Scripts.Processing_Objects.Zones
{
    [RequireComponent(typeof(Collider))]
    public class DeliveryZoneBase : MonoBehaviour
    {
        public Action<ItemTypes> OnItemDelivered;

        private ItemTypes _itemType;
        private PlayerItemsComponent _playerItemsComponent;
        
        [SerializeField] 
        [Min(0)]
        private float _delayBetweenActions = .2f;
        private float _currentDelay = 0f;

        private void Start()
        {
            _playerItemsComponent = FindObjectOfType<PlayerItemsComponent>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_currentDelay <= 0)
                {
                    _currentDelay = _delayBetweenActions;

                    if (_playerItemsComponent.TryDeliverItem(_itemType))
                    {
                        OnItemDelivered?.Invoke(_itemType);
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
    }
}
