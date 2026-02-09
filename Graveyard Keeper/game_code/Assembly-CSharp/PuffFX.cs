// Decompiled with JetBrains decompiler
// Type: PuffFX
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PuffFX : MonoBehaviour
{
  public static PuffFX _prefab;
  public GameObject small;
  public GameObject large;
  [NonSerialized]
  public WorldGameObject _linked_wgo;

  public static PuffFX Create(WorldGameObject wgo, Bounds? wgo_bounds = null)
  {
    try
    {
      if (!((UnityEngine.Object) wgo == (UnityEngine.Object) null))
      {
        if (!((UnityEngine.Object) wgo.gameObject == (UnityEngine.Object) null))
          goto label_4;
      }
      Debug.LogError((object) "PuffFX.Create error: WGO is null");
      return (PuffFX) null;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("PuffFX.Create error: exception: " + ex?.ToString()));
      return (PuffFX) null;
    }
label_4:
    if ((UnityEngine.Object) PuffFX._prefab == (UnityEngine.Object) null)
      PuffFX._prefab = Resources.Load<PuffFX>("puff fx");
    PuffFX puffFx = PuffFX._prefab.Copy<PuffFX>();
    puffFx._linked_wgo = wgo;
    puffFx.transform.SetParent(wgo.gameObject.transform, false);
    PuffPoint componentInChildren = wgo.GetComponentInChildren<PuffPoint>(true);
    bool flag;
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
    {
      puffFx.transform.SetParent(componentInChildren.transform, false);
      puffFx.transform.localPosition = Vector3.zero;
      flag = componentInChildren.size == PuffFX.PuffSize.Small;
    }
    else
    {
      if (!wgo_bounds.HasValue)
        wgo_bounds = new Bounds?(wgo.GetTotalBounds());
      Bounds bounds = wgo_bounds.Value;
      Vector3 center = bounds.center;
      puffFx.transform.position = center;
      bounds = wgo_bounds.Value;
      flag = (double) (new Vector2(Mathf.Round(bounds.size.x), Mathf.Round(wgo_bounds.Value.size.y)) / 96f).magnitude <= 1.0;
    }
    puffFx.small.gameObject.SetActive(flag);
    puffFx.large.gameObject.SetActive(!flag);
    puffFx.transform.localPosition = new Vector3(puffFx.transform.localPosition.x, puffFx.transform.localPosition.y, 0.0f);
    puffFx.transform.SetParent(MainGame.me.world_root);
    return puffFx;
  }

  public void Start()
  {
    GJTimer.AddTimer(2f, (GJTimer.VoidDelegate) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__6_0() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [Serializable]
  public enum PuffSize
  {
    Small,
    Large,
  }
}
