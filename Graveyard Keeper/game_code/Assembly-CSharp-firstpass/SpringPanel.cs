// Decompiled with JetBrains decompiler
// Type: SpringPanel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Internal/Spring Panel")]
[RequireComponent(typeof (UIPanel))]
public class SpringPanel : MonoBehaviour
{
  public static SpringPanel current;
  public Vector3 target = Vector3.zero;
  public float strength = 10f;
  public SpringPanel.OnFinished onFinished;
  public UIPanel mPanel;
  public Transform mTrans;
  public UIScrollView mDrag;

  public void Start()
  {
    this.mPanel = this.GetComponent<UIPanel>();
    this.mDrag = this.GetComponent<UIScrollView>();
    this.mTrans = this.transform;
  }

  public void Update() => this.AdvanceTowardsPosition();

  public virtual void AdvanceTowardsPosition()
  {
    float deltaTime = RealTime.deltaTime;
    bool flag = false;
    Vector3 localPosition = this.mTrans.localPosition;
    Vector3 vector3_1 = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
    if ((double) (vector3_1 - this.target).sqrMagnitude < 0.0099999997764825821)
    {
      vector3_1 = this.target;
      this.enabled = false;
      flag = true;
    }
    this.mTrans.localPosition = vector3_1;
    Vector3 vector3_2 = vector3_1 - localPosition;
    Vector2 clipOffset = this.mPanel.clipOffset;
    clipOffset.x -= vector3_2.x;
    clipOffset.y -= vector3_2.y;
    this.mPanel.clipOffset = clipOffset;
    if ((Object) this.mDrag != (Object) null)
      this.mDrag.UpdateScrollbars(false);
    if (!flag || this.onFinished == null)
      return;
    SpringPanel.current = this;
    this.onFinished();
    SpringPanel.current = (SpringPanel) null;
  }

  public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
  {
    SpringPanel springPanel = go.GetComponent<SpringPanel>();
    if ((Object) springPanel == (Object) null)
      springPanel = go.AddComponent<SpringPanel>();
    springPanel.target = pos;
    springPanel.strength = strength;
    springPanel.onFinished = (SpringPanel.OnFinished) null;
    springPanel.enabled = true;
    return springPanel;
  }

  public delegate void OnFinished();
}
