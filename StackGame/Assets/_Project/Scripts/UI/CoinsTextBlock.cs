using System;
using System.Collections.Generic;
using _Project.Scripts.Pickups;
using _Project.Scripts.Processing_Objects.Zones;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class CoinsTextBlock : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textBlock;

        [SerializeField] private float _coinsPerItem = 10;
        private float _currentCoins = 0;
        
        [Space(5)]
        [SerializeField]
        List<DeliveryZoneBase> _bindedDeliveryZones = new List<DeliveryZoneBase>();

        private void Start()
        {
            foreach (var deliveryZone in _bindedDeliveryZones)
            {
                deliveryZone.OnItemDelivered += OnItemDelivered;
            }
            
            _textBlock.text = _currentCoins.ToString();
        }

        private void OnItemDelivered(ItemTypes ItemType)
        {
            _currentCoins += _coinsPerItem;
            _textBlock.text = _currentCoins.ToString();
        }
    }
}
