// Decompiled with JetBrains decompiler
// Type: CustomNetworkBehaviour
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CustomNetworkBehaviour : MonoBehaviour
{
  public List<CustomNetworkBehaviour.DelegateVoid> _dlg_void = new List<CustomNetworkBehaviour.DelegateVoid>();
  public List<CustomNetworkBehaviour.DelegateFloat> _dlg_float = new List<CustomNetworkBehaviour.DelegateFloat>();
  public List<CustomNetworkBehaviour.DelegateWGOFloat> _dlg_wgo_float = new List<CustomNetworkBehaviour.DelegateWGOFloat>();

  public void NetRegisterDelegate(CustomNetworkBehaviour.DelegateVoid dlg)
  {
    this._dlg_void.Add(dlg);
  }

  public void NetRegisterDelegate(CustomNetworkBehaviour.DelegateFloat dlg)
  {
    this._dlg_float.Add(dlg);
  }

  public void NetRegisterDelegate(CustomNetworkBehaviour.DelegateWGOFloat dlg)
  {
    this._dlg_wgo_float.Add(dlg);
  }

  public delegate void DelegateVoid();

  public delegate void DelegateFloat(float f);

  public delegate void DelegateWGOFloat(WorldGameObject wgo, float f);
}
