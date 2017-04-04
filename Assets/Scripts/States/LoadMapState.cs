using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.States
{
	class LoadMapState : State
	{

		#region methods

		public override void Init()
		{
			try
			{
				LoadMap("ufo.level");
			}
			catch(Exception exception)
			{
				Debug.Log(exception.Message);
				return;
			}

			ChangeState<BuildMapState>();
		}

		public override void UpdateLoop()
		{
		}

		public override void CleanUp()
		{
		}

		public void LoadMap(string path)
		{
			GetComponent<BuildMapState>().CurrentMap = new Game.Map(path);

		}
		#endregion
	}
}
