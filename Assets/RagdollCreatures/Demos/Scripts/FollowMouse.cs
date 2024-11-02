using UnityEngine;
using UnityEngine.InputSystem;

namespace RagdollCreatures
{
	/// <summary>
	/// Simple script to let GameObject follow the mouse position.
	/// Uses the new Input system.
	/// </summary>
	public class FollowMouse : MonoBehaviour, IInputSystem
	{
		#region Input System
		[Header("Input System")]
		public bool useNewInputSystem;
		#endregion

		#region Internal
		private Vector2 position;
		#endregion

		void Awake()
		{
			useNewInputSystem = InputSystemSwitcher.UseNewInputSystem;
		}

		void Update()
		{
			Vector2 newPosition = GetMouseInput();
		
			transform.position = Camera.main.ScreenToWorldPoint(newPosition);
		}

		private Vector2 GetMouseInput()
		{
			Vector2 newPosition;
			if (useNewInputSystem)
			{
				
				newPosition = position;
				//Debug.Log(newPosition);
			}
			else
			{
				newPosition = Input.mousePosition;
			}

			return newPosition;
		}

		public void OnMouseMove(InputAction.CallbackContext context)
		{
			// Debug.Log("OnMouseMove");
			position = context.ReadValue<Vector2>();
			// Debug.Log(position);
		}

		public bool UseNewInputSystem()
		{
			return useNewInputSystem;
		}

		public void SetUseNewInputSystem(bool useNewInputSystem)
		{
			this.useNewInputSystem = useNewInputSystem;
		}
	}
}
