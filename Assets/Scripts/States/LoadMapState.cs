using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using Assets.Scripts.UI;

namespace Assets.Scripts.States
{
	[System.Serializable]
	public class Item
	{
		public string levelName;
		public Sprite iconImage;
		public string lifes;
	}

	class LoadMapState : State
	{
		#region variables

		[SerializeField]
		GameObject button = null;
		[SerializeField]
		List<Item> itemList = null;
		[SerializeField]
		Transform contentPanel = null;

		List<GameObject> buttons = null;

		#endregion


		#region methods

		public override void Init()
		{
			buttons = new List<GameObject>();
			itemList = new List<Item>();
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
			itemList.Clear();
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
				Debug.Log(exception.Message);
				return;
			}
		}

		void PopulateList()
		{
			foreach (var item in itemList)
			{
				GameObject newButton = Instantiate(button) as GameObject;
				LevelButton levelButton = newButton.GetComponent<LevelButton>();
				levelButton.LevelName.text = item.levelName;
				levelButton.Lifes.text = item.lifes;
				levelButton.IconImage.sprite = item.iconImage;

				string nameOfLevel = item.levelName;


				levelButton.Button.onClick.AddListener(() => { LoadMap(nameOfLevel + ".level"); });
				newButton.transform.SetParent(contentPanel);

				buttons.Add(newButton);
			}
		}

		public void GetItemsList()
		{
			DirectoryInfo dir = new DirectoryInfo("./");
			FileInfo[] info = dir.GetFiles("*.level");
			foreach (FileInfo f in info)
			{
				Debug.Log(f.Name);
				StreamReader reader = new StreamReader(f.Name);

				Item item = new Item();
				item.levelName = reader.ReadLine();
				item.lifes = "3"; // position in file in the future

				itemList.Add(item);
			}
		}
		#endregion
	}
}
