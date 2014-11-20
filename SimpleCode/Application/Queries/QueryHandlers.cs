using System;
using System.Collections.Generic;
using System.Linq;
using PetaPoco;
using SimpleCode.Domain.Memento;
using SimpleCode.ReadModel;

namespace SimpleCode.Application.Queries
{
	public class QueryHandlers
	{
		public static readonly Func<Database, List<UserProfile>> GetAllProfiles = database =>
			database.Query<UserProfile>(Sql.Builder.Select("id", "firstname", "lastname")
													.From("users"))
					.ToList();

		public static readonly Func<int, Database, List<Friend>> GetFriends = (userId, database) =>
			database.Query<Friend>(Sql.Builder.Select("friends.firstname", "friends.lastname", "users_friends.friendssince")
											  .From("users")
											  .InnerJoin("users_friends")
											  .On("users_friends.userid = users.id")
											  .InnerJoin("users friends")
											  .On("friends.id = users.friends.friendid")
											  .Where("users.id = @userid", new {userId}))
					.ToList();

		public static readonly Func<int, Database, UserMemento> GetuserMemento = (id, database) =>
			new UserMemento
			{
				Id = id,
				Friends = database.Query<int>(Sql.Builder.Select("friends.id")
														 .From("users")
														 .InnerJoin("users_friends")
														 .On("users_friends.userid = users.id")
														 .InnerJoin("users friends")
														 .On("friends.id = users.friends.friendid")
														 .Where("users.id = @id", new { id }))
								 .ToList()
			};
	}
}