using System;
using System.Collections.Generic;
using _Project.Scripts.Pickups;
using _Project.Scripts.Processing_Objects.Zones;
using UnityEngine;

namespace _Project.Scripts.Processing_Objects
{
    public class ProcessingObject : MonoBehaviour
    {
        [SerializeField]
        private ItemTypes _requiredItemType;
        [SerializeField]
        private ItemTypes _producedItemType;
        
        [SerializeField]
        private List<DeliveryZoneBase> _deliveryZones = new List<DeliveryZoneBase>();
        [SerializeField]
        private List<LoadZoneBase> _loadZones = new List<LoadZoneBase>();

        private void Start()
        {
            foreach (DeliveryZoneBase deliveryZone in _deliveryZones)
            {
                deliveryZone.OnItemDelivered += DeliverItem;
                deliveryZone.SetItemType(_requiredItemType);
            }

            foreach (LoadZoneBase loadZone in _loadZones)
            {
                loadZone.OnItemPickedUp += OnItemPickedUp;
            }
        }

        private void DeliverItem(ItemTypes itemType)
        {
            
        }

        private void OnItemPickedUp(PickupComponent item)
        {
            
        }
    }
}
