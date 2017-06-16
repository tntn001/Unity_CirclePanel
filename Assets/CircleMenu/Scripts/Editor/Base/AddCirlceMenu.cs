using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;

public class AddCirlceMenu : MonoBehaviour
{

    [MenuItem("GameObject/UI/CirclePanel",false,0)]
    static void CreateCirclePanel()
    {        
        GameObject pr;

        if (CustomContext._ClickedObject == null)
        {
            var re = (RectTransform)GameObject.FindObjectOfType(typeof(RectTransform));

            if (re == null)
            {
                pr = ConfigEditor.CreateCanvas();
            }
            else
            {
                pr = re.gameObject;
            }
        }
        else
        {
            if (CustomContext._ClickedObject.GetComponent<RectTransform>() == null)
            {
                var re = (RectTransform)GameObject.FindObjectOfType(typeof(RectTransform));

                if (re == null)
                {
                    pr = ConfigEditor.CreateCanvas();
                }
                else
                {
                    pr = re.gameObject;
                }
            }
            else
            {
                pr = CustomContext._ClickedObject;
            }
        }
        GameObject go = new GameObject("CirlcePanel", typeof(Image));       
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        go.transform.SetParent(pr.transform);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        go.AddComponent(typeof(CrclMnRt));

        go.GetComponent<CrclMnRt>().hideFlags = HideFlags.HideInInspector;
        go.GetComponent<EventTrigger>().hideFlags = HideFlags.HideInInspector;
        Color clr = go.GetComponent<Image>().color;
        go.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 50f/255f);
        go.GetComponent<Image>().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        go.AddComponent(typeof(NotEditableRect));
        go.AddComponent(typeof(CircleMenu));               
        Selection.activeObject = go;
    }	
}
#endif