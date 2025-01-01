using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace DATN
{
	public class MyControllInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool startAttack;
		public bool confirmAttack;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputAction.CallbackContext context)
		{
			
			MoveInput(context.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			if(cursorInputForLook)
			{
				LookInput(context.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			JumpInput(context.ReadValue<float>()>0);
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			SprintInput(context.ReadValue<float>()>0);
		}
		public void OnAttack(InputAction.CallbackContext context){
			// AttackInput(value.isPressed);
			if(context.started){
				// Debug.Log("Down");
				StartAttackInput(true);
			}
			if(context.canceled){
				// Debug.Log("Up");
				ConfirmAttackInput(true);
			}
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void StartAttackInput(bool newAttackState)
		{
			startAttack = newAttackState;
		}
		public void ConfirmAttackInput(bool newAttackState){
			confirmAttack = newAttackState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}