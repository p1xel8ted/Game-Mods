// Decompiled with JetBrains decompiler
// Type: RewiredActivateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
