using System.Collections.Generic;
using _Project.Scripts.Pickups;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Processing_Objects.Zones
{
    [RequireComponent(typeof(Collider))]
    public class WarehouseLoadZone : LoadZoneBase
    {
        private Stack<GameObject> _itemsStack = new Stack<GameObject>();

        [SerializeField]
        private Transform _itemsStartPoint;

        [SerializeField] 
        [Min(1)]
        private int _horizontalElements = 3;
        [SerializeField]
        [Min(1)]
        private int _verticalElements = 3;

        [SerializeField]
        [Min(0)] 
        private float _objectOffset;
        [SerializeField]
        [Min(0)] 
        private float _verticalObjectOffset;

        private void OnItemDelivered()
        {
            GameObject item = Instantiate(_prefab, _itemsStartPoint); 
            _itemsStack.Push(item);

            int index = _itemsStack.Count - 1;
            
            int x = index % _horizontalElements;                         
            int z = (index / _horizontalElements) % _verticalElements;      
            int y = index / (_horizontalElements * _verticalElements);       

            item.transform.localPosition = new Vector3(
                x * _objectOffset,   
                y * _verticalObjectOffset,     
                z * _objectOffset         
            );
            
            PickupComponent pickupComponent = item.GetComponent<PickupComponent>();
            pickupComponent.DisableInterraction();
            pickupComponent.PlayAppearAnimation();
        }

        public override void SetOwningProcessingObject(ProcessingObject processingObject)
        {
            base.SetOwningProcessingObject(processingObject);
            
            _owningProcessingObject.OnItemDelivered += OnItemDelivered;
            _owningProcessingObject.OnItemPickedUp += OnItemPickedFromProcessingObject;
        }

        private void OnItemPickedFromProcessingObject()
        {
            PickupComponent pickupComponent = _itemsStack.Pop().GetComponent<PickupComponent>();
            pickupComponent.PlayDisappearAnimation();
        }
    }
}
