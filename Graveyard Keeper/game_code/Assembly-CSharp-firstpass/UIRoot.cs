// Decompiled with JetBrains decompiler
// Type: UIRoot
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Root")]
public class UIRoot : MonoBehaviour
{
  public static List<UIRoot> list = new List<UIRoot>();
  public UIRoot.Scaling scalingStyle;
  public int manualWidth = 1280 /*0x0500*/;
  public int manualHeight = 720;
  public int minimumHeight = 320;
  public int maximumHeight = 1536 /*0x0600*/;
  public bool fitWidth;
  public bool fitHeight = true;
  public bool adjustByDPI;
  public bool shrinkPortraitUI;
  public Transform mTrans;

  public UIRoot.Constraint constraint
  {
    get
    {
      return this.fitWidth ? (this.fitHeight ? UIRoot.Constraint.Fit : UIRoot.Constraint.FitWidth) : (this.fitHeight ? UIRoot.Constraint.FitHeight : UIRoot.Constraint.Fill);
    }
  }

  public UIRoot.Scaling activeScaling
  {
    get
    {
      UIRoot.Scaling scalingStyle = this.scalingStyle;
      return scalingStyle == UIRoot.Scaling.ConstrainedOnMobiles ? UIRoot.Scaling.Flexible : scalingStyle;
    }
  }

  public int activeHeight
  {
    get
    {
      if (this.activeScaling == UIRoot.Scaling.Flexible)
      {
        Vector2 screenSize = NGUITools.screenSize;
        float num = screenSize.x / screenSize.y;
        if ((double) screenSize.y < (double) this.minimumHeight)
        {
          screenSize.y = (float) this.minimumHeight;
          screenSize.x = screenSize.y * num;
        }
        else if ((double) screenSize.y > (double) this.maximumHeight)
        {
          screenSize.y = (float) this.maximumHeight;
          screenSize.x = screenSize.y * num;
        }
        int height = Mathf.RoundToInt(!this.shrinkPortraitUI || (double) screenSize.y <= (double) screenSize.x ? screenSize.y : screenSize.y / num);
        return !this.adjustByDPI ? height : NGUIMath.AdjustByDPI((float) height);
      }
      UIRoot.Constraint constraint = this.constraint;
      if (constraint == UIRoot.Constraint.FitHeight)
        return this.manualHeight;
      Vector2 screenSize1 = NGUITools.screenSize;
      float num1 = screenSize1.x / screenSize1.y;
      float num2 = (float) this.manualWidth / (float) this.manualHeight;
      switch (constraint)
      {
        case UIRoot.Constraint.Fit:
          return (double) num2 <= (double) num1 ? this.manualHeight : Mathf.RoundToInt((float) this.manualWidth / num1);
        case UIRoot.Constraint.Fill:
          return (double) num2 >= (double) num1 ? this.manualHeight : Mathf.RoundToInt((float) this.manualWidth / num1);
        case UIRoot.Constraint.FitWidth:
          return Mathf.RoundToInt((float) this.manualWidth / num1);
        default:
          return this.manualHeight;
      }
    }
  }

  public float pixelSizeAdjustment
  {
    get
    {
      int height = Mathf.RoundToInt(NGUITools.screenSize.y);
      return height != -1 ? this.GetPixelSizeAdjustment(height) : 1f;
    }
  }

  public static float GetPixelSizeAdjustment(GameObject go)
  {
    UIRoot inParents = NGUITools.FindInParents<UIRoot>(go);
    return !((Object) inParents != (Object) null) ? 1f : inParents.pixelSizeAdjustment;
  }

  public float GetPixelSizeAdjustment(int height)
  {
    height = Mathf.Max(2, height);
    if (this.activeScaling == UIRoot.Scaling.Constrained)
      return (float) this.activeHeight / (float) height;
    if (height < this.minimumHeight)
      return (float) this.minimumHeight / (float) height;
    return height > this.maximumHeight ? (float) this.maximumHeight / (float) height : 1f;
  }

  public virtual void Awake() => this.mTrans = this.transform;

  public virtual void OnEnable() => UIRoot.list.Add(this);

  public virtual void OnDisable() => UIRoot.list.Remove(this);

  public virtual void Start()
  {
    UIOrthoCamera componentInChildren = this.GetComponentInChildren<UIOrthoCamera>();
    if ((Object) componentInChildren != (Object) null)
    {
      Debug.LogWarning((object) "UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", (Object) componentInChildren);
      Camera component = componentInChildren.gameObject.GetComponent<Camera>();
      componentInChildren.enabled = false;
      if (!((Object) component != (Object) null))
        return;
      component.orthographicSize = 1f;
    }
    else
      this.UpdateScale(false);
  }

  public void Update() => this.UpdateScale();

  public void UpdateScale(bool updateAnchors = true)
  {
    if (!((Object) this.mTrans != (Object) null))
      return;
    float activeHeight = (float) this.activeHeight;
    if ((double) activeHeight <= 0.0)
      return;
    float num = 2f / activeHeight;
    Vector3 localScale = this.mTrans.localScale;
    if ((double) Mathf.Abs(localScale.x - num) <= 1.4012984643248171E-45 && (double) Mathf.Abs(localScale.y - num) <= 1.4012984643248171E-45 && (double) Mathf.Abs(localScale.z - num) <= 1.4012984643248171E-45)
      return;
    this.mTrans.localScale = new Vector3(num, num, num);
    if (!updateAnchors)
      return;
    this.BroadcastMessage("UpdateAnchors", SendMessageOptions.DontRequireReceiver);
  }

  public static void Broadcast(string funcName)
  {
    int index = 0;
    for (int count = UIRoot.list.Count; index < count; ++index)
    {
      UIRoot uiRoot = UIRoot.list[index];
      if ((Object) uiRoot != (Object) null)
        uiRoot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
    }
  }

  public static void Broadcast(string funcName, object param)
  {
    if (param == null)
    {
      Debug.LogError((object) "SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
    }
    else
    {
      int index = 0;
      for (int count = UIRoot.list.Count; index < count; ++index)
      {
        UIRoot uiRoot = UIRoot.list[index];
        if ((Object) uiRoot != (Object) null)
          uiRoot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
      }
    }
  }

  public enum Scaling
  {
    Flexible,
    Constrained,
    ConstrainedOnMobiles,
  }

  public enum Constraint
  {
    Fit,
    Fill,
    FitWidth,
    FitHeight,
  }
}
