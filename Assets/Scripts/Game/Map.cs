using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

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

		readonly int minSize = 10;

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
			if(size < minSize)
			{
				throw new Exception("Map size must be at least " + minSize);
			}

			source = null;
			destination = null;
			this.size = size;
			name = "";
			obstacles = new List<Obstacle>();

			SetUpFields();

			source = GetRandomField();
            source.Type = FieldType.SOURCE;

			destination = GetRandomField();
            destination.Type = FieldType.DESTINATION;

			RandomizeObstaclesLocation(numberOfObstacles);
		}

		public Map(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				LoadMapName(reader);
				LoadMapSize(reader);
				SetUpFields();
				LoadSourceAndDestination(reader);
				LoadObstacles(reader);
			}
		}

		public void SetUpFields()
		{
			grid = new Dictionary<Vector3, Field>();
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					Vector3 position = new Vector3(x, y, 0);
					grid[position] = new Field(position);
				}
			}
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
				int width = (int)Mathf.Round(UnityEngine.Random.Range(1.0f, 2.0f));
				int height = (int)Mathf.Round(UnityEngine.Random.Range(1.0f, 2.0f));
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
				Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0);
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


		#region level_loading_methods
		void LoadMapName(StreamReader reader)
		{
			string line = reader.ReadLine();
			name = line;
		}

		void LoadMapSize(StreamReader reader)
		{
			string line = reader.ReadLine();
			bool hasSucceeded = Int32.TryParse(line, out size);

			if (!hasSucceeded)
			{
				throw new Exception("Map size is not an integer");
			}

			if(size < 10)
			{
				throw new Exception("Map size must be at least " + minSize);
			}
		}

		void LoadSourceAndDestination(StreamReader reader)
		{
			string line = reader.ReadLine();
			string[] sourcePosition = line.Split();
			line = reader.ReadLine();
			string[] destinationPosition = line.Split();

			int[] position = new int[2];
			for (int i = 0; i < 2; ++i)
			{
				if (!Int32.TryParse(sourcePosition[i], out position[i]))
				{
					throw new Exception("Source position is not a vector of integers");
				}
			}

			if (!grid.TryGetValue(new Vector3(position[0], position[1]), out source))
			{
				throw new Exception("Source - invalid position");
			}
			source.Type = FieldType.SOURCE;

			for (int i = 0; i < 2; ++i)
			{
				if (!Int32.TryParse(destinationPosition[i], out position[i]))
				{
					throw new Exception("Destination position is not a vector of integers");
				}
			}
			if (!grid.TryGetValue(new Vector3(position[0], position[1]), out destination))
			{
				throw new Exception("Destination - invalid position");
			}
			destination.Type = FieldType.DESTINATION;
		}

		void LoadObstacles(StreamReader reader)
		{
			obstacles = new List<Obstacle>();

			string line = reader.ReadLine();
			int numberOfObstacles = 0;
			if (!Int32.TryParse(line, out numberOfObstacles))
			{
				Debug.Log(line);
				throw new Exception("Number of obstacles is not an integer");
			}

			for (int i = 0; i < numberOfObstacles; ++i)
			{
				line = reader.ReadLine();
				string[] words = line.Split();
				int[] data = new int[4];

				for (int j = 0; j < 4; ++j)
				{
					if (!Int32.TryParse(words[j], out data[j]))
					{
						throw new Exception("Obstacle data is corrupted");
					}
				}
				PlaceObstacle(new Vector2(data[2], data[3]), new Vector3(data[0], data[1], 0));
			}
		}
		#endregion
	}
}
