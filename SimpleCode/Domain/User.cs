using System;
using System.Collections.Generic;
using SimpleCode.Application.Events;
using SimpleCode.Domain.Memento;

namespace SimpleCode.Domain
{
	public class User
	{
		int _id;
		List<int> _friends;

		User() { }
		public User(string firstName, string lastName)
		{
			_friends = new List<int>();
			Events.Raise(new UserCreated(firstName, lastName));
		}

		public void UpdateProfile(string firstName, string lastName)
		{
			Events.Raise(new ProfileUpdated(_id, firstName, lastName));
		}

		public void AddFriend(int friendId, DateTime friendSince)
		{
			if (_friends.Count >= 10)
				throw new InvalidOperationException("Cannot add more than 10 friends");

			_friends.Add(friendId);
			Events.Raise(new FriendAdded(_id, friendId, friendSince));
		}

		public static User FromMemento(UserMemento memento)
		{
			return new User
			{
				_friends = memento.Friends,
				_id = memento.Id
			};
		}
	}
}