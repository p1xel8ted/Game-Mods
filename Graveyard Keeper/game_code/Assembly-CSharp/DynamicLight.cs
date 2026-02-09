// Decompiled with JetBrains decompiler
// Type: DynamicLight
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DynamicLight : MonoBehaviour
{
  public GameObject light_zero_point;
  public bool _tf_set;
  public Transform _tf;
  public Vector3 pos = Vector3.zero;
  public bool active_in_hierarchy = true;
  [Range(0.0f, 2f)]
  public float intensity_k = 1f;
  public int _parent_go = -1;
  public bool _parent_go_set;
  public DynamicSpritePreset intensity_preset;

  public void OnEnable()
  {
    DynamicLights.dyn_lights.Add(this);
    this.CustomUpdate();
  }

  public void OnDisable()
  {
    DynamicLights.dyn_lights.Remove(this);
    this.active_in_hierarchy = false;
  }

  public void CustomUpdate()
  {
    if (!this._tf_set)
    {
      this._tf = (Object) this.light_zero_point == (Object) null ? this.transform : this.light_zero_point.transform;
      this._tf_set = true;
    }
    this.pos = this._tf.position;
    this.active_in_hierarchy = this.gameObject.activeInHierarchy;
  }

  public bool DoesLightBelongsToTheSameObjectAsShadow(ObjectDynamicShadow shadow)
  {
    if (!this._parent_go_set)
    {
      this._parent_go_set = true;
      WorldGameObject componentInParent1 = this.GetComponentInParent<WorldGameObject>();
      if ((Object) componentInParent1 != (Object) null)
      {
        this._parent_go = componentInParent1.gameObject.GetInstanceID();
      }
      else
      {
        WorldSimpleObject componentInParent2 = this.GetComponentInParent<WorldSimpleObject>();
        if ((Object) componentInParent2 != (Object) null)
          this._parent_go = componentInParent2.gameObject.GetInstanceID();
      }
    }
    return this._parent_go == shadow.ParentGoInstanceIDInstanceID;
  }
}
