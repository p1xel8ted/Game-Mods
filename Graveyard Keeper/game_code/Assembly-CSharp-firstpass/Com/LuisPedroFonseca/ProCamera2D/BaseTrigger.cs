// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.BaseTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public abstract class BaseTrigger : BasePC2D
{
  public Action OnEnteredTrigger;
  public Action OnExitedTrigger;
  [Tooltip("Every X seconds detect collision. Smaller intervals are more precise but also require more processing.")]
  public float UpdateInterval = 0.1f;
  public TriggerShape TriggerShape;
  [Tooltip("If enabled, use the targets mid point to know when inside/outside the trigger.")]
  public bool UseTargetsMidPoint = true;
  [Tooltip("If UseTargetsMidPoint is disabled, use this transform to know when inside/outside the trigger.")]
  public Transform TriggerTarget;
  public float _exclusiveInfluencePercentage;
  public Coroutine _testTriggerRoutine;
  public bool _insideTrigger;
  public Vector2 _vectorFromPointToCenter;
  public int _instanceID;
  public bool _triggerEnabled;

  public override void Awake()
  {
    base.Awake();
    if ((UnityEngine.Object) this.ProCamera2D == (UnityEngine.Object) null)
      return;
    this._instanceID = this.GetInstanceID();
    this.UpdateInterval += UnityEngine.Random.Range(-0.02f, 0.02f);
    this.Toggle(true);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    if (!this._triggerEnabled)
      return;
    this.Toggle(true);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this._testTriggerRoutine = (Coroutine) null;
  }

  public void Toggle(bool value)
  {
    if (value)
    {
      if (this._testTriggerRoutine == null)
        this._testTriggerRoutine = this.StartCoroutine(this.TestTriggerRoutine());
      this._triggerEnabled = true;
    }
    else
    {
      if (this._testTriggerRoutine != null)
      {
        this.StopCoroutine(this._testTriggerRoutine);
        this._testTriggerRoutine = (Coroutine) null;
      }
      this._triggerEnabled = false;
    }
  }

  public void TestTrigger()
  {
    Vector3 vector3 = this.ProCamera2D.TargetsMidPoint;
    if (!this.UseTargetsMidPoint && (UnityEngine.Object) this.TriggerTarget != (UnityEngine.Object) null)
      vector3 = this.TriggerTarget.position;
    if (this.TriggerShape == TriggerShape.RECTANGLE && Utils.IsInsideRectangle(this.Vector3H(this._transform.position), this.Vector3V(this._transform.position), this.Vector3H(this._transform.localScale), this.Vector3V(this._transform.localScale), this.Vector3H(vector3), this.Vector3V(vector3)))
    {
      if (this._insideTrigger)
        return;
      this.EnteredTrigger();
    }
    else if (this.TriggerShape == TriggerShape.CIRCLE && Utils.IsInsideCircle(this.Vector3H(this._transform.position), this.Vector3V(this._transform.position), (float) (((double) this.Vector3H(this._transform.localScale) + (double) this.Vector3V(this._transform.localScale)) * 0.25), this.Vector3H(vector3), this.Vector3V(vector3)))
    {
      if (this._insideTrigger)
        return;
      this.EnteredTrigger();
    }
    else
    {
      if (!this._insideTrigger)
        return;
      this.ExitedTrigger();
    }
  }

  public virtual void EnteredTrigger()
  {
    this._insideTrigger = true;
    if (this.OnEnteredTrigger == null)
      return;
    this.OnEnteredTrigger();
  }

  public virtual void ExitedTrigger()
  {
    this._insideTrigger = false;
    if (this.OnExitedTrigger == null)
      return;
    this.OnExitedTrigger();
  }

  public IEnumerator TestTriggerRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    WaitForSeconds waitForSeconds = new WaitForSeconds(this.UpdateInterval);
    while (true)
    {
      this.TestTrigger();
      yield return (object) waitForSeconds;
    }
  }

  public float GetDistanceToCenterPercentage(Vector2 point)
  {
    this._vectorFromPointToCenter = point - new Vector2(this.Vector3H(this._transform.position), this.Vector3V(this._transform.position));
    if (this.TriggerShape != TriggerShape.RECTANGLE)
      return (this._vectorFromPointToCenter.magnitude / (float) (((double) this.Vector3H(this._transform.localScale) + (double) this.Vector3V(this._transform.localScale)) * 0.25)).Remap(this._exclusiveInfluencePercentage, 1f, 0.0f, 1f);
    double f1 = (double) this.Vector3H((Vector3) this._vectorFromPointToCenter) / ((double) this.Vector3H(this._transform.localScale) * 0.5);
    float f2 = this.Vector3V((Vector3) this._vectorFromPointToCenter) / (this.Vector3V(this._transform.localScale) * 0.5f);
    return Mathf.Max(Mathf.Abs((float) f1), Mathf.Abs(f2)).Remap(this._exclusiveInfluencePercentage, 1f, 0.0f, 1f);
  }
}
