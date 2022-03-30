
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour {

    [SerializeField] bool OnPointerEnter = true;
    [SerializeField] bool OnSubmit = true;
    [SerializeField] bool OnClick = true;
    [SerializeField] bool OnSelect = true;
    [SerializeField] bool OnPointerExit = false;

    EventTrigger et;

    private void Awake() {        
        et = gameObject.AddComponent<EventTrigger>();
    }

    private void Start() {
        AddPointerEnterEvent();
        AddPointerClickEvent();
        AddPointerExitEvent();
        AddSelectEvent();
        AddSubmitEvent();
    }

    void AddPointerEnterEvent() {
        if(!OnPointerEnter)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((eventData) => { AudioManager.Instance.PlaySound("Menu Enter"); });
        et.triggers.Add(entry);
    }

    void AddPointerClickEvent() {
        if(!OnClick)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((eventData) => { AudioManager.Instance.PlaySound("Menu Click"); });
        et.triggers.Add(entry);
    }

    void AddPointerExitEvent() {
        if(!OnPointerExit)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry {
            eventID = EventTriggerType.PointerExit
        };
        entry.callback.AddListener((eventData) => { AudioManager.Instance.PlaySound("Menu Exit"); });
        et.triggers.Add(entry);
    }

    void AddSelectEvent() {
        if(!OnSelect)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry {
            eventID = EventTriggerType.Select
        };
        entry.callback.AddListener((eventData) => { AudioManager.Instance.PlaySound("Menu Enter"); });
        et.triggers.Add(entry);
    }

    void AddSubmitEvent() {
        if(!OnSubmit)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry {
            eventID = EventTriggerType.Submit
        };
        entry.callback.AddListener((eventData) => { AudioManager.Instance.PlaySound("Menu Click"); });
    }
}