using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour {
	private Button m_button;

	private void Start() {
		m_button = GetComponent<Button>();

		EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
		if (trigger == null) {
			trigger = gameObject.AddComponent<EventTrigger>();
		}

		// Add PointerEnter event
		EventTrigger.Entry entryEnter = new EventTrigger.Entry();
		entryEnter.eventID = EventTriggerType.PointerEnter;
		entryEnter.callback.AddListener((data) => { OnPointerEnter(); });
		trigger.triggers.Add(entryEnter);

		// Add PointerClick event
		EventTrigger.Entry entryClick = new EventTrigger.Entry();
		entryClick.eventID = EventTriggerType.PointerClick;
		entryClick.callback.AddListener((data) => { OnPointerClick(); });
		trigger.triggers.Add(entryClick);
	}

	public void OnPointerEnter() {
		AudioManager.instance.PlayButtonHover(Vector3.zero);
	}

	public void OnPointerClick() {
		AudioManager.instance.PlayButtonClick(Vector3.zero);
	}
}
