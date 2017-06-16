using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(EventTrigger))]
public class CrclMnRt : MonoBehaviour 
{
    public event Action EndRotateAround;
    readonly float _rate_scale_button_target_ = 0.4f;
    readonly float _delta_angle_scale_button_ = 10f;

    private EventTrigger Trgr;

    private bool isE;
    private Vector2 OdPos;
    private Vector2 NwPos;
    private Vector2 PnlPos;
    private RectTransform reTrfm;
    private float m_fDeltaRotate;
    private bool m_isElastic;   
    private float m_fElasticity;
    private int m_nPart;
    private float m_gAnglePart;
    private bool m_isTouch = false;
    private bool m_isRunElastic;
    private float m_fFirstAngle;
    private float oddRotation;
    private Vector3 targetRotation;
    private float SpeedRotate;
    private float deltaRotate;
    private bool HasAcceleration;
    private int RotateDirection; // 1 or -1
    private bool m_isTargetButton;
    private List<RectTransform> m_listButton;
    private Vector2 m_fCurrentButtonScale;
    private float m_fButtonIndex;
    private float m_fScaleButtonFirst;
    private float m_fScaleButtonSecond; 
    private bool m_isCanDrag =  true;
    private float m_fspeed = 1;

    private int m_nCurrentButton;
    public int CurrentButton
    {
        get{ return m_nCurrentButton; }
    }
    // Use this for initialization  

    #region PUBLIC METHOD
    public void SetElasticity(int part,float elasticity, float firstAngle, float speed = 1)
    {
        m_isElastic = true;
        m_isTouch = false;
        m_isRunElastic = false;
        m_fElasticity = elasticity;   
        m_nPart = part;
        m_gAnglePart = 360f / m_nPart;
        m_fFirstAngle = firstAngle;
        m_fspeed = speed;
    }
    public void MoveToNext(int next)
    {
        m_isRunElastic = true;   
        targetRotation = reTrfm.localRotation.eulerAngles + next * (new Vector3(0, 0, m_gAnglePart));
        targetRotation = (targetRotation.z < 0)? (new Vector3(0,0,360 + targetRotation.z)): targetRotation;
    }

    public void RotateRound(float degree)
    {
        HasAcceleration = true;
        deltaRotate = Mathf.Abs(degree);
        m_isTouch = true;
        RotateDirection = degree > 0 ? -1 : 1;
        m_isCanDrag = false;
    }
    #endregion

