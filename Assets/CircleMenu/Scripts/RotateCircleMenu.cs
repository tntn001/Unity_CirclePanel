using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(EventTrigger))]
public class RotateCirlceMenu : MonoBehaviour
{
    private EventTrigger Trgr;

    private bool isE;
    private Vector2 OdPos;
    private Vector2 NwPos;
    private Vector2 PnlPos;
    private RectTransform reTrfm;
	// Use this for initialization
	void Start () 
    {        
        
        //this.hideFlags = HideFlags.HideInInspector;
        reTrfm = GetComponent<RectTransform>();
        PnlPos = new Vector2(reTrfm.position.x, reTrfm.position.y);
        OdPos = Vector2.zero;
        isE = false;
        Trgr = GetComponent<EventTrigger>();
        //EventTrigger.Entry[] Entry = new EventTrigger.Entry[6];
        EventTrigger.Entry DrE = new EventTrigger.Entry();
        EventTrigger.Entry BgDrE = new EventTrigger.Entry();
        EventTrigger.Entry EdDrE = new EventTrigger.Entry();
        EventTrigger.Entry PtDnE = new EventTrigger.Entry();
        EventTrigger.Entry PtUpE = new EventTrigger.Entry();
        EventTrigger.Entry ClkE = new EventTrigger.Entry();
        EventTrigger.Entry EtE = new EventTrigger.Entry();
        EventTrigger.Entry ExE = new EventTrigger.Entry();

        DrE.eventID = EventTriggerType.Drag;
        DrE.callback.AddListener((dt) =>
            {
                OnDrag((PointerEventData)dt);
            });
        Trgr.triggers.Add(DrE);
        BgDrE.eventID = EventTriggerType.BeginDrag;
        BgDrE.callback.AddListener((dt) =>
            {
                OnBeginDrag((PointerEventData)dt);
            });
        Trgr.triggers.Add(BgDrE);
        EdDrE.eventID = EventTriggerType.EndDrag;
        EdDrE.callback.AddListener((dt) =>
            {
                OnEndDrag((PointerEventData)dt);
            });
        Trgr.triggers.Add(EdDrE);
        PtDnE.eventID = EventTriggerType.PointerDown;
        PtDnE.callback.AddListener((dt) =>
            {
                OnPointerDown((PointerEventData)dt);
            });
        Trgr.triggers.Add(PtDnE);
        PtUpE.eventID = EventTriggerType.PointerUp;
        PtUpE.callback.AddListener((dt) =>
            {
                OnPointerUp((PointerEventData)dt);
            });
        Trgr.triggers.Add(PtUpE);
        ClkE.eventID = EventTriggerType.PointerClick;
        ClkE.callback.AddListener((dt) =>
            {
                OnClick((PointerEventData)dt);
            });
        Trgr.triggers.Add(ClkE);
        EtE.eventID = EventTriggerType.PointerEnter;
        EtE.callback.AddListener((dt) =>
            {
                PointerEnter((PointerEventData)dt);
            });
        Trgr.triggers.Add(EtE);
        ExE.eventID = EventTriggerType.PointerExit;
        ExE.callback.AddListener((dt) =>
            {
                PointerExit((PointerEventData)dt);
            });
        Trgr.triggers.Add(ExE);
        
	}
	
    void OnDrag(PointerEventData eDt)
    {
        if (isE)
        {
            //Debug.Log("Position: " + eventData.position);
            NwPos = eDt.position;
            if (NwPos != OdPos)
            {        
                //var angle = Vector2.Angle(NwPos, OdPos); 
                var nwPos = NwPos - PnlPos;
                var odPos = OdPos - PnlPos;

                var ag = CustomVector.Angle(nwPos, odPos);   
                reTrfm.Rotate(0, 0, ag);
            }
            OdPos = NwPos;
        }
    }
    void OnBeginDrag(PointerEventData eDt)
    {
        
    }
    void OnEndDrag(PointerEventData eDt)
    {

    }
    void OnPointerDown(PointerEventData eDt)
    {
       
        OdPos = eDt.position;
    }
    void OnPointerUp(PointerEventData eDt)
    {
        
    }
    void OnClick(PointerEventData eDt)
    {

    }
    void PointerEnter(PointerEventData eDt)
    {
        isE = true;
    }
    void PointerExit(PointerEventData eDt)
    {
        isE = false;
    }
}


