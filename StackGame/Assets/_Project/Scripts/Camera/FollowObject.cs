using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Camera
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        Vector3 _cameraOffset = Vector3.zero;
        
        void LateUpdate () 
        {
            transform.position = _target.position + _cameraOffset;
        }
    }
    
}
