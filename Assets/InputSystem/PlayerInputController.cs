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
		[Header("Mouse Cursor Settings")]
		[SerializeField] public bool cursorLocked = true;
		[SerializeField] public bool cursorInputForLook = true;
		[SerializeField] public bool inMenu = true; //new
	
		public void OnMove(InputValue value) { MoveInput(value.Get<Vector2>()); }
		public void MoveInput(Vector2 newMoveDirection) { move = newMoveDirection; } 
	
		public void OnLook(InputValue value) { if(cursorInputForLook) LookInput(value.Get<Vector2>()); }
		public void LookInput(Vector2 newLookDirection) { look = newLookDirection; }

		public void OnJump(InputValue value) { JumpInput(value.isPressed); }
		public void JumpInput(bool newJumpState) { jump = newJumpState; }
	
		public void OnSprint(InputValue value) { SprintInput(value.isPressed); }
		public void SprintInput(bool newSprintState) { sprint = newSprintState; }

		// private void OnApplicationFocus(bool hasFocus)
		// {
		// 	if (hasFocus) 
		// 	{
		// 		SetCursorVisible(inMenu); // new
		// 		SetCursorState(cursorLocked); 
		// 	}
		// }
		
		private void OnApplicationFocus(bool hasFocus)
		{
			if (hasFocus)
			{
				SetCursorVisible(inMenu); // new
				if (inMenu)
				{
					SetCursorState(false); // Déverrouillez le curseur si vous êtes dans le menu
				}
				else
				{
					SetCursorState(cursorLocked); // Verrouillez le curseur uniquement si vous n'êtes pas dans le menu
				}
			}
			else
			{
				SetCursorState(false); // Déverrouillez le curseur lorsque vous perdez le focus sur la fenêtre
			}
		}
		
		private void SetCursorState(bool newState) { Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None; }
		public void SetCursorVisible(bool visible) { Cursor.visible = visible; } // new
	}
}
