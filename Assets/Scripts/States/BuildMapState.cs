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
		#endregion
	}
}
