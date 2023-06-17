using UnityEngine;
using UnityEngine.UI;

public class PlaySoundOnClick : MonoBehaviour {


	[SerializeField] string m_SoundName;
	
	void Start() {
		Button button = GetComponent<Button>();
		if (button) button.onClick.AddListener(PlaySound);
	}
	
	void PlaySound()
	{
		if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D(m_SoundName);
	}
}
