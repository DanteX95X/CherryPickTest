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
		[SerializeField]
		InputField numberOfObstaclesInput = null;

		int mapSize;
		int numberOfObstacles;
		Map newMap;
		bool isMapGeneratedSuccessfully;
		#endregion

		#region methods

		public override void Init()
		{
			isMapGeneratedSuccessfully = false;
			mapSize = 0;
			numberOfObstacles = 0;
			newMap = null;

			LoadMapSize();
			LoadNumberOfObstacles();
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
			newMap = null;
		}

		public void GenerateMap()
		{
			if(mapSize < 6)
			{
				Debug.Log("Map is too small");
				return;
			}

			newMap = new Map(mapSize, numberOfObstacles);
			isMapGeneratedSuccessfully = true;
		}

		public void LoadMapSize()
		{
			int value = LoadFromInputField(mapSizeInput);
			if (value != -1)
			{
				mapSize = value;
			}
		}

		public void LoadNumberOfObstacles()
		{
			int value = LoadFromInputField(numberOfObstaclesInput);
			if(value != -1)
			{
				numberOfObstacles = value;
			}
		}

		public int LoadFromInputField(InputField input)
		{
			int value = -1;
			bool isNumber = Int32.TryParse(input.text, out value);

			return value;
		}
		

		#endregion
	}
}
