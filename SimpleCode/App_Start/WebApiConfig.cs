using System;
using System.Collections.Generic;
using System.Web.Http;
using PetaPoco;
using SimpleCode.Application.Commands;
using SimpleCode.Application.Events;
using SimpleCode.Application.Queries;
using SimpleCode.Domain;
using SimpleCode.Domain.Memento;
using SimpleCode.Infrastructure.Logging;
using SimpleCode.ReadModel;

namespace SimpleCode
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API routes
			config.MapHttpAttributeRoutes();
			bootstrap();
		}

		static void bootstrap()
		{
			var getUser = new Func<int, User>(id => User.FromMemento(Query.Get<UserMemento>(new ById(id))));

			// Commands
			Commands.Register<CreateUser>(createUser => 
				LogHandlers<CreateUser>.Log(createUser, 
					() => CommandHandlers.CreateUser(createUser)));

			Commands.Register<UpdateProfile>(updateProfile =>
				LogHandlers<UpdateProfile>.Log(updateProfile,
					() => CommandHandlers.UpdateProfile(updateProfile, getUser(updateProfile.ID))));

			Commands.Register<AddFriend>(addFriend =>
				LogHandlers<AddFriend>.Log(addFriend,
					() => CommandHandlers.AddFriend(addFriend, getUser(addFriend.ID))));


			// Queries
			Query.Register<List<UserProfile>, All>(all =>
				wrapWithDatabase(db => QueryHandlers.GetAllProfiles(db)));

			Query.Register<List<Friend>, ByUserId>(byUserId =>
				wrapWithDatabase(db => QueryHandlers.GetFriends(byUserId.UserId, db)));

			Query.Register<UserMemento, ById>(byId =>
				wrapWithDatabase(db => QueryHandlers.GetuserMemento(byId.ID, db)));

			// Events
			Events.Register<ProfileUpdated>(profileUpdated => 
				wrapWithDatabase(db => EventHandlers.ProfileUpdated(profileUpdated, db)));

			Events.Register<UserCreated>(userCreated =>
				wrapWithDatabase(db => EventHandlers.UserCreated(userCreated, db)));

			Events.Register<FriendAdded>(friendAdded =>
				wrapWithDatabase(db => EventHandlers.FriendAdded(friendAdded, db)));


		}

		static TReturnType wrapWithDatabase<TReturnType>(Func<Database, TReturnType> handler)
		{
			using (var db = new Database("connectionString"))
				return handler(db);
		}

		static void wrapWithDatabase(Action<Database> handler)
		{
			using (var db = new Database("connectionString"))
				handler(db);
		}
	}
}
