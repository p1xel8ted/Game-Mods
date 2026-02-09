// Decompiled with JetBrains decompiler
// Type: ListenerFollower
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using DarkTonic.MasterAudio;
using UnityEngine;

#nullable disable
[AudioScriptOrder(-10)]
public class ListenerFollower : MonoBehaviour
{
  public Transform _transToFollow;
  public GameObject _goToFollow;
  public Transform _trans;
  public GameObject _go;
  public SphereCollider _collider;

  public void Awake()
  {
    int num = (Object) this.Trigger == (Object) null ? 1 : 0;
  }

  public void StartFollowing(Transform transToFollow, string soundType, float trigRadius)
  {
    this._transToFollow = transToFollow;
    this._goToFollow = transToFollow.gameObject;
    this.Trigger.radius = trigRadius;
  }

  public void LateUpdate()
  {
    this.BatchOcclusionRaycasts();
    if ((Object) this._transToFollow == (Object) null || !DTMonoHelper.IsActive(this._goToFollow))
      return;
    this.Trans.position = this._transToFollow.position;
  }

  public void BatchOcclusionRaycasts()
  {
    if (!DarkTonic.MasterAudio.MasterAudio.Instance.useOcclusion)
      return;
    int num = 0;
    while (num < DarkTonic.MasterAudio.MasterAudio.Instance.occlusionMaxRayCastsPerFrame && DarkTonic.MasterAudio.MasterAudio.HasQueuedOcclusionRays())
    {
      SoundGroupVariationUpdater variationUpdater = DarkTonic.MasterAudio.MasterAudio.OldestQueuedOcclusionRay();
      if (!((Object) variationUpdater == (Object) null) && variationUpdater.enabled && variationUpdater.RayCastForOcclusion())
        ++num;
    }
  }

  public SphereCollider Trigger
  {
    get
    {
      if ((Object) this._collider != (Object) null)
        return this._collider;
      this._collider = this.GameObj.AddComponent<SphereCollider>();
      this._collider.isTrigger = true;
      return this._collider;
    }
  }

  public GameObject GameObj
  {
    get
    {
      if ((Object) this._go != (Object) null)
        return this._go;
      this._go = this.gameObject;
      return this._go;
    }
  }

  public Transform Trans
  {
    get
    {
      if ((Object) this._trans == (Object) null)
        this._trans = this.transform;
      return this._trans;
    }
  }
}
