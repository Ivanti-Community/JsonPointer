namespace JsonPointer
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public static class JsonPointer
	{
		#region Public Methods
		/// <summary>
		/// Gets the value at the JSON pointer path.
		/// </summary>
		/// <typeparam name="T">returned Type</typeparam>
		/// <param name="source">The source object, can be JSON string, dynamic object, or POCO</param>
		/// <param name="path">The JSON pointer path. see: https://tools.ietf.org/html/rfc6901 </param>
		/// <returns></returns>
		public static T Get<T>(object source, string path)
		{
			var tokens = path.Split('/').Skip(1).Select(Decode).ToArray();
			return Get<T>(ConvertObjectToToken(source), tokens);
		}

		/// <summary>
		/// Tries to get the value at the JSON pointer path.
		/// </summary>
		/// <typeparam name="T">Result Type</typeparam>
		/// <param name="source">The source object, can be JSON string, dynamic object, or POCO</param>
		/// <param name="path">The JSON pointer path. see: https://tools.ietf.org/html/rfc6901 </param>
		/// <param name="result">The result if path exists.</param>
		/// <returns></returns>
		public static bool TryGet<T>(object source, string path, out T result)
		{
			try
			{
				result = Get<T>(source, path);
				return true;
			}
			catch (ArgumentException)
			{
				result = default(T);
				return false;
			}
		}

		/// <summary>
		/// Sets the value at the JSON pointer path.
		/// </summary>
		/// <typeparam name="T">origin Type</typeparam>
		/// <param name="source">The source object, can be JSON string, dynamic object, or POCO.</param>
		/// <param name="path">The JSON pointer path. see: https://tools.ietf.org/html/rfc6901 </param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T Set<T>(T source, string path, object value) where T : class
		{
			var tokens = path.Split('/').Skip(1).Select(Decode).ToArray();

			var origin = ConvertObjectToToken(source);
			JToken property = Find(origin, tokens, true);
			property.Replace(ConvertObjectToToken(value));

			if (typeof(T) == typeof(string))
			{
				return JsonConvert.SerializeObject(origin) as T;
			}
			return origin.ToObject<T>();
		}
		#endregion

		#region Private Methods
		private static JToken ConvertObjectToToken(object value)
		{
			var sourceString = value as string;
			if (sourceString != null && (sourceString.StartsWith("{") || sourceString.StartsWith("[")))
			{
				return JToken.Parse(sourceString);
			}
			return JToken.FromObject(value);
		}

		private static T Get<T>(JToken source, IReadOnlyCollection<string> tokens)
		{
			var token = Find(source, tokens, false);
			return token.ToObject<T>();
		}

		private static JToken Find(JToken source, IReadOnlyCollection<string> tokens, bool create)
		{
			if (tokens.Count == 0)
			{
				return source;
			}
			try
			{
				var pointer = source;
				foreach (var token in tokens)
				{
					var array = pointer as JArray;
					if (array != null)
					{
						switch (token)
						{
							case "-":
								pointer = JToken.FromObject(default(int));
								array.Add(pointer);
								break;
							case "":
								break;
							default:
								pointer = pointer[Convert.ToInt32(token)];
								break;
						}
					}
					else if (token == "")
					{
						pointer = pointer.Value<JToken>();
					}
					else
					{
						var temp = pointer[token];
						if (temp == null)
						{
							if (create)
							{
								pointer[token] = JToken.FromObject(default(int));
								pointer = pointer[token];
							}
							else
							{
								throw new ArgumentException("Cannot find " + token);
							}
						}
						else
						{
							pointer = temp;
						}
					}
				}
				return pointer;
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Failed to dereference pointer", ex);
			}
		}

		private static string Decode(string token)
		{
			return Uri.UnescapeDataString(token).Replace("~1", "/").Replace("~0", "~");
		}
		#endregion
	}
}
