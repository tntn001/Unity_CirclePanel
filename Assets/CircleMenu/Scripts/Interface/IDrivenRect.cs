using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public interface IDrivenRect 
{
    void StPos(Vector2 pos, bool _fd = true);
    void StSzDlt(Vector2 sz, bool _fd = true);
    void StAcrMx(Vector2 mx, bool _fd = true);
    void StAcMn(Vector2 mn, bool _fd = true);
    void StPv(Vector2 pv, bool _fd = true);
    void StRt(Vector3 rt, bool _fd = true, bool _isRt = true);
}