    #region void start
    void Start ()
    {   
        reTrfm = GetComponent<RectTransform>();
        reTrfm.localRotation = Quaternion.Euler(new Vector3(0, 0, m_fFirstAngle));
        PnlPos = new Vector2(reTrfm.position.x, reTrfm.position.y);
        OdPos = Vector2.zero;
        isE = false;
        m_isRunElastic = false;
        Trgr = GetComponent<EventTrigger>();       
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
    #endregion

    void FixedUpdate()
    {
        if (m_isElastic)
        {
            if (!m_isTouch)
            {
                if (!m_isRunElastic)
                {
                    oddRotation = Mathf.Abs((reTrfm.rotation.eulerAngles.z-m_fFirstAngle) % m_gAnglePart);
                    if (oddRotation > 0)
                    {
                        m_isRunElastic = true;
                        if (oddRotation > (m_gAnglePart / 2))
                        {
                            oddRotation = m_gAnglePart - oddRotation;
                            targetRotation = reTrfm.localRotation.eulerAngles + new Vector3(0, 0, oddRotation);
                        }
                        else
                        {
                            targetRotation = reTrfm.localRotation.eulerAngles - new Vector3(0, 0, oddRotation);
                        }
                    }
                }
                else
                {                    
                    oddRotation = Mathf.Abs(targetRotation.z - reTrfm.localEulerAngles.z);
                    if (targetRotation.z > 360)
                        oddRotation = Mathf.Abs(targetRotation.z - 360 - reTrfm.localEulerAngles.z);                   
                    SpeedRotate = oddRotation * m_fElasticity * 0.2f;
                   
                    if (oddRotation < 0.01f)
                    {
                        m_isRunElastic = false;
                        return;
                    }                    
                    reTrfm.localRotation = Quaternion.Lerp(reTrfm.localRotation, Quaternion.Euler(targetRotation), SpeedRotate * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (HasAcceleration)
                {  
                    if (deltaRotate <= 0)
                    {
                        HasAcceleration = false;
                        m_isTouch = false;
                        m_isRunElastic = false;
                        if (!m_isCanDrag)
                        {
                            m_isCanDrag = true;
                            if (EndRotateAround != null)
                                EndRotateAround();
                        }
                    }
                    reTrfm.Rotate(0, 0, deltaRotate * Time.fixedDeltaTime * RotateDirection);
                    deltaRotate = deltaRotate - (50 * Time.fixedDeltaTime);
                }
            }
        }
    }

    void OnDrag(PointerEventData eDt)
    {
        if (m_isCanDrag)
        {
            if (isE)
            {            
                NwPos = eDt.position;
                m_fDeltaRotate = CustomVector.Angle(NwPos - PnlPos, OdPos - PnlPos);
                if (NwPos != OdPos)
                {  
                    var nwPos = NwPos - PnlPos;
                    var odPos = OdPos - PnlPos;

                    var ag = CustomVector.Angle(nwPos, odPos);   
                    RotateDirection = ag > 0 ? 1 : -1;
                    reTrfm.Rotate(0, 0, ag);
                }
                OdPos = NwPos;
            }
        }
    }
    void OnBeginDrag(PointerEventData eDt)
    {
        if (m_isCanDrag)
        {
            OdPos = eDt.position;
            m_isTouch = true;
            m_isRunElastic = false;
            HasAcceleration = false;
        }
    }
    void OnEndDrag(PointerEventData eDt)
    {
        if (m_isCanDrag)
        {
            m_isTouch = false;
            deltaRotate = Vector3.Magnitude(eDt.delta);
            if (deltaRotate > 0)
            {
                deltaRotate *= ((10f + m_fspeed) / 10f);
                m_isTouch = true;
                HasAcceleration = true;
            }
        }
    }
    void OnPointerDown(PointerEventData eDt)
    {
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


public struct CustomVector
{
    public static float Angle(Vector2 vt1, Vector2 vt2)
    {
        int Dir = 0;
        vt1.Normalize();
        vt2.Normalize();

        var angle = Vector2.Angle(vt1, vt2);
        
        if (vt1.x >= 0 && vt1.y >= 0)
        {
            if (vt2.x >= 0 && vt2.y >= 0)
            {
                Dir = ((vt2.y - vt1.y) > 0) ? -1 : 1;
            }
            else if (vt2.x < 0 && vt2.y >= 0)
            {
                Dir = -1;
            }
            else if (vt2.x >= 0 && vt2.y < 0)
            {
                Dir = 1;
            }
            else
            {
                if (Mathf.Abs(vt2.x) <= Mathf.Abs(vt1.x))
                    Dir = 1;
                else
                    Dir = -1;
            }
        }
        else if (vt1.x >= 0 && vt1.y < 0)
        {
            if (vt2.x >= 0 && vt2.y < 0)
            {
                Dir = ((vt2.y - vt1.y) > 0) ? -1 : 1;
            }
            else if (vt2.x < 0 && vt2.y < 0)
            {
                Dir = 1;
            }
            else if (vt2.x >= 0 && vt2.y >= 0)
            {
                Dir = -1;
            }
            else
            {
                if (Mathf.Abs(vt2.x) <= Mathf.Abs(vt1.x))
                    Dir = -1;
                else
                    Dir = 1;
            }

        }
        else if (vt1.x < 0 && vt1.y < 0)
        {
            if (vt2.x < 0 && vt2.y < 0)
            {
                Dir = ((vt2.y - vt1.y) > 0) ? 1 : -1;
            }
            else if (vt2.x >= 0 && vt2.y < 0)
            {
                Dir = -1;
            }
            else if (vt2.x < 0 && vt2.y >= 0)
            {
                Dir = 1;
            }
            else
            {
                if (Mathf.Abs(vt2.x) <= Mathf.Abs(vt1.x))
                    Dir = 1;
                else
                    Dir = -1;
            }
        }
        else
        {
            if (vt2.x < 0 && vt2.y >= 0)
            {
                Dir = ((vt2.y - vt1.y) >= 0) ? 1 : -1;
            }
            else if (vt2.x < 0 && vt2.y < 0)
            {
                Dir = -1;
            }
            else if (vt2.x >= 0 && vt2.y >= 0)
            {
                Dir = 1;
            }
            else
            {
                if (Mathf.Abs(vt2.x) <= Mathf.Abs(vt1.x))
                    Dir = -1;
                else
                    Dir = 1;
            }

        }
        return Dir * angle;
    }
}

