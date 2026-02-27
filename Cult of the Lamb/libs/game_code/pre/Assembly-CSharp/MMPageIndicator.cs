// Decompiled with JetBrains decompiler
// Type: MMPageIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MMPageIndicator : MonoBehaviour
{
  [SerializeField]
  private GameObject _activeState;
  [SerializeField]
  private GameObject _inactiveState;

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
