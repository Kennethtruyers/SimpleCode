using System;
using SimpleCode.Application.Commands;
using SimpleCode.Domain;

namespace SimpleCode.Infrastructure.Logging
{
	public class LogHandlers<T> where T : ICommand
	{
		public static readonly Action<T, Action> Log = (item, next) =>
		{
			// write log here
			next();
		};
	}
}