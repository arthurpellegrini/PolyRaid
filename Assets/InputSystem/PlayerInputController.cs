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
		public bool shoot;
		public bool reload;

		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}
		public void MoveInput(Vector2 newMoveDirection) { move = newMoveDirection; }

		public void OnLook(InputValue value)
		{
			LookInput(value.Get<Vector2>());
		}
		public void LookInput(Vector2 newLookDirection) { look = newLookDirection; }

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed); 
		}
		public void JumpInput(bool newJumpState) { jump = newJumpState; }

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void SprintInput(bool newSprintState) { sprint = newSprintState; }

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void ShootInput(bool newShootState) { shoot = newShootState; }
		
		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
		public void ReloadInput(bool newReloadState) { reload = newReloadState; }
		
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
