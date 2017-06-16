using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class NotEditableRect : MonoBehaviour ,IDrivenRect
{
    [SerializeField]
    private Vector2 Pos;
    [SerializeField]
    private Vector2 Sz;
    [SerializeField]
    private Vector2 AcMx;
    [SerializeField]
    private Vector2 AcMn;
    [SerializeField]
    private Vector2 Pv;
    [SerializeField]
    private Vector3 Rotation;

    private RectTransform MnRe;
    private bool fxPos, fxSz, fxAcMx, fxAcMn, fxAcPv, fxAcRt;

    [SerializeField]
    private bool isRt;
    private bool isSt;
    void Start()
    {
        this.hideFlags = HideFlags.HideInInspector;     
        if (Application.isEditor && !Application.isPlaying)
        {
            Init();
        }
        else
        {
            MnRe = GetComponent<RectTransform>();
            MnRe.anchorMax = new Vector2(0.5f, 0.5f);
            MnRe.anchorMin = new Vector2(0.5f, 0.5f);
            MnRe.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    void Init()
    {
      
        MnRe = GetComponent<RectTransform>();
        Pos = MnRe.anchoredPosition;
        Sz = MnRe.sizeDelta;
        AcMx = MnRe.anchorMax;
        AcMn = MnRe.anchorMin;
        Pv = MnRe.pivot;

        if(!isSt)
            isRt = true;
    }
    void FixedRect()
    {
        if (fxPos)
        {
            MnRe.anchoredPosition = Pos;
        }
        if (fxSz)
        {
            MnRe.sizeDelta = Sz;
        }
        if (fxAcMx)
        {
            MnRe.anchorMax = AcMx;
        }
        if (fxAcMn)
        {
            MnRe.anchorMin = AcMn;
        }
        if (fxAcPv)
        {
            MnRe.pivot = Pv;
        }     
        if (fxAcRt)
        {
            MnRe.rotation = Quaternion.Euler(Rotation);
//            Debug.Log("hello");
        }
    }
    void Update()
    {
        if(Application.isEditor && !Application.isPlaying)
            FixedRect(); 
        
        if (!isRt)
        {
            MnRe.rotation = Quaternion.identity;
        }
    }
    public void StPos(Vector2 pos, bool _fd = true)
    {
        Pos = pos;
        fxPos = _fd;
        if (MnRe != null)
            MnRe.anchoredPosition = Pos;
    }
    public void StSzDlt(Vector2 sz, bool _fd = true)
    {
        Sz = sz;
        fxSz = _fd;
        if (MnRe != null)
            MnRe.sizeDelta = Sz;
    }
    public void StAcrMx(Vector2 mx, bool _fd = true)
    {
        AcMx = mx;
        fxAcMx = _fd;
        if (MnRe != null)
            MnRe.anchorMax = AcMx;
    }
    public void StAcMn(Vector2 mn, bool _fd = true)
    {
        AcMn = mn;
        fxAcMn = _fd;
        if (MnRe != null)
            MnRe.anchorMin = AcMn;
    }
    public void StPv(Vector2 pv, bool _fd = true)
    {
        Pv = pv;
        fxAcPv = _fd;
        if (MnRe != null)
            MnRe.pivot = Pv;
    }
    public void StRt(Vector3 rt, bool _fd = true, bool _isRt = true)
    {
        isSt = true;
        isRt = _isRt;
        fxAcRt = _fd;
        if (!_isRt)
        {
            if (MnRe != null)
            MnRe.rotation = Quaternion.identity;
            Rotation = Vector3.zero;
            return;
        }
        Rotation = rt;       
        if (MnRe != null)
        {
            MnRe.rotation = Quaternion.Euler(rt);
        }

    }
}
