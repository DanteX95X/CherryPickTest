using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using Assets.Scripts.UI;

namespace Assets.Scripts.States
{
	class LoadMapState : State
	{
		#region variables

		[SerializeField]
		GameObject button = null;
		[SerializeField]
		List<string> mapNames = null;
		[SerializeField]
		Transform contentPanel = null;

		List<GameObject> buttons = null;

		#endregion


		#region methods

		public override void Init()
		{
			buttons = new List<GameObject>();
			mapNames = new List<string>();
			GetItemsList();
			PopulateList();
		}

		public override void UpdateLoop()
		{
		}

		public override void CleanUp()
		{
			foreach(GameObject button in buttons)
			{
				Destroy(button);
			}
			mapNames.Clear();
		}

		public void LoadMap(string path)
		{
			try
			{
				GetComponent<BuildMapState>().CurrentMap = new Game.Map(path);
				ChangeState<BuildMapState>();
			}
			catch (Exception exception)
			{
				DisplayMessage(exception.Message);
				return;
			}
		}

		void PopulateList()
		{
			foreach (string name in mapNames)
			{
				GameObject newButton = Instantiate(button) as GameObject;
				LevelButton levelButton = newButton.GetComponent<LevelButton>();
				levelButton.LevelName.text = name;

				levelButton.Button.onClick.AddListener(() => { LoadMap(name + ".level"); });
				newButton.transform.SetParent(contentPanel);

				buttons.Add(newButton);
			}
		}

		public void GetItemsList()
		{
			DirectoryInfo directory = new DirectoryInfo("./");
			FileInfo[] info = directory.GetFiles("*.level");
			foreach (FileInfo file in info)
			{
				Debug.Log(file.Name);
				StreamReader reader = new StreamReader(file.Name);
				string mapName = reader.ReadLine();
				mapNames.Add(mapName);
			}
		}

		public void GoBackToMainMenu()
		{
			ChangeState<MainMenuState>();
		}
		#endregion
	}
}
