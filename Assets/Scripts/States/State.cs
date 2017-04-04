using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.States
{
	public abstract class State : MonoBehaviour
	{
		#region variables

		[SerializeField]
		GameObject ui = null;

		GameObject messageWindow = null;

		#endregion


		#region methods

		public void OnEnable()
		{
			if (ui != null)
				ui.SetActive(true);

			messageWindow = GameObject.FindGameObjectWithTag("MessageWindow");
			if (messageWindow.GetComponentInChildren<Text>().text == "")
			{
				messageWindow.SetActive(false);
			}

			Init();
		}

		public void OnDisable()
		{
			if (ui != null)
				ui.SetActive(false);

			messageWindow.SetActive(true);
			CleanUp();
		}

		public void Update()
		{
			UpdateLoop();
		}

		IEnumerator FadeMessage(string message, float seconds)
		{
			messageWindow.GetComponentInChildren<Text>().text = message;
			messageWindow.SetActive(true);
			yield return new WaitForSeconds(seconds);
			messageWindow.SetActive(false);
			messageWindow.GetComponentInChildren<Text>().text = "";
		}

		protected void DisplayMessage(string message, float seconds = 1)
		{
			StartCoroutine(FadeMessage(message, seconds));
		}

		public abstract void Init();

		public abstract void UpdateLoop();

		public abstract void CleanUp();

		public void ChangeState<T>() where T : State
		{
			enabled = false;
			GetComponent<T>().enabled = true;
		}

		#endregion
	}
}
