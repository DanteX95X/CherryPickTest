using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Assets.Scripts.UI
{

	public class LevelButton : MonoBehaviour
	{
		#region variables

		[SerializeField]
		Button button = null;

		[SerializeField]
		Text levelName = null;

		#endregion

		#region properties

		public Button Button
		{
			get { return button; }
		}

		public Text LevelName
		{
			get { return levelName; }
		}
		#endregion
	}
}