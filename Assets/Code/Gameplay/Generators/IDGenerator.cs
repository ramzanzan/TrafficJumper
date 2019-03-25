public class IDGenerator
{
	private int _id = 0;

	public int NextID()
	{
		return _id++;
	}

	public void Reset(int init = 0)
	{
		_id = init;
	}
}
