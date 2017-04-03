using UnityEngine;

namespace Assets.Scripts.States
{
	public abstract class State : MonoBehaviour
	{
		#region variables

		public static State currentState = null;

		#endregion


		#region methods

		public void OnEnable()
		{
			currentState = this;
			Init();
		}

		public void OnDisable()
		{
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
