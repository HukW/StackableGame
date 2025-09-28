using System;
using System.Collections.Generic;
using _Project.Scripts.Pickups;
using _Project.Scripts.Processing_Objects.Zones;
using UnityEngine;

namespace _Project.Scripts.Processing_Objects
{
    public class ProcessingObject : MonoBehaviour
    {
        public Action OnItemDelivered;
        public Action OnItemPickedUp;
        
        [SerializeField]
        private ItemTypes _requiredItemType;
        [SerializeField]
        private ItemTypes _producedItemType;
        [SerializeField]
        private GameObject _producedItemPrefab;
        public GameObject ProducedItemPrefab => _producedItemPrefab;
        
        [SerializeField]
        private List<DeliveryZoneBase> _deliveryZones = new List<DeliveryZoneBase>();
        [SerializeField]
        private List<LoadZoneBase> _loadZones = new List<LoadZoneBase>();

        private int _producedItemsCount = 0;
        public int ProducedItemsCount => _producedItemsCount;
        
        private void Start()
        {
            foreach (DeliveryZoneBase deliveryZone in _deliveryZones)
            {
                deliveryZone.OnItemDelivered += DeliverItem;
                deliveryZone.SetItemType(_requiredItemType);
            }

            foreach (LoadZoneBase loadZone in _loadZones)
            {
                loadZone.OnItemPickedUp += ItemPickedUp;
                loadZone.SetItemType(_producedItemType);
                loadZone.SetItemPrefab(_producedItemPrefab);
                loadZone.SetOwningProcessingObject(this);
            }
        }

        private void DeliverItem(ItemTypes itemType)
        {
            _producedItemsCount++;
            OnItemDelivered?.Invoke();
        }

        private void ItemPickedUp(ItemTypes itemType)
        {
            _producedItemsCount--;
            OnItemPickedUp?.Invoke();
        }
    }
}
