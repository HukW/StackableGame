using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterController : MonoBehaviour
    {
        private const string WalkingAnimationPropertyName = "Walking";
        private int WalkingAnimationPropertyRef;
        
        private CharacterController _characterController;
        [SerializeField]
        private Animator _animator;
        
        [SerializeField]
        private float _characterSpeed = 3f;
        [SerializeField]
        private float _rotationSpeed = 8f;
        
        
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            WalkingAnimationPropertyRef = Animator.StringToHash(WalkingAnimationPropertyName);
        }

        private void FixedUpdate()
        {
            float verticalMovement = Input.GetAxis("Vertical");
            float horizontalMovement = Input.GetAxis("Horizontal");
            
            Move(verticalMovement);
            Rotate(horizontalMovement);
            
            HandleAnimation(verticalMovement, horizontalMovement);
        }

        private void Move(float verticalMovement)
        {
            Vector3 movement = new Vector3(0f, -1f, verticalMovement);
            movement *= _characterSpeed * Time.deltaTime;
            
            _characterController.Move(transform.TransformDirection(movement));
        }

        private void Rotate(float horizontalMovement)
        {
            float rotation = horizontalMovement * _rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotation, 0f);
        }

        private void HandleAnimation(float verticalMovement, float horizontalMovement)
        {
            bool isWalking = verticalMovement != 0;    
            _animator.SetBool(WalkingAnimationPropertyRef, isWalking);
        } 
    }
}
