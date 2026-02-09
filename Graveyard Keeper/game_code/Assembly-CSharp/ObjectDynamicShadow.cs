// Decompiled with JetBrains decompiler
// Type: ObjectDynamicShadow
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
[ExecuteInEditMode]
public class ObjectDynamicShadow : ObjectDynamicShadowChild
{
  public const bool CREATE_SHADOWS_ON_A_FLY = false;
  public const int LIGHTS_ARRAY_SIZE = 20;
  public List<ObjectDynamicShadowChild> _child_shadows = new List<ObjectDynamicShadowChild>();
  public DynamicLight[] _lights = new DynamicLight[20];
  public float[] _distances = new float[20];
  public int _lights_count;
  public Transform light_point;
  public bool _light_point_is_null = true;
  public Vector2?[] _lposes = new Vector2?[4];
  public float[] _ldist = new float[4];
  public Vector2 _last_pos = new Vector2(99999f, 99999f);
  [NonSerialized]
  public Vector3 point_pos = Vector3.zero;
  public bool _shadows_initialized;
  public bool _shadows_init_was_force_immidiate;
  public bool _shadows_created;
  public bool _shadows_really_created;
  public int _parent_go_instance_id = -1;
  public bool _parent_go_set;
  public System.Action _queued_shadows_action;
  public bool _destroyed;

  public void Start()
  {
    this._light_point_is_null = (UnityEngine.Object) this.light_point == (UnityEngine.Object) null;
    this._lights_count = 0;
  }

  public void Update()
  {
    if (Application.isPlaying && !this.visible)
      return;
    this.point_pos = this._light_point_is_null ? this.transform.position : this.light_point.position;
    Vector2 pos = this.Recalculate();
    this._last_pos = pos;
    for (int index = 0; index < this._child_shadows.Count; ++index)
    {
      ObjectDynamicShadowChild childShadow = this._child_shadows[index];
      childShadow.SetShadowByNumber(this, childShadow.shadow_n, pos, this.is_mirrored);
    }
  }

  public Vector2? GetLight(int n) => this._lposes[n];

  public void SortLightsList()
  {
    for (int index = 0; index < 4; ++index)
    {
      this._lposes[index] = new Vector2?();
      this._ldist[index] = float.MaxValue;
    }
    for (int index1 = 0; index1 < this._lights_count && index1 <= 20; ++index1)
    {
      if (!((UnityEngine.Object) this._lights[index1] == (UnityEngine.Object) null))
      {
        DynamicLight light = this._lights[index1];
        if (!Application.isPlaying)
          light.pos = light.transform.position;
        Vector2 pos = (Vector2) light.pos;
        if (!Application.isPlaying)
        {
          if (this._distances == null || index1 >= this._distances.Length)
            break;
          this._distances[index1] = (pos - (Vector2) this.point_pos).sqrMagnitude;
        }
        float num = this._distances[index1] / light.intensity_k;
        if ((double) num <= 1000000.0)
        {
          for (int index2 = 0; index2 < 4; ++index2)
          {
            if ((double) num <= (double) this._ldist[index2])
            {
              for (int index3 = 3; index3 > index2; --index3)
              {
                this._ldist[index3] = this._ldist[index3 - 1];
                this._lposes[index3] = this._lposes[index3 - 1];
              }
              this._ldist[index2] = num;
              this._lposes[index2] = new Vector2?(pos);
              break;
            }
          }
        }
      }
    }
  }

  public void ClearLightsList() => this._lights_count = 0;

  public void LateUpdate()
  {
    bool isMirrored = this.is_mirrored;
    this.is_mirrored = (double) this.transform.lossyScale.x < 0.0;
    if (this.is_mirrored == isMirrored)
      return;
    this.Update();
  }

  public Vector2 Recalculate()
  {
    if (Application.isPlaying && !this.visible)
      return Vector2.zero;
    if (!Application.isPlaying)
    {
      this.point_pos = (UnityEngine.Object) this.light_point == (UnityEngine.Object) null ? this.transform.position : this.light_point.position;
      if ((UnityEngine.Object) DynamicShadows.me != (UnityEngine.Object) null)
      {
        this._lights = DynamicShadows.me.lights.ToArray();
        this._lights_count = this._lights.Length;
      }
    }
    this.SortLightsList();
    return (Vector2) this.point_pos;
  }

  public static void InstantiateAllAdditionalShadows(int total_n = 4)
  {
  }

  public void InstantiateAdditionalShadows(int total_n, bool force_immediate = false)
  {
    if (force_immediate || !Application.isPlaying)
    {
      this.ReallyInstantiateAdditionalShadows(total_n);
    }
    else
    {
      if (this._shadows_created || this._queued_shadows_action != null)
        return;
      this._queued_shadows_action = (System.Action) (() => this.ReallyInstantiateAdditionalShadows(total_n));
      ObjectDynamicShadowsManager.QueueShadowCreation(this._queued_shadows_action, (System.Action) (() => this._queued_shadows_action = (System.Action) null));
    }
  }

