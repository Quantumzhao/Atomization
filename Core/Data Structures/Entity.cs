using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCGuidebook.Core.DataStructures
{
	public sealed class Entity
	{
		private List<Property> _Properties = new List<Property>();
		public void AddProperty(string name, object data, object tag = null)
		{
			_Properties.Add(new Property(name, data, tag));
		}

		public object this[string name]
		{
			get => _Properties.SingleOrDefault(p => p.Name == name).Data;
			set
			{
				var p = _Properties.SingleOrDefault(p => p.Name == name);
				p.Data = value;
			}
		}
		public object this[object tag]
		{
			get => _Properties.SingleOrDefault(p => p.Tag == tag).Data;
			set
			{
				var p = _Properties.SingleOrDefault(p => p.Tag == tag);
				p.Data = value;
			}
		}

		public T Get<T>(string name) => (T)this[name];
		public T Get<T>(object tag) => (T)this[tag];
	}

	public class Property
	{
		public Property(string name, object data, object tag)
		{
			Name = name;
			Data = data;
			Tag = tag;
		}

		public string Name { get; set; }
		public object Tag { get; set; }
		public object Data { get; set; }
	}
}
