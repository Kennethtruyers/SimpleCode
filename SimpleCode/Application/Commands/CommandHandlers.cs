using System;
using SimpleCode.Domain;

namespace SimpleCode.Application.Commands
{
	public class CommandHandlers
	{
		public static readonly Action<CreateUser> CreateUser = createUser =>
			new User(createUser.Name, createUser.LastName);

		public static readonly Action<UpdateProfile, User> UpdateProfile = (updateProfile, user) =>
			user.UpdateProfile(updateProfile.Name, updateProfile.LastName);

		public static readonly Action<AddFriend, User> AddFriend = (addFriend, user) =>
			user.AddFriend(addFriend.FriendId, addFriend.FriendSince);
	}
}