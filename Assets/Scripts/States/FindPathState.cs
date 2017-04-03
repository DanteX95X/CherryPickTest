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
		}

		public void FindPathWithBFS()
		{
			List<Field> path = Pathfinding.BFS(currentMap.Source, currentMap.Destination);

			if (path.Count < 2)
			{
				Debug.Log("Destination Unreachable");
				return;
			}

			ShowPath(path);
		}

		void ShowPath(List<Field> path)
		{
			for (int i = 1; i < path.Count - 1; ++i)
			{
				path[i].FieldObject.GetComponent<Renderer>().material = pathFieldMaterial;
			}
		}

		#endregion
	}
}
