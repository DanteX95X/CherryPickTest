using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
	class Map
	{
		#region variables

		int size;
		Dictionary<Vector3, Field> grid;
		Field source;
		Field destination;
		string name;

		#endregion

		#region properties

		public Dictionary<Vector3, Field> Grid
		{
			get { return grid; }
		}

		#endregion

		#region methods

		public Map(int size)
		{
			source = null;
			destination = null;
			this.size = size;
			name = "default";

			grid = new Dictionary<Vector3, Field>();
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					Vector3 position = new Vector3(x, y, 0);
					grid[position] = new Field(position);
				}
			}

			source = GetRandomField();
			source.Type = FieldType.SOURCE;
			destination = GetRandomField();
			destination.Type = FieldType.DESTINATION;

		}

		Field GetRandomField()
		{
			Vector3 randomPosition = new Vector3(Random.Range(0, size), Random.Range(0, size), 0);
			Field outField;
			grid.TryGetValue(randomPosition, out outField);

			return outField;
		}

		#endregion
	}
}
