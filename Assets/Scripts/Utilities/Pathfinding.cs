using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Game;

namespace Assets.Scripts.Utilities
{
	static class Pathfinding
	{
		public static List<Field> BFS(Field source, Field destination)
		{
			if(source == destination)
			{
				return new List<Field>() { source, destination };
			}

			HashSet<Field> visited = new HashSet<Field>();
			Queue<Field> frontier = new Queue<Field>();
			Dictionary<Field, Field> cameFrom = new Dictionary<Field, Field>();

			frontier.Enqueue(source);
			cameFrom[source] = null;

			while(frontier.Count > 0)
			{
				Field currentField = frontier.Dequeue();

				if(currentField == destination)
				{
					break;
				}

				visited.Add(currentField);

				foreach(Field neighbour in currentField.Neighbours.Values)
				{
					if (!visited.Contains(neighbour))
					{
						frontier.Enqueue(neighbour);
						cameFrom[neighbour] = currentField;
					}
				}
			}

			return GetPath(source, destination, cameFrom);

		}

		static List<Field> GetPath(Field source, Field destination, Dictionary<Field, Field> cameFrom)
		{
			List<Field> path = new List<Field>();
			Field currentField = destination;

			do
			{
				path.Add(currentField);
				currentField = cameFrom[currentField];
			}
			while (currentField != null);

			path.Reverse();
			return path;
		}

	}
}
