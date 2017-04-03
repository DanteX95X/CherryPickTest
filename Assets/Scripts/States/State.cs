using UnityEngine;

namespace Assets.Scripts.States
{
	public abstract class State : MonoBehaviour
	{
		#region variables

		[SerializeField]
		GameObject ui = null;

		#endregion


		#region methods

		public void OnEnable()
		{
			if (ui != null)
				ui.SetActive(true);

			Init();
		}

		public void OnDisable()
		{
			if (ui != null)
				ui.SetActive(false);

			CleanUp();
		}

		public void Update()
		{
			UpdateLoop();
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
