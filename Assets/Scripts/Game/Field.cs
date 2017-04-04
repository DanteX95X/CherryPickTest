using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Game
{
	public class Field
	{
		#region variables

		Vector3 position;
		Dictionary<Vector3, Field> neighbours;
		GameObject fieldObject;
		FieldType type;

		#endregion

		#region properties

		public Vector3 Position
		{
			get { return position; }
		}

		public GameObject FieldObject
		{
			get { return fieldObject; }
			set { fieldObject = value; }
		}

		public FieldType Type
		{
			get { return type; }
			set { type = value; }
		}

		public Dictionary<Vector3, Field> Neighbours
		{
			get { return neighbours; }
		}
		#endregion


		#region methods

		public Field(Vector3 position)
		{
			this.position = position;
			neighbours = new Dictionary<Vector3, Field>();
			fieldObject = null;
			type = FieldType.CLEAR;
		}


		public void SetUpNeighbours(Dictionary<Vector3, Field> grid)
		{
			List<Vector3> displacements = new List<Vector3>() { new Vector3(0, 1), new Vector3(1, 0), new Vector3(0, -1), new Vector3(-1, 0) };

			foreach (Vector3 displacement in displacements)
			{
				Field neighbour = null;
				grid.TryGetValue(position + displacement, out neighbour);

				if (neighbour != null)
				{
					neighbours[displacement] = neighbour;
				}
			}
		}

		public double GetCost()
		{
			return 1;
		}

		#endregion
	}
}
