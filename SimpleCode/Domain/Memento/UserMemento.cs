using System.Collections.Generic;

namespace SimpleCode.Domain.Memento
{
	public class UserMemento
	{
		public int Id { get; set; }
		public List<int> Friends { get; set; }
	}
}