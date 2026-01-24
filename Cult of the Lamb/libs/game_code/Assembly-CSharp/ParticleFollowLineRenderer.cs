// Decompiled with JetBrains decompiler
// Type: ParticleFollowLineRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class ParticleFollowLineRenderer : MonoBehaviour
{
  [SerializeField]
  public bool loop = true;
  [SerializeField]
  public bool invert;
  [SerializeField]
  public float speed = 5f;
  [SerializeField]
  public Vector2 timeBetween;
  [Space]
  [SerializeField]
  public GameObject particle;
  [SerializeField]
  public LineRenderer lineRenderer;
  public GameObject currentParticle;

  public void OnEnable()
  {
    if ((UnityEngine.Object) this.currentParticle != (UnityEngine.Object) null)
    {
      this.currentParticle.transform.DOKill();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.currentParticle.gameObject);
    }
    GameManager.GetInstance().WaitForSeconds(0.0f, new System.Action(this.SpawnParticle));
  }

  public void SpawnParticle()
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    this.currentParticle = UnityEngine.Object.Instantiate<GameObject>(this.particle, this.transform);
    Vector3[] vector3Array = new Vector3[this.lineRenderer.positionCount];
    this.lineRenderer.GetPositions(vector3Array);
    if (this.invert)
      vector3Array = ((IEnumerable<Vector3>) vector3Array).Reverse<Vector3>().ToArray<Vector3>();
    this.currentParticle.gameObject.SetActive(false);
    this.currentParticle.transform.position = vector3Array[0];
    AudioManager.Instance.PlayOneShot("event:/dlc/env/charged_shard_room/electric_current_pulse", this.currentParticle.transform.position);
    this.currentParticle.transform.DOPath(vector3Array, this.speed, gizmoColor: (Color?) new Color?()).SetSpeedBased<TweenerCore<Vector3, Path, PathOptions>>(true).SetDelay<TweenerCore<Vector3, Path, PathOptions>>(UnityEngine.Random.Range(this.timeBetween.x, this.timeBetween.y)).OnComplete<TweenerCore<Vector3, Path, PathOptions>>((TweenCallback) (() =>
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.currentParticle.gameObject);
      this.SpawnParticle();
    })).OnStart<TweenerCore<Vector3, Path, PathOptions>>((TweenCallback) (() => this.currentParticle.gameObject.SetActive(true)));
  }

  public void Disable()
  {
    if (!((UnityEngine.Object) this.currentParticle != (UnityEngine.Object) null))
      return;
    this.currentParticle.transform.DOKill();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.currentParticle.gameObject);
  }

  [CompilerGenerated]
  public void \u003CSpawnParticle\u003Eb__8_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.currentParticle.gameObject);
    this.SpawnParticle();
  }

  [CompilerGenerated]
  public void \u003CSpawnParticle\u003Eb__8_1() => this.currentParticle.gameObject.SetActive(true);
}
