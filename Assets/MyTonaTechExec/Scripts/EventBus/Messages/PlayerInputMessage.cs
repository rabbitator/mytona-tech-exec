using UnityEngine;

namespace MyTonaTechExec.EventBus.Messages
{
	public class PlayerInputMessage : Message
	{
		public Vector2 MovementDirection;
		public Vector2 AimDirection;
		public bool Fire = false;
	}
}