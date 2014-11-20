using System;
using System.Collections.Generic;
using SimpleCode.Application.Commands;
using SimpleCode.Application.Events;
using SimpleCode.Application.Queries;

namespace SimpleCode
{
	public class Commands
	{
		static readonly Dictionary<Type, Action<ICommand>> _handlers = new Dictionary<Type, Action<ICommand>>();
		public static void Register<T>(Action<T> handler) where T : ICommand
		{
			_handlers.Add(typeof(T), x => handler((T)x));
		}

		public static void Dispatch(ICommand command)
		{
			_handlers[command.GetType()](command);
		}
	}
	public class Events
	{
		static readonly Dictionary<Type, List<Action<IEvent>>> _handlers = new Dictionary<Type, List<Action<IEvent>>>();
		public static void Register<T>(Action<T> handler) where T : IEvent
		{
			if (!_handlers.ContainsKey(typeof(T)))
				_handlers.Add(typeof(T), new List<Action<IEvent>>());

			_handlers[typeof(T)].Add(x => handler((T)x));
		}

		public static void Unregister<T>()
		{
			if (_handlers.ContainsKey(typeof(T)))
				_handlers.Remove(typeof(T));
		}
		public static void Raise(IEvent @event)
		{
			foreach (var eventHandler in _handlers[@event.GetType()])
			{
				eventHandler(@event);
			}
		}
	}
	public class Query
	{
		
		static readonly Dictionary<Tuple<Type, Type>, Func<IQuery, object>> _handlers = new Dictionary<Tuple<Type, Type>, Func<IQuery, object>>();

		public static void Register<TReturnValue, TQuery>(Func<TQuery, TReturnValue> handler) where TQuery : IQuery
		{
			_handlers.Add(Tuple.Create(typeof(TReturnValue), typeof(TQuery)), x => handler((TQuery)x));
		}

		public static TReturnValue Get<TReturnValue>(IQuery query)
		{
			return (TReturnValue)_handlers[Tuple.Create(typeof(TReturnValue), query.GetType())](query);
		}

		public static TReturnValue Get<TReturnValue, TQuery>() where TQuery : IQuery, new()
		{
			return Get<TReturnValue>(new TQuery());
		}
	}
}