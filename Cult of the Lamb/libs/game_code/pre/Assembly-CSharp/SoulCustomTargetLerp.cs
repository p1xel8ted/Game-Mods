// Decompiled with JetBrains decompiler
// Type: SoulCustomTargetLerp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SoulCustomTargetLerp : BaseMonoBehaviour
{
  private Vector3 StartingPosition;
  private GameObject Target;
  private System.Action Callback;
  public SpriteRenderer Image;
  public TrailRenderer Trail;
  public AnimationCurve MovementCurve;
  private Vector3 MovementVector;
  public AnimationCurve HeightCurve;
  private float Progress;
  public float Duration = 0.5f;
  public static GameObject hitFX;
  public static GameObject SoulCustom;
  public static List<GameObject> pool;
  private static GameObject SoulCustomTargetLerpPool;

  public Vector3 Offset { get; set; } = Vector3.zero;

  public static GameObject Create(
    GameObject Target,
    Vector3 position,
    float Duration,
    Color color,
    System.Action Callback = null)
  {
    if ((UnityEngine.Object) SoulCustomTargetLerp.hitFX == (UnityEngine.Object) null)
      SoulCustomTargetLerp.hitFX = Resources.Load("FX/HitFX/HitFX_Soul") as GameObject;
    if ((UnityEngine.Object) SoulCustomTargetLerp.SoulCustom == (UnityEngine.Object) null)
      SoulCustomTargetLerp.SoulCustom = Resources.Load("Prefabs/Resources/SoulCustomTarget Lerp") as GameObject;
    AudioManager.Instance.PlayOneShot("event:/player/collect_black_soul", position);
    GameObject objectObjectFromPool = SoulCustomTargetLerp.GetObjectObjectFromPool(Target.transform.parent);
    objectObjectFromPool.transform.position = position;
    objectObjectFromPool.transform.eulerAngles = Vector3.zero;
    SoulCustomTargetLerp component = objectObjectFromPool.GetComponent<SoulCustomTargetLerp>();
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

  private void Update()
  {
    this.MovementVector = Vector3.LerpUnclamped(this.StartingPosition, this.Target.transform.position + this.Offset, this.MovementCurve.Evaluate((this.Progress += Time.deltaTime) / this.Duration));
    this.HeightCurve.keys[this.HeightCurve.keys.Length - 1].value = this.Target.transform.position.z + this.Offset.z;
    this.MovementVector.z = -this.HeightCurve.Evaluate(this.Progress / this.Duration);
    this.transform.position = this.MovementVector;
    if ((double) this.Progress / (double) this.Duration < 1.0)
      return;
    this.CollectMe();
  }

  private void CollectMe()
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

  private void ReturnToPool()
  {
    this.Callback = (System.Action) null;
    this.gameObject.transform.parent = SoulCustomTargetLerp.SoulCustomTargetLerpPool.transform;
    this.gameObject.SetActive(false);
    this.Trail.Clear();
  }

  private void OnDestroy() => SoulCustomTargetLerp.pool = (List<GameObject>) null;
}