  public void ReallyInstantiateAdditionalShadows(int total_n)
  {
    if (this._destroyed || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || this._shadows_really_created)
      return;
    this._shadows_initialized = true;
    this._shadows_created = true;
    this._child_shadows.Clear();
    this._lights_count = 0;
    this._shadows_really_created = true;
    foreach (ObjectDynamicShadowChild componentsInChild in this.gameObject.GetComponentsInChildren<ObjectDynamicShadowChild>(true))
    {
      if (!(componentsInChild is ObjectDynamicShadow))
      {
        componentsInChild.transform.SetParent((Transform) null, false);
        NGUITools.Destroy((UnityEngine.Object) componentsInChild.gameObject);
      }
    }
    SpriteRenderer component1 = this.GetComponent<SpriteRenderer>();
    for (int index = 0; index < total_n; ++index)
    {
      ObjectDynamicShadowChild shadow = ShadowsPool.GetShadow();
      GameObject gameObject = shadow.gameObject;
      gameObject.name = "[dynamic shadow] #" + index.ToString();
      SpriteRenderer component2 = gameObject.GetComponent<SpriteRenderer>();
      component2.sprite = component1.sprite;
      component2.sharedMaterial = component1.sharedMaterial;
      component2.sortingLayerID = component1.sortingLayerID;
      component2.color = new Color(1f, 1f, 1f, 0.0f);
      this._child_shadows.Add(shadow);
      shadow.shadow_n = index;
      shadow.shadow_alpha = this.shadow_alpha;
      gameObject.transform.SetParent(this.transform, false);
      gameObject.transform.localScale = Vector3.one;
      gameObject.transform.localPosition = Vector3.zero;
    }
    this.GetComponent<SpriteRenderer>().enabled = false;
    this.gameObject.layer = 12;
    this.shadow_n = -1;
  }

  public void CheckLightsRange(List<DynamicLight> lights)
  {
    int index1 = 0;
    for (int index2 = 0; index2 < lights.Count; ++index2)
    {
      DynamicLight light = lights[index2];
      if (!((UnityEngine.Object) light == (UnityEngine.Object) null) && light.active_in_hierarchy)
      {
        double num1 = (double) light.pos.x - (double) this.point_pos.x;
        float num2 = light.pos.y - this.point_pos.y;
        float num3 = (float) (num1 * num1 + (double) num2 * (double) num2);
        if ((double) num3 <= 1000000.0 && !light.DoesLightBelongsToTheSameObjectAsShadow(this))
        {
          this._lights[index1] = light;
          this._distances[index1] = num3;
          ++index1;
          if (index1 >= 20)
            break;
        }
      }
    }
    this._lights_count = index1;
  }

  public override void SetShadowSprite(UnityEngine.Sprite spr)
  {
    base.SetShadowSprite(spr);
    foreach (ObjectDynamicShadowChild childShadow in this._child_shadows)
      childShadow.SetShadowSprite(spr);
  }

  public void EnsureObjectHasShadows(bool force_immediate = false)
  {
    if (!Application.isPlaying || this._shadows_initialized && (!force_immediate || this._shadows_init_was_force_immidiate || this._shadows_really_created))
      return;
    this._shadows_initialized = true;
    this._shadows_init_was_force_immidiate = force_immediate;
    if (this.shadow_n != 0)
      return;
    this.InstantiateAdditionalShadows(4, force_immediate);
  }

  public void Awake()
  {
    if (!Application.isPlaying || this._shadows_initialized)
      return;
    this.EnsureObjectHasShadows();
  }

  public void OnEnable()
  {
    if (Application.isPlaying && !this._shadows_initialized)
      this.EnsureObjectHasShadows();
    DynamicLights.shadows.Add(this);
  }

  public void OnDisable() => DynamicLights.shadows.Remove(this);

  public int ParentGoInstanceIDInstanceID
  {
    get
    {
      if (!this._parent_go_set)
      {
        this._parent_go_set = true;
        WorldGameObject componentInParent1 = this.GetComponentInParent<WorldGameObject>();
        if ((UnityEngine.Object) componentInParent1 != (UnityEngine.Object) null)
        {
          this._parent_go_instance_id = componentInParent1.gameObject.GetInstanceID();
        }
        else
        {
          WorldSimpleObject componentInParent2 = this.GetComponentInParent<WorldSimpleObject>();
          if ((UnityEngine.Object) componentInParent2 != (UnityEngine.Object) null)
            this._parent_go_instance_id = componentInParent2.gameObject.GetInstanceID();
        }
      }
      return this._parent_go_instance_id;
    }
  }

  public void OnBecameVisible()
  {
    if (this._shadows_created || !Application.isPlaying)
      return;
    if (this._queued_shadows_action != null)
      ObjectDynamicShadowsManager.ForceShadowAction(this._queued_shadows_action);
    else
      this.ReallyInstantiateAdditionalShadows(4);
  }

  public static void InitOnGameStart()
  {
  }

  public void OnDestroy() => this._destroyed = true;
}
