// Decompiled with JetBrains decompiler
// Type: PropertyBinding
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
  public PropertyReference source;
  public PropertyReference target;
  public PropertyBinding.Direction direction;
  public PropertyBinding.UpdateCondition update = PropertyBinding.UpdateCondition.OnUpdate;
  public bool editMode = true;
  public object mLastValue;

  public void Start()
  {
    this.UpdateTarget();
    if (this.update != PropertyBinding.UpdateCondition.OnStart)
      return;
    this.enabled = false;
  }

  public void Update()
  {
    if (this.update != PropertyBinding.UpdateCondition.OnUpdate)
      return;
    this.UpdateTarget();
  }

  public void LateUpdate()
  {
    if (this.update != PropertyBinding.UpdateCondition.OnLateUpdate)
      return;
    this.UpdateTarget();
  }

  public void FixedUpdate()
  {
    if (this.update != PropertyBinding.UpdateCondition.OnFixedUpdate)
      return;
    this.UpdateTarget();
  }

  public void OnValidate()
  {
    if (this.source != null)
      this.source.Reset();
    if (this.target == null)
      return;
    this.target.Reset();
  }

  [ContextMenu("Update Now")]
  public void UpdateTarget()
  {
    if (this.source == null || this.target == null || !this.source.isValid || !this.target.isValid)
      return;
    if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
      this.target.Set(this.source.Get());
    else if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
    {
      this.source.Set(this.target.Get());
    }
    else
    {
      if (!System.Type.op_Equality(this.source.GetPropertyType(), this.target.GetPropertyType()))
        return;
      object obj1 = this.source.Get();
      if (this.mLastValue == null || !this.mLastValue.Equals(obj1))
      {
        this.mLastValue = obj1;
        this.target.Set(obj1);
      }
      else
      {
        object obj2 = this.target.Get();
        if (this.mLastValue.Equals(obj2))
          return;
        this.mLastValue = obj2;
        this.source.Set(obj2);
      }
    }
  }

  public enum UpdateCondition
  {
    OnStart,
    OnUpdate,
    OnLateUpdate,
    OnFixedUpdate,
  }

  public enum Direction
  {
    SourceUpdatesTarget,
    TargetUpdatesSource,
    BiDirectional,
  }
}
