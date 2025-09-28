using System;
using _Project.Scripts.Pickups;
using UnityEngine;

namespace _Project.Scripts.Processing_Objects.Zones
{
    public class LoadZoneBase : MonoBehaviour
    {
        public Action<PickupComponent> OnItemPickedUp;
    }
}
