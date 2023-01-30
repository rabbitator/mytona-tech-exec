public class LoadLevelMessage : Message
{
	public int LevelIndex;

	public LoadLevelMessage(int levelIndex)
	{
		LevelIndex = levelIndex;
	}
}