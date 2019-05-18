using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public delegate void OnValueChanged<T>(object sender, T previousValue, T newValue);
	public delegate void OnValueAdded<T>(IEnumerable<T> sender, T addedItem);
	public delegate void OnValueDeleted<T>(IEnumerable<T> sender, T deletedItem);
}
