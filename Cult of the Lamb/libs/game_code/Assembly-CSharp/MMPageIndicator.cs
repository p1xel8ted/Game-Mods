// Decompiled with JetBrains decompiler
// Type: MMPageIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MMPageIndicator : MonoBehaviour
{
  [SerializeField]
  public GameObject _activeState;
  [SerializeField]
  public GameObject _inactiveState;

  public void Activate()
  {
    this._activeState.SetActive(true);
    this._inactiveState.SetActive(false);
  }

  public void Deactivate()
  {
    this._activeState.SetActive(false);
    this._inactiveState.SetActive(true);
  }
}
