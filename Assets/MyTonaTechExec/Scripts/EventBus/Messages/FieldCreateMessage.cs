namespace MyTonaTechExec.EventBus.Messages
{
	public class FieldCreateMessage : Message
	{
		public bool[,] Field;
	}
}