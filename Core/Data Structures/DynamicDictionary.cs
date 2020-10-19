using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCGuidebook.Core.DataStructures
{
	public class DynamicDictionary<K, V>
	{
		public DynamicDictionary(Func<K, bool> keyRestriction = null, Func<V, bool> valueRestriction = null, 
			K defaultK = default, V defaultV = default, Func<V, V> rectification = null)
		{
			_KeyRestriction = keyRestriction ?? (k => true);
			_ValueRestriction = valueRestriction ?? (v => true);
			_Rectification = rectification ?? (v => default);
			_DefaultK = defaultK;
			_DefaultV = defaultV;
		}

		private readonly Dictionary<K, V> _Dictionary = new Dictionary<K, V>();
		private readonly Func<K, bool> _KeyRestriction;
		private readonly Func<V, bool> _ValueRestriction;
		private readonly Func<V, V> _Rectification;
		private readonly K _DefaultK;
		private readonly V _DefaultV;

		public void Add(K key, V value)
		{
			if (_KeyRestriction(key) && _ValueRestriction(value))
			{
				_Dictionary.Add(key, value);
			}
		}

		public V this[K key]
		{
			get
			{
				if (_Dictionary.ContainsKey(key))
				{
					return _Dictionary[key];
				}
				else
				{
					return _DefaultV;
				}
			}
			set
			{
				if (_Dictionary.ContainsKey(key))
				{
					if (_ValueRestriction(value))
					{
						_Dictionary[key] = value;
					}
					else
					{
						_Dictionary[key] = _Rectification(value);
					}
				}
			}
		}

		public K this[V value]
		{
			get
			{
				if (_Dictionary.ContainsValue(value))
				{
					return _Dictionary.Keys.Single(k => _Dictionary[k].Equals(value));
				}
				else
				{
					return _DefaultK;
				}
			}
		}
	}
}
