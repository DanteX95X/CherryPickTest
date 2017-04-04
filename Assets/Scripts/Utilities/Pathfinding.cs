using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Game;

namespace Assets.Scripts.Utilities
{
	static class Pathfinding
	{
		#region heuristics

		public delegate double Heuristic(Field source, Field destination);

		public static double ManhattanDistanceHeuristic(Field source, Field destination)
		{
			Vector3 direction = destination.Position - source.Position;
			double distance = (double)(direction.x + direction.y);
			return distance;
		}

		#endregion


		#region methods

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
			visited.Add(source);

			while(frontier.Count > 0)
			{
				Field currentField = frontier.Dequeue();

				if(currentField == destination)
				{
					break;
				}

				foreach(Field neighbour in currentField.Neighbours.Values)
				{
					if (!visited.Contains(neighbour))
					{
						frontier.Enqueue(neighbour);
						cameFrom[neighbour] = currentField;
						visited.Add(neighbour);
					}
				}
			}

			return GetPath(source, destination, cameFrom);

		}

		public static List<Field> AStar(Field source, Field destination, Heuristic heuristic)
		{
			if(source == destination)
			{
				return new List<Field>() { source, destination };
			}

			HashSet<Field> visited = new HashSet<Field>();
			PriorityQueue<Field> frontier = new PriorityQueue<Field>();
			Dictionary<Field, Field> cameFrom = new Dictionary<Field, Field>();
			Dictionary<Field, double> cost = new Dictionary<Field, double>();

			cameFrom[source] = null;
			cost[source] = 0;
			frontier.Push(source, heuristic(source, destination));
			visited.Add(source);

			while(frontier.Count > 0)
			{
				Field currentField = frontier.Pop();

				if(currentField == destination)
				{
					break;
				}

				foreach(Field neighbour in currentField.Neighbours.Values)
				{
					double neighbourCost = neighbour.GetCost() + cost[currentField];
					if(!visited.Contains(neighbour) || neighbourCost < cost[neighbour])
					{
						frontier.Push(neighbour, neighbourCost + heuristic(neighbour, destination));
						visited.Add(neighbour);
						cameFrom[neighbour] = currentField;
						cost[neighbour] = neighbourCost;
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
				cameFrom.TryGetValue(currentField, out currentField);
			}
			while (currentField != null);

			path.Reverse();
			return path;
		}

		#endregion
	}
}
