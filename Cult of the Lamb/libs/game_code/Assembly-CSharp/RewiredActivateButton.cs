// Decompiled with JetBrains decompiler
// Type: RewiredActivateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using Unify.Input;

#nullable disable
public class RewiredActivateButton : BaseMonoBehaviour
{
  public UnityEngine.UI.Button button;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  public int Action;
  public Rewired.Player player;

  public void Start() => this.player = RewiredInputManager.MainPlayer;

  public void Update()
  {
    if (!this.player.GetButtonDown(this.Action))
      return;
    this.button.onClick.Invoke();
    this.enabled = false;
  }
}
