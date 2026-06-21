using Entities;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : EntityMovement
    {
        private static readonly int IsMovingAnim = Animator.StringToHash("isMoving");
        private InputSystem_Actions _input;
        private SpriteRenderer _renderer;
        private bool _isMoving;
        [SerializeField] private Animator animator;

        protected override void Flip()
        {
            if (MoveInput.x != 0)
            {
                _renderer.flipX = MoveInput.x < 0;
            }
        }

        protected override void AwakeInitialize()
        {
            _input = new InputSystem_Actions();
            _renderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled += OnCancelMoving;
        }

        private void OnDisable()
        {
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled -= OnCancelMoving;
            _input.Disable();
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            animator.SetBool(IsMovingAnim, true);
            IsMoving = true;
        }

        private void OnCancelMoving(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            animator.SetBool(IsMovingAnim, false);
            IsMoving = false;
        }
        
    }
}