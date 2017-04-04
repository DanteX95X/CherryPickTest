using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Game;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.States
{
	class FindPathState : State
	{
		#region variables

		[SerializeField]
		Material pathFieldMaterial = null;

		[SerializeField]
		List<Material> fieldMaterials = null;

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
			
		}

		public override void UpdateLoop()
		{
		}

		public override void CleanUp()
		{
			currentMap.DestroyMap();
			currentMap = null;
		}

		public void FindPathWithBFS()
		{
			List<Field> path = Pathfinding.BFS(currentMap.Source, currentMap.Destination);
			ShowPath(path);
		}

		public void FindPathWithAStar()
		{
			List<Field> path = Pathfinding.AStar(currentMap.Source, currentMap.Destination, Pathfinding.ManhattanDistanceHeuristic);
			ShowPath(path);
		}

		public void ClearMap()
		{
			foreach(Field field in currentMap.Grid.Values)
			{
				field.FieldObject.GetComponent<Renderer>().material = fieldMaterials[(int)field.Type];
			}
		}

		void ShowPath(List<Field> path)
		{
			ClearMap();

			if (path.Count < 2)
			{
				Debug.Log("Destination Unreachable");
				return;
			}

			for (int i = 0; i < path.Count; ++i)
			{
				path[i].FieldObject.GetComponent<Renderer>().material = pathFieldMaterial;
			}
		}

		public void GoToMainMenu()
		{
			ChangeState<MainMenuState>();
		}

		#endregion
	}
}
