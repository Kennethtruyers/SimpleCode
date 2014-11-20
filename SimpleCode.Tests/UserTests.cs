using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SimpleCode.Application.Events;
using SimpleCode.Domain;
using SimpleCode.Domain.Memento;

namespace SimpleCode.Tests
{
	[Subject(typeof(User))]
	class When_updating_a_profile : WithSubject<User>
	{
		It updates_the_profile = () =>
			ConfigForAnEventBus<ProfileUpdated>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			Subject.UpdateProfile("first", "last");

		Establish context = () =>
		{
			Subject = User.FromMemento(new UserMemento { Id = 1 });
			With<ConfigForAnEventBus<ProfileUpdated>>();
		};

		static ProfileUpdated expected = new ProfileUpdated(1, "first", "last");
	}

	[Subject(typeof(User))]
	class When_creating_a_user : WithSubject<User>
	{
		It creates_the_user = () =>
			ConfigForAnEventBus<UserCreated>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			new User("first", "last");

		Establish context = () => 
			With<ConfigForAnEventBus<UserCreated>>();

		static UserCreated expected = new UserCreated("first", "last");
	}

	[Subject(typeof(User))]
	class When_adding_a_friend_to_a_user_which_has_less_than_10_friends : WithSubject<User>
	{
		It adds_the_friend = () =>
			ConfigForAnEventBus<FriendAdded>.LastEvent.ShouldBeLike(expected);

		Because of = () =>
			Subject.AddFriend(expected.FriendId, expected.FriendsSince);

		Establish context = () =>
		{
			Subject = User.FromMemento(new UserMemento { Id = 1, Friends = new List<int> {1,2,3,4} });
			With<ConfigForAnEventBus<FriendAdded>>();
		};

		static FriendAdded expected = new FriendAdded(1, 5, DateTime.Now);
	}

	[Subject(typeof(User))]
	class When_adding_a_friend_to_a_user_which_has_10_friends : WithSubject<User>
	{
		It does_not_add_the_friend = () =>
			ConfigForAnEventBus<FriendAdded>.LastEvent.ShouldBeNull();

		It throws_an_invalid_operation_exception = () =>
			exception.ShouldBeOfExactType<InvalidOperationException>();

		Because of = () =>
			exception = Catch.Exception(() => Subject.AddFriend(1, DateTime.Now));

		Establish context = () =>
		{
			Subject = User.FromMemento(new UserMemento { Id = 1, Friends = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } });
			With<ConfigForAnEventBus<FriendAdded>>();
		};

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
