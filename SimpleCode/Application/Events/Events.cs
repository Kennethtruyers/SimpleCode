using System;

namespace SimpleCode.Application.Events
{
	public interface IEvent { }

	public class ProfileUpdated : IEvent
	{
		public ProfileUpdated(int id, string firstName, string lastName)
		{
			ID = id;
			FirstName = firstName;
			LastName = lastName;
		}

		public int ID { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
	}

	public class UserCreated : IEvent
	{
		public UserCreated(string firstName, string lastName)
		{
			FirstName = firstName;
			LastName = lastName;
		}
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
	}

	public class FriendAdded : IEvent
	{
		public FriendAdded(int userId, int friendId, DateTime friendsSince)
		{
			UserId = userId;
			FriendId = friendId;
			FriendsSince = friendsSince;
		}
		public int UserId { get; private set; }
		public int FriendId { get; private set; }
		public DateTime FriendsSince { get; private set; }
	}

}
