﻿using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Utilities;

namespace Assets.Scripts.Game
{
	class Map
	{
		#region enum

		enum ObstacleType
		{
			SMALL = 0,
			HORIZONTAL = 1,
			VERTICAL = 2,
			BIG = 3,
		}
		#endregion

		#region variables

		int size;
		Dictionary<Vector3, Field> grid;
		List<Pair<Vector3, Vector2>> obstacles;
		Field source;
		Field destination;
		string name;

		#endregion

		#region properties

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

		#endregion

		#region methods

		public Map(int size)
		{
			source = null;
			destination = null;
			this.size = size;
			name = "default";
			obstacles = new List<Pair<Vector3, Vector2>>();

			grid = new Dictionary<Vector3, Field>();
			for (int x = 0; x < size; ++x)
			{
				for (int y = 0; y < size; ++y)
				{
					Vector3 position = new Vector3(x, y, 0);
					grid[position] = new Field(position);
				}
			}

			Debug.Log(size);

			source = GetRandomField();
            source.Type = FieldType.SOURCE;

			destination = GetRandomField();
            destination.Type = FieldType.DESTINATION;

			RandomizeObstaclesLocation(10);
		}

		void RandomizeObstaclesLocation(int desiredQuantity)
		{
			int obstaclesQuantity = 0;
			int maxTrials = size * size;

			while(obstaclesQuantity < desiredQuantity)
			{
				Field obstacleField = null;
				int width = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
				int height = (int)Mathf.Round(Random.Range(1.0f, 2.0f));
				Vector2 obstacleSize = new Vector2(width, height);
				Debug.Log(obstacleSize);
				int numberOfTrials = 0;
				do
				{
					Field consideredField = GetRandomField();
					++numberOfTrials;

					if (DoesObstacleFitLocation(obstacleSize, consideredField.Position))
					{
						obstacleField = consideredField;
						++obstaclesQuantity;
					}
					else if(consideredField.Type == FieldType.CLEAR)
					{
						obstacleSize = new Vector2(1, 1);
						obstacleField = consideredField;
						++obstaclesQuantity;
					}
				}
				while (obstacleField == null && numberOfTrials < maxTrials);
				//Debug.Log("Size: " + obstacleSize + " Position: " + obstacleField.Position + " Trials: " + numberOfTrials);

				if(numberOfTrials > maxTrials)
				{
					Debug.Log("Failed to generate obstacles");
					return;
				}

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
						Debug.Log("Too large obstacle");
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
			obstacles.Add(new Pair<Vector3, Vector2>(position, obstacleSize));
		}

		Field GetRandomField()
		{
			Vector3 randomPosition = new Vector3(Random.Range(0, size), Random.Range(0, size), 0);
			Field outField;
			grid.TryGetValue(randomPosition, out outField);

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
		}

		#endregion
	}
}
