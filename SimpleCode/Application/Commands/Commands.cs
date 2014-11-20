using System;

namespace SimpleCode.Application.Commands
{
	public interface ICommand { }

	public class CreateUser : ICommand
	{
		public CreateUser(string name, string lastName)
		{
			Name = name;
			LastName = lastName;
		}
		public string Name { get; private set; }
		public string LastName { get; private set; }
	}

	public class UpdateProfile : ICommand
	{
		public UpdateProfile(int id, string name, string lastName)
		{
			ID = id;
			Name = name;
			LastName = lastName;
		}
		public int ID { get; private set; }
		public string Name { get; private set; }
		public string LastName { get; private set; }
	}

	public class AddFriend : ICommand
	{
		public AddFriend(int id, int friendId, DateTime friendSince)
		{
			ID = id;
			FriendId = friendId;
			FriendSince = friendSince;
		}

		public int ID { get; private set; }
		public int FriendId { get; private set; }
		public DateTime FriendSince { get; private set; }
	}
}
