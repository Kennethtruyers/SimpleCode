using System;
using PetaPoco;

namespace SimpleCode.Application.Events
{
	public class EventHandlers
	{
		public static readonly Action<ProfileUpdated, Database> ProfileUpdated = (@event, database) =>
			database.Execute(Sql.Builder.Append("update table users set firstname = @firstname, lastname = @lastname where id = @id", new { @event.FirstName, @event.LastName, @event.ID }));

		public static readonly Action<UserCreated, Database> UserCreated = (@event, database) =>
			database.Execute(Sql.Builder.Append("insert into users (firstname, lastname) values (@firstname, @lastname)", new { @event.FirstName, @event.LastName }));

		public static readonly Action<FriendAdded, Database> FriendAdded = (@event, database) =>
			database.Execute(Sql.Builder.Append("insert into users_friends (userid, friendid, friendssince) values (@userid, @friendid, @friendssince)", new { @event.UserId, @event.FriendId, @event.FriendsSince }));
	}
}