using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.States
{
	class MainMenuState : State
	{
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

		public void GoToGenerateMapState()
		{
			ChangeState<GenerateMapState>();
		}

		public void ShutDownApplication()
		{
			Application.Quit();
		}

		#endregion
	}
}
