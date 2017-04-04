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

		[SerializeField]
		Image iconImage = null;

		[SerializeField]
		Text lifes = null;

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

		public Image IconImage
		{
			get { return iconImage; }
		}

		public Text Lifes
		{
			get { return lifes; }
		}
		#endregion
	}
}