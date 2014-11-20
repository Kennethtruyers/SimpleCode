namespace SimpleCode.Application.Queries
{
	public interface IQuery { }

	public class All : IQuery { }

	public class ByUserId : IQuery
	{
		public ByUserId(int userId)
		{
			UserId = userId;
		}
		public int UserId { get; private set; }
	}

	public class ById : IQuery
	{
		public int ID { get; private set; }

		public ById(int id)
		{
			ID = id;
		}
	}
}
