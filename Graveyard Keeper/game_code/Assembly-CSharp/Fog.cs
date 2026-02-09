// Decompiled with JetBrains decompiler
// Type: Fog
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Fog : AbstractControllerComponent
{
  public static Fog me;
  public List<FogObject> _objs = new List<FogObject>();
  public float _cur_amount;
  public float _target_amount;
  public Material _mat;
  public GameObject _sub_go;
  public float speed = 0.2f;

  public static Transform SpawnNewFog()
  {
    GameObject gameObject = new GameObject(nameof (Fog));
    Fog.me = gameObject.AddComponent<Fog>();
    gameObject.transform.SetParent(MainGame.me.world_root.transform, false);
    Fog.me._sub_go = new GameObject("fog objects");
    Fog.me._sub_go.transform.SetParent(gameObject.transform, false);
    return Fog.me._sub_go.transform;
  }

  public void OnNewFogObjectCreated(FogObject fo)
  {
    if (this._objs.Count == 0)
    {
      this._mat = fo.GetComponent<SpriteRenderer>().sharedMaterial;
      this.ApplyCurrentAmount();
    }
    this._objs.Add(fo);
  }

  public void Update()
  {
    this._cur_amount = this._target_amount;
    this.ApplyCurrentAmount();
  }

  public void ApplyCurrentAmount()
  {
    this._sub_go.SetActive((double) this._cur_amount > 0.01);
    this._mat.SetFloat("_Multiply", this._cur_amount);
  }

  public override void Set(float a) => this._target_amount = a;
}
