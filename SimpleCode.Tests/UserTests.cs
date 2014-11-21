using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SimpleCode.Application.Commands;
using SimpleCode.Application.Events;
using SimpleCode.Domain;
using SimpleCode.Domain.Memento;

namespace SimpleCode.Tests
{
	[Subject(typeof(User))]
	class When_updating_a_profile : WithFakes
	{
		It updates_the_profile = () =>
			ConfigForAnEventBus<ProfileUpdated>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			CommandHandlers.UpdateProfile(new UpdateProfile(1, expected.FirstName, expected.LastName), user);

		Establish context = () =>
			With<ConfigForAnEventBus<ProfileUpdated>>();

		static User user = User.FromMemento(new UserMemento { Id = 1 });
		static ProfileUpdated expected = new ProfileUpdated(1, "first", "last");
	}

	[Subject(typeof(User))]
	class When_creating_a_user : WithFakes
	{
		It creates_the_user = () =>
			ConfigForAnEventBus<UserCreated>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			CommandHandlers.CreateUser(new CreateUser(expected.FirstName, expected.LastName));

		Establish context = () =>
			With<ConfigForAnEventBus<UserCreated>>();

		static UserCreated expected = new UserCreated("first", "last");
	}

	[Subject(typeof(User))]
	class When_adding_a_friend_to_a_user_which_has_less_than_10_friends : WithFakes
	{
		It adds_the_friend = () =>
			ConfigForAnEventBus<FriendAdded>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			CommandHandlers.AddFriend(command, user);

		Establish context = () =>
			With<ConfigForAnEventBus<FriendAdded>>();

		static User user = User.FromMemento(new UserMemento { Id = 1, Friends = new List<int> { 1, 2, 3, 4 } });
		static AddFriend command = new AddFriend(1, 5, DateTime.Now);
		static FriendAdded expected = new FriendAdded(command.ID, command.FriendId, command.FriendSince);
	}

	[Subject(typeof(User))]
	class When_adding_a_friend_to_a_user_which_has_10_friends : WithFakes
	{
		It does_not_add_the_friend = () =>
			ConfigForAnEventBus<FriendAdded>.LastEvent.ShouldBeNull();

		It throws_an_invalid_operation_exception = () =>
			exception.ShouldBeOfExactType<InvalidOperationException>();

		Because of = () =>
			exception = Catch.Exception(() => CommandHandlers.AddFriend(command, user));

		Establish context = () =>
			With<ConfigForAnEventBus<FriendAdded>>();

		static User user = User.FromMemento(new UserMemento { Id = 1, Friends = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } });
		static AddFriend command = new AddFriend(1, 5, DateTime.Now);
		static FriendAdded expected = new FriendAdded(command.ID, command.FriendId, command.FriendSince);
		static Exception exception;
	}

	class ConfigForAnEventBus<T> where T : IEvent
	{
		OnEstablish context = ctx =>
			Events.Register<T>(@event => LastEvent = @event);

		OnCleanup stuff = ctx =>
		{
			LastEvent = default(T);
			Events.Unregister<T>();
		};

		public static T LastEvent;
	}
}
