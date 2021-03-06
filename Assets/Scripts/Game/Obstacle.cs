﻿using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Game
{
	class Obstacle
	{
		#region variables

		Vector3 position;
		Vector2 size;

		GameObject obstacleObject;
		#endregion

		#region properties

		public Vector3 Position
		{
			get { return position; }
		}

		public Vector2 Size
		{
			get { return size; }
		}

		public GameObject ObstacleObject
		{
			get { return obstacleObject; }
			set
			{
				obstacleObject = value;
				obstacleObject.transform.localScale = new Vector3(size.x, size.y, 2);
				obstacleObject.transform.position = position + new Vector3(size.x - 1, size.y - 1)/2;
			}
		}

		#endregion

		#region methods
		public Obstacle(Vector3 position, Vector2 size)
		{
			this.position = position;
			this.size = size;
			obstacleObject = null;
		}
		#endregion
	}
}
