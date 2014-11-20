using System;
using System.Collections.Generic;
using System.Web.Http;
using SimpleCode.Application.Commands;
using SimpleCode.Application.Queries;
using SimpleCode.ReadModel;

namespace SimpleCode.Api
{
	public class UserController : ApiController
	{
		[HttpGet]
		[Route("users/profiles")]
		public List<UserProfile> GetAllProfiles()
		{
			return Query.Get<List<UserProfile>, All>();
		}

		[HttpGet]
		[Route("users/{int:userid}/friends")]
		public List<Friend> GetFriendsForUser(int userId)
		{
			return Query.Get<List<Friend>>(new ByUserId(userId));
		}

		[HttpPost]
		[Route("users")]
		public void CreateUser(string name, string lastName)
		{
			Commands.Dispatch(new CreateUser(name, lastName));
		}

		[HttpPut]
		[Route("users/{int:id}/profiles")]
		public void UpdateProfile(int id, string name, string lastName)
		{
			Commands.Dispatch(new UpdateProfile(id, name, lastName));
		}

		[HttpPost]
		[Route("users/{int:id}/friends")]
		public void AddFriend(int id, int friendId, DateTime friendSince)
		{
			Commands.Dispatch(new AddFriend(id, friendId, friendSince));
		}
	}
}