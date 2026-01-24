// Decompiled with JetBrains decompiler
// Type: RewiredActivateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
