using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
	public class PlayerInputController : MonoBehaviour
	{
		[Header("Player Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		public void OnMove(InputValue value)
		{
			if (GameManager.Instance.IsPlaying) MoveInput(value.Get<Vector2>());
			else MoveInput(new Vector2(0,0));
		}
		public void MoveInput(Vector2 newMoveDirection) { move = newMoveDirection; }

		public void OnLook(InputValue value)
		{
			if (GameManager.Instance.IsPlaying) LookInput(value.Get<Vector2>());
			else LookInput(new Vector2(0,0));
		}
		public void LookInput(Vector2 newLookDirection) { look = newLookDirection; }

		public void OnJump(InputValue value)
		{
			if (GameManager.Instance.IsPlaying) JumpInput(value.isPressed); 
			else JumpInput(false);
		}
		public void JumpInput(bool newJumpState) { jump = newJumpState; }

		public void OnSprint(InputValue value)
		{
			if (GameManager.Instance.IsPlaying) SprintInput(value.isPressed);
			else SprintInput(false);
		}
		public void SprintInput(bool newSprintState) { sprint = newSprintState; }

		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus && GameManager.Instance.IsPlaying)
			{
				LockCursor();
			}
			else // In menus for example
			{
				UnlockCursor(); 
			}
		}

		private void LockCursor()
		{
			// Why do we change the lock state of the cursor twice ?
			// See the Unity Forum => https://discussions.unity.com/t/cursor-lockstate-not-working/145392
			Cursor.lockState = CursorLockMode.None; 
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void UnlockCursor()
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
