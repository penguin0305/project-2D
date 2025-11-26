using UnityEngine;

namespace SupanthaPaul
{
	public class InputSystem : MonoBehaviour
	{
		// input string caching
		static readonly string HorizontalInput = "Horizontal";
		static readonly string JumpInput = "Jump";
		static readonly string DashInput = "Dash";
		static readonly string PauseInput = "Pause";
		static readonly string UsedInput = "Used";

		public static float HorizontalRaw()
		{
			return Input.GetAxisRaw(HorizontalInput);
		}

		public static bool Jump()
		{
			return Input.GetButtonDown(JumpInput);
		}

		public static bool Dash()
		{
			return Input.GetButtonDown(DashInput);
		}

		public static bool Pause()
		{
			return Input.GetButtonDown(PauseInput);
		}

		public static bool Used()
		{
			return Input.GetButtonDown(UsedInput);
		}
	}
}
