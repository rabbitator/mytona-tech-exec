namespace MyTonaTechExec.EventBus.Messages
{
	public class SpawnMobMessage : Message
	{
		public const int MELEE = 0;
		public const int RANGE = 1;
	
		public int Type;
	}
}