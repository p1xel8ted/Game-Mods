// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.ProCamera2DTriggerInfluence
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

[HelpURL("http://www.procamera2d.com/user-guide/trigger-influence/")]
public class ProCamera2DTriggerInfluence : BaseTrigger
{
  public static string TriggerName = "Influence Trigger";
  public Transform FocusPoint;
  public float InfluenceSmoothness = 0.3f;
  [Range(0.0f, 1f)]
  public float ExclusiveInfluencePercentage = 0.25f;
  public Vector2 _influence;
  public Vector2 _velocity;
  public Vector3 _exclusivePointVelocity;
  public Vector3 _tempExclusivePoint;

  public void Start()
  {
    if ((Object) this.FocusPoint == (Object) null)
      this.FocusPoint = this.transform.Find("FocusPoint");
    if (!((Object) this.FocusPoint == (Object) null))
      return;
    this.FocusPoint = this.transform;
  }

  public override void EnteredTrigger()
  {
    base.EnteredTrigger();
    this.StartCoroutine(this.InsideTriggerRoutine());
  }

  public override void ExitedTrigger()
  {
    base.ExitedTrigger();
    this.StartCoroutine(this.OutsideTriggerRoutine());
  }

  public IEnumerator InsideTriggerRoutine()
  {
    ProCamera2DTriggerInfluence dtriggerInfluence = this;
    yield return (object) dtriggerInfluence.ProCamera2D.GetYield();
    float previousDistancePercentage = 1f;
    dtriggerInfluence._tempExclusivePoint = dtriggerInfluence.VectorHV(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.transform.position), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.transform.position));
    while (dtriggerInfluence._insideTrigger)
    {
      dtriggerInfluence._exclusiveInfluencePercentage = dtriggerInfluence.ExclusiveInfluencePercentage;
      float centerPercentage = dtriggerInfluence.GetDistanceToCenterPercentage(new Vector2(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.TargetsMidPoint), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.TargetsMidPoint)));
      Vector2 vector2 = new Vector2(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.TargetsMidPoint) + dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.TargetsMidPoint) - dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.PreviousTargetsMidPoint), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.TargetsMidPoint) + dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.TargetsMidPoint) - dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.PreviousTargetsMidPoint)) - new Vector2(dtriggerInfluence.Vector3H(dtriggerInfluence.FocusPoint.position), dtriggerInfluence.Vector3V(dtriggerInfluence.FocusPoint.position));
      if ((double) centerPercentage == 0.0)
      {
        dtriggerInfluence.ProCamera2D.ExclusiveTargetPosition = new Vector3?(Vector3.SmoothDamp(dtriggerInfluence._tempExclusivePoint, dtriggerInfluence.VectorHV(dtriggerInfluence.Vector3H(dtriggerInfluence.FocusPoint.position), dtriggerInfluence.Vector3V(dtriggerInfluence.FocusPoint.position)), ref dtriggerInfluence._exclusivePointVelocity, dtriggerInfluence.InfluenceSmoothness));
        dtriggerInfluence._tempExclusivePoint = dtriggerInfluence.ProCamera2D.ExclusiveTargetPosition.Value;
        dtriggerInfluence._influence = -vector2 * (1f - centerPercentage);
        dtriggerInfluence.ProCamera2D.ApplyInfluence(dtriggerInfluence._influence);
      }
      else
      {
        if ((double) previousDistancePercentage == 0.0)
          dtriggerInfluence._influence = new Vector2(dtriggerInfluence.Vector3H((Vector3) dtriggerInfluence.ProCamera2D.CameraTargetPositionSmoothed), dtriggerInfluence.Vector3V((Vector3) dtriggerInfluence.ProCamera2D.CameraTargetPositionSmoothed)) - new Vector2(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.TargetsMidPoint) + dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.TargetsMidPoint) - dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.PreviousTargetsMidPoint), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.TargetsMidPoint) + dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.TargetsMidPoint) - dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.PreviousTargetsMidPoint)) + new Vector2(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.ParentPosition), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.ParentPosition));
        dtriggerInfluence._influence = Vector2.SmoothDamp(dtriggerInfluence._influence, -vector2 * (1f - centerPercentage), ref dtriggerInfluence._velocity, dtriggerInfluence.InfluenceSmoothness, float.PositiveInfinity, Time.deltaTime);
        dtriggerInfluence.ProCamera2D.ApplyInfluence(dtriggerInfluence._influence);
        dtriggerInfluence._tempExclusivePoint = dtriggerInfluence.VectorHV(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.CameraTargetPosition), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.CameraTargetPosition)) + dtriggerInfluence.VectorHV(dtriggerInfluence.Vector3H(dtriggerInfluence.ProCamera2D.ParentPosition), dtriggerInfluence.Vector3V(dtriggerInfluence.ProCamera2D.ParentPosition));
      }
      previousDistancePercentage = centerPercentage;
      yield return (object) dtriggerInfluence.ProCamera2D.GetYield();
    }
  }

  public IEnumerator OutsideTriggerRoutine()
  {
    ProCamera2DTriggerInfluence dtriggerInfluence = this;
    yield return (object) dtriggerInfluence.ProCamera2D.GetYield();
    while (!dtriggerInfluence._insideTrigger && dtriggerInfluence._influence != Vector2.zero)
    {
      dtriggerInfluence._influence = Vector2.SmoothDamp(dtriggerInfluence._influence, Vector2.zero, ref dtriggerInfluence._velocity, dtriggerInfluence.InfluenceSmoothness, float.PositiveInfinity, Time.deltaTime);
      dtriggerInfluence.ProCamera2D.ApplyInfluence(dtriggerInfluence._influence);
      yield return (object) dtriggerInfluence.ProCamera2D.GetYield();
    }
  }
}
