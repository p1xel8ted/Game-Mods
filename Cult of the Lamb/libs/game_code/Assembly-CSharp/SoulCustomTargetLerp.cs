// Decompiled with JetBrains decompiler
// Type: SoulCustomTargetLerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SoulCustomTargetLerp : BaseMonoBehaviour
{
  public Vector3 StartingPosition;
  public GameObject Target;
  public System.Action Callback;
  public SpriteRenderer Image;
  public TrailRenderer Trail;
  public AnimationCurve MovementCurve;
  public AnimationCurve m_MovementCurve;
  public Vector3 MovementVector;
  public AnimationCurve HeightCurve;
  public float Progress;
  public float Duration = 0.5f;
  [CompilerGenerated]
  public Vector3 \u003COffset\u003Ek__BackingField = Vector3.zero;
  public static GameObject hitFX;
  public static GameObject SoulCustom;
  public static List<GameObject> pool;
  public static GameObject SoulCustomTargetLerpPool;

  public Vector3 Offset
  {
    get => this.\u003COffset\u003Ek__BackingField;
    set => this.\u003COffset\u003Ek__BackingField = value;
  }

  public static GameObject Create(
    GameObject Target,
    Vector3 position,
    float Duration,
    Color color,
    System.Action Callback = null,
    AnimationCurve _MovementCurve = null,
    string sfx = "event:/player/collect_black_soul")
  {
    if ((UnityEngine.Object) SoulCustomTargetLerp.hitFX == (UnityEngine.Object) null)
      SoulCustomTargetLerp.hitFX = Resources.Load("FX/HitFX/HitFX_Soul") as GameObject;
    if ((UnityEngine.Object) SoulCustomTargetLerp.SoulCustom == (UnityEngine.Object) null)
      SoulCustomTargetLerp.SoulCustom = Resources.Load("Prefabs/Resources/SoulCustomTarget Lerp") as GameObject;
    AudioManager.Instance.PlayOneShot(sfx, position);
    GameObject objectObjectFromPool = SoulCustomTargetLerp.GetObjectObjectFromPool(Target.transform.parent);
    objectObjectFromPool.transform.position = position;
    objectObjectFromPool.transform.eulerAngles = Vector3.zero;
    SoulCustomTargetLerp component = objectObjectFromPool.GetComponent<SoulCustomTargetLerp>();
    component.m_MovementCurve = _MovementCurve == null ? component.MovementCurve : _MovementCurve;
    component.Trail.Clear();
    component.Init(Target, position, color, Duration, Callback);
    objectObjectFromPool.SetActive(true);
    return objectObjectFromPool;
  }

  public static GameObject GetObjectObjectFromPool(Transform TargetParent)
  {
    if (SoulCustomTargetLerp.pool == null)
    {
      SoulCustomTargetLerp.pool = new List<GameObject>();
      SoulCustomTargetLerp.SoulCustomTargetLerpPool = new GameObject("SoulCustomTargetLerpPool");
      UnityEngine.Object.Instantiate<GameObject>(SoulCustomTargetLerp.SoulCustomTargetLerpPool, Vector3.zero, Quaternion.identity);
    }
    if (SoulCustomTargetLerp.pool.Count > 0)
    {
      for (int index = 0; index < SoulCustomTargetLerp.pool.Count; ++index)
      {
        if (!SoulCustomTargetLerp.pool[index].activeInHierarchy)
        {
          Debug.Log((object) "reused SoulCustomTargetLerp");
          SoulCustomTargetLerp.pool[index].transform.parent = TargetParent;
          return SoulCustomTargetLerp.pool[index];
        }
      }
    }
    GameObject objectObjectFromPool = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Resources/SoulCustomTarget Lerp") as GameObject, TargetParent, true);
    SoulCustomTargetLerp.pool.Add(objectObjectFromPool);
    return objectObjectFromPool;
  }

  public void Init(
    GameObject Target,
    Vector3 position,
    Color color,
    float Duration,
    System.Action Callback)
  {
    this.Progress = 0.0f;
    this.Target = Target;
    this.StartingPosition = position;
    this.Duration = Duration;
    this.Image.color = color;
    this.Image.transform.localScale = Vector3.one * 0.4f;
    this.Trail.startColor = color;
    this.Trail.endColor = new Color(color.r, color.g, color.b, 0.0f);
    this.Trail.Clear();
    this.Callback = Callback;
  }

  public void Update()
  {
    this.MovementVector = Vector3.LerpUnclamped(this.StartingPosition, this.Target.transform.position + this.Offset, this.m_MovementCurve.Evaluate((this.Progress += Time.deltaTime) / this.Duration));
    this.HeightCurve.keys[this.HeightCurve.keys.Length - 1].value = this.Target.transform.position.z + this.Offset.z;
    this.MovementVector.z = -this.HeightCurve.Evaluate(this.Progress / this.Duration);
    this.transform.position = this.MovementVector;
    if ((double) this.Progress / (double) this.Duration < 1.0)
      return;
    this.CollectMe();
  }

  public void CollectMe()
  {
    BiomeConstants.Instance.EmitHitVFXSoul(this.Image.gameObject.transform.position, Quaternion.identity);
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", this.Target);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    System.Action callback = this.Callback;
    if (callback != null)
      callback();
    this.Trail.Clear();
    this.ReturnToPool();
  }

  public void ReturnToPool()
  {
    this.Callback = (System.Action) null;
    this.gameObject.transform.parent = SoulCustomTargetLerp.SoulCustomTargetLerpPool.transform;
    this.gameObject.SetActive(false);
    this.Trail.Clear();
  }

  public void OnDestroy() => SoulCustomTargetLerp.pool = (List<GameObject>) null;
}
