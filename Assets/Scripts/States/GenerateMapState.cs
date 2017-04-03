using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Game;

namespace Assets.Scripts.States
{
	class GenerateMapState : State
	{
		#region variables

		[SerializeField]
		InputField mapSizeInput = null;

		int mapSize;
		Map newMap;
		bool isMapGeneratedSuccessfully;
		#endregion

		#region methods

		public override void Init()
		{
			isMapGeneratedSuccessfully = false;
			mapSize = 0;
			newMap = null;
		}

		public override void UpdateLoop()
		{
			if(isMapGeneratedSuccessfully)
			{
				GetComponent<BuildMapState>().CurrentMap = newMap;
				ChangeState<BuildMapState>();
			}
		}

		public override void CleanUp()
		{

		}

		public void GenerateMap()
		{
			newMap = new Map(mapSize);
			isMapGeneratedSuccessfully = true;
		}

		public void LoadMapSize()
		{
			Debug.Log(mapSizeInput.text);
			int tempSize = -1;
			bool isNumber = Int32.TryParse(mapSizeInput.text, out tempSize);

			if(isNumber)
			{
				mapSize = tempSize;
			}
		}

		#endregion
	}
}
