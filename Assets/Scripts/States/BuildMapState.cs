using System;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Game;

namespace Assets.Scripts.States
{
	class BuildMapState : State
	{
		#region variables


		[SerializeField]
		List<GameObject> prefabs = null;

		Map currentMap;

		#endregion


		#region properties

		public Map CurrentMap
		{
			set { currentMap = value; }
		}

		#endregion

		#region methods
		public override void Init()
		{
			foreach (Field field in currentMap.Grid.Values)
			{
				field.SetUpNeighbours(currentMap.Grid);
				field.FieldObject = Instantiate(prefabs[(int)field.Type], field.Position, Quaternion.identity) as GameObject;
			}

			foreach (Obstacle obstacle in currentMap.Obstacles)
			{
				obstacle.ObstacleObject = Instantiate(prefabs[3]) as GameObject;
			}

			SetCamera(new Vector2[] { new Vector2(0, 0), new Vector2(currentMap.Size, currentMap.Size) });

			GetComponent<FindPathState>().CurrentMap = currentMap;
			ChangeState<FindPathState>();
		}

		public override void UpdateLoop()
		{
		}

		public override void CleanUp()
		{
			currentMap = null;
		}

		void SetCamera(Vector2[] positions)
		{
			Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
			Vector3 desiredPosition = new Vector3((positions[0].x + positions[1].x) / 2, (positions[0].y + positions[1].y) / 2, -10);
			mainCamera.transform.position = desiredPosition;


			Vector3 desiredLocalPosition = mainCamera.transform.InverseTransformPoint(desiredPosition);
			float size = 0;
			foreach (Vector3 position in positions)
			{
				Vector3 targetLocalPosition = mainCamera.transform.InverseTransformPoint(position);
				Vector3 desiredPositionToTarget = targetLocalPosition - desiredLocalPosition;

				size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.y));
				size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.x) / mainCamera.aspect);
			}
			size += 1;
			mainCamera.orthographicSize = size;
		}

		#endregion
	}
}
