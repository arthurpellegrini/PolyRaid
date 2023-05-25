namespace STUDENT_NAME
{
	using UnityEngine;

	public class CameraController : SimpleGameStateObserver
	{
		[SerializeField] Transform m_Target;
		Transform m_Transform;
		Vector3 m_InitPosition;

		void ResetCamera()
		{
			m_Transform.position = m_InitPosition;
		}

		protected override void Awake()
		{
			base.Awake();
			m_Transform = transform;
			m_InitPosition = m_Transform.position;
		}

		void Update()
		{
			if (!GameManager.Instance.IsPlaying) return;

			// TO DO
		}

		protected override void GameMenu(GameMenuEvent e)
		{
			ResetCamera();
		}
	}
}