/// <summary>
/// MAKE BY MONKEY
/// Email: ti.en.ti0000@gmail.com
/// 2016-11-12 (yyyy-mm-dd)
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
#endif

[ExecuteInEditMode]
[AddComponentMenu("")]
public class CircleMenu : MonoBehaviour
{
    public event Action EndRotateAround;
    enum APPEAR
    {
        NONE,
        LEFT,
        RIGHT,
        UPSIDE,
        DOWNSIDE,
    }

    readonly int _dfl_pdg_ = 2;
    
    [Space(20)]
    [Header("Setup before play")]
    [SerializeField]
    private int TopPadding;
    [SerializeField]
    private int BottomPadding;
    [Space(10)]
    [SerializeField]
    private int Space;
    [Space(10)]
    [SerializeField]
    private bool isRotateChild;
    [Space(10)]
    [Header("Only in runtime")]   

    #region IN RUNTIME        
    private Vector2 m_v2AppearAnchorPosition;
    #endregion
    [Space(10)]
    [SerializeField]
    [HideInInspector]
    private int NumbersButton;

   

    [SerializeField]
    private float m_fFirstAngle;

    [Space(10)]
    [SerializeField]
    private bool isElastic;

    [SerializeField]
    [HideInInspector]
    private float m_fElasticity;

    [SerializeField]
    [HideInInspector]
    private float m_fSpeedElastic = 1;
   

    [SerializeField]
    [HideInInspector]
    private List<RectTransform> LtBtn;
    public List<RectTransform> ListButton
    {
        get { return LtBtn; }
    }

    private RectTransform re;
    private float reW;
    private int m_nCurrentButton;
    private int m_nClickButton;
    private CrclMnRt m_mainRotate;
    public int CurrentButton
    {
        get{ return m_nCurrentButton; }
    }  
    public int ClickButton
    {
        get{ return m_nClickButton; }
    }

    private bool m_isCanClickButton;
    public bool CanClickButton
    {
        get{ return m_isCanClickButton; }
    }

    private DrivenRectTransformTracker drvTrkr;  

    public void RotateRound(float degree)
    {
        m_mainRotate.RotateRound(degree);
    }
    void Awake()
    {
        m_mainRotate = GetComponent<CrclMnRt>();           
    }

    // Use this for initialization
	void Start () 
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            Init();
            GtRctInChrn();
        }
        else
        {   
            m_mainRotate.EndRotateAround += delegate
            {
                if(EndRotateAround != null)
                    EndRotateAround();
            };
            if (isElastic)
            {
                m_mainRotate.SetElasticity(NumbersButton, m_fElasticity, m_fFirstAngle,m_fSpeedElastic);
            }
            re = GetComponent<RectTransform>();
            m_v2AppearAnchorPosition = re.anchoredPosition;          
        }
	}
	
	// Update is called once per frame
	void Update () 
    {        
        if (Application.isEditor && !Application.isPlaying)
        {
            GtRctInChrn();
            FxWnH();
        }
        else
        {
            m_nCurrentButton = m_mainRotate.CurrentButton;
        }
        
	}  

    void Init()
    {
        LtBtn = new List<RectTransform>();
        re = GetComponent<RectTransform>();
        reW = re.sizeDelta.x;

        GetComponent<IDrivenRect>().StAcrMx(new Vector2(0.5f,0.5f));
        GetComponent<IDrivenRect>().StAcMn(new Vector2(0.5f,0.5f));
        GetComponent<IDrivenRect>().StPv(new Vector2(0.5f,0.5f));
    }

    public void OnClick(int n)
    {        
        m_nClickButton = n;
        if (n < m_nCurrentButton)
        {
            if (m_nCurrentButton == (ListButton.Count - 1) && n == 0)
                m_mainRotate.MoveToNext(1);
            else
                m_mainRotate.MoveToNext(-1);
        }
        else if (n > m_nCurrentButton)
        {
            if (m_nCurrentButton == 0 && n == (ListButton.Count - 1))
                m_mainRotate.MoveToNext(-1);
            else
                m_mainRotate.MoveToNext(1);
        }
    }

    void GtRctInChrn()
    {
        LtBtn.Clear();
        foreach (RectTransform re in GetComponentsInChildren<RectTransform>())
        {
            if (re.transform.parent == this.transform)
            {
                LtBtn.Add(re); 

                if (re.GetComponent<NotEditableRect>() == null)
                {                    
                    re.gameObject.AddComponent(typeof(NotEditableRect));                     
                }
                else
                {
                    re.gameObject.GetComponent<IDrivenRect>().StAcrMx(new Vector2(0.5f, 0.5f));
                    re.gameObject.GetComponent<IDrivenRect>().StAcMn(new Vector2(0.5f, 0.5f));
                    re.gameObject.GetComponent<IDrivenRect>().StPv(new Vector2(0.5f, 0.5f));
                }
                drvTrkr.Add(re as UnityEngine.Object, re, DrivenTransformProperties.Anchors | DrivenTransformProperties.Pivot);               
            }
        }
        ConfigMenu();
    }
    void ConfigMenu()
    {
        reW = re.sizeDelta.x;
        var egVal = (reW/ (Mathf.Sqrt(2))) / (LtBtn.Count * 0.25f);
        if (LtBtn.Count == 1)
            egVal = egVal / 4f;
        else if (LtBtn.Count == 2)
            egVal = egVal / 2f;
        else if (LtBtn.Count == 3)
            egVal = egVal / 1.45f;
        else if (LtBtn.Count == 4)
            egVal = egVal / 1.35f;
        else if (LtBtn.Count == 5)
            egVal = egVal / 1.3f;
        else if (LtBtn.Count == 6)
            egVal = egVal / 1.25f;
        else if (LtBtn.Count == 7)
            egVal = egVal / 1.2f;
        else if (LtBtn.Count == 8)
            egVal = egVal / 1.15f;
        else if (LtBtn.Count == 9)
            egVal = egVal / 1.1f;
        else if (LtBtn.Count == 10)
            egVal = egVal / 1.05f;

        var WdVal = egVal - Space - _dfl_pdg_;
        var HtVal = egVal - TopPadding - BottomPadding - _dfl_pdg_;
        var AgStp = 360f / LtBtn.Count;
        var FstPos = re.anchoredPosition + new Vector2(-re.anchoredPosition.x, re.sizeDelta.y/2f + BottomPadding - TopPadding);
      
        for (int i = 0; i < LtBtn.Count; i++)
        {
            
            LtBtn[i].rotation = Quaternion.identity;
            LtBtn[i].sizeDelta = new Vector2(WdVal, HtVal);
            LtBtn[i].GetComponent<IDrivenRect>().StSzDlt(new Vector2(WdVal, HtVal));

            
            LtBtn[i].anchoredPosition = FstPos;                     
            var xScn = re.transform.position.x;                       
            var yScn = re.transform.position.y;                       
            LtBtn[i].RotateAround(new Vector2(xScn,yScn), Vector3.back, AgStp * i);            
            LtBtn[i].GetComponent<IDrivenRect>().StPos(new Vector2(LtBtn[i].anchoredPosition.x, LtBtn[i].anchoredPosition.y));
            LtBtn[i].GetComponent<IDrivenRect>().StRt(
                new Vector3(LtBtn[i].rotation.eulerAngles.x, LtBtn[i].rotation.eulerAngles.y, LtBtn[i].rotation.eulerAngles.z),false,isRotateChild);
        }       
    }

    float OdW,OdH,NwW,NwH;
    void FxWnH()
    {
        NwW = re.sizeDelta.x;
        NwH = re.sizeDelta.y;
        if (NwW != OdW)
        {
            OdW = NwW;
            OdH = NwW;
            NwH = OdH;
        }
        if(NwH != OdH)
        {
            OdH = NwH;
            OdW = NwH;
            NwW = OdW;
        }
        re.sizeDelta = new Vector2(NwW, NwH);       
    }
}

