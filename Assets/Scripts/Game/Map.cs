using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Utilities;

namespace Assets.Scripts.Game
{
	class Map
	{
		#region variables

		int size;
		Dictionary<Vector3, Field> grid;
		List<Obstacle> obstacles;
		Field source;
		Field destination;
		string name;

		#endregion

		#region properties

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public int Size
		{
			get { return size; }
		}

		public Dictionary<Vector3, Field> Grid
		{
			get { return grid; }
		}

		public Field Source
		{
			get { return source; }
		}

		public Field Destination
		{
			get { return destination; }
		}

		public List<Obstacle> Obstacles
		{
			get { return obstacles; }
		}
		#endregion

		#region methods

		public Map(int size, int numberOfObstacles)
		{
			source = null;
			destination = null;
			this.size = size;
			name = "";
			obstacles = new List<Obstacle>();

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

			RandomizeObstaclesLocation(numberOfObstacles);
		}

		void RandomizeObstaclesLocation(int desiredQuantity)
		{
			if(desiredQuantity > size*size - 2)
			{
				throw new System.Exception("Impossible to place that many obstacles");
			}

			for(int i = 0; i < desiredQuantity; ++i)
			{
				Field obstacleField = null;
				int width = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
				int height = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
				Vector2 obstacleSize = new Vector2(width, height);

				do
				{
					Field consideredField = GetRandomField();
					if(consideredField == null)
					{
						throw new System.Exception("Failed to generate that many obstacles");
					}

					if (!DoesObstacleFitLocation(obstacleSize, consideredField.Position))
					{
						obstacleSize = new Vector2(1, 1);
					}

					obstacleField = consideredField;
				}
				while (obstacleField == null);
				
				PlaceObstacle(obstacleSize, obstacleField.Position);
			}
		}

		bool DoesObstacleFitLocation(Vector2 obstacleSize, Vector3 position)
		{
			for(int x = 0; x < obstacleSize.x; ++x)
			{
				for(int y = 0; y < obstacleSize.y; ++y)
				{
					Vector3 displacement = new Vector3(x, y, 0);
					Vector3 obstaclePosition = position + displacement;

					Field consideredObstacle = null;
					grid.TryGetValue(obstaclePosition, out consideredObstacle);

					if(consideredObstacle == null || consideredObstacle.Type != FieldType.CLEAR)
					{
						return false;
					}
				}
			}

			return true;
		}

		void PlaceObstacle(Vector2 obstacleSize, Vector3 position)
		{
			for (int x = 0; x < obstacleSize.x; ++x)
			{
				for (int y = 0; y < obstacleSize.y; ++y)
				{
					Vector3 displacement = new Vector3(x, y, 0);
					Vector3 obstaclePosition = position + displacement;

					grid[obstaclePosition].Type = FieldType.OBSTACLE;
				}
			}

			obstacles.Add(new Obstacle(position, obstacleSize));
		}

		Field GetRandomField()
		{
			Field outField = null;
			int trials = 0;
			int maxTrials = size * size;
			do
			{
				if (trials > maxTrials)
					return null;

				outField = null;
				Vector3 randomPosition = new Vector3(Random.Range(0, size), Random.Range(0, size), 0);
				grid.TryGetValue(randomPosition, out outField);
				++trials;
			}
			while (outField == null || outField.Type != FieldType.CLEAR);

			return outField;
		}

		public void DestroyMap()
		{
			foreach(Field field in grid.Values)
			{
				if(field.FieldObject)
				{
					GameObject.Destroy(field.FieldObject);
				}
			}

			foreach(Obstacle obstacle in obstacles)
			{
				if(obstacle.ObstacleObject)
				{
					GameObject.Destroy(obstacle.ObstacleObject);
				}
			}
		}

		public override string ToString()
		{
			string mapString = "";
			mapString += name + "\n" + size + "\n";
			mapString += source.Position.x + " " + source.Position.y + "\n";
			mapString += destination.Position.x + " " + destination.Position.y + "\n";
			mapString += obstacles.Count + "\n";

			foreach(Obstacle obstacle in obstacles)
			{
				mapString += obstacle.Position.x + " " + obstacle.Position.y + " " + obstacle.Size.x + " " + obstacle.Size.y + "\n";
			}

			return mapString;
		}

		#endregion
	}
}
