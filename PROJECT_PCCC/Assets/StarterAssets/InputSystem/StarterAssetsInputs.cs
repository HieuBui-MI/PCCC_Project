using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool sprint;
		public bool jump;
		public bool leftClick = false;
		public bool rightClick = false;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Other Settings")]
		public bool toolWheel = false;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnLeftClick(InputValue value)
		{
			leftClick = value.isPressed;
		}

		public void OnRightClick(InputValue value)
		{
			rightClick = value.isPressed;
		}

		public void OnToolWheel(InputValue value)
		{
			toolWheel = value.isPressed;
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

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void LeftClickInput(bool newLeftClickState)
		{
			leftClick = newLeftClickState;
		}

		public void RightClickInput(bool newRightClickState)
		{
			rightClick = newRightClickState;
		}

		public void ToolWheelInput(bool newToolWheelState)
		{
			toolWheel = newToolWheelState;
		}
	}

}