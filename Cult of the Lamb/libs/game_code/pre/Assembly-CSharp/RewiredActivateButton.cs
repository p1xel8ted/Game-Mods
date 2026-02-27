// Decompiled with JetBrains decompiler
// Type: RewiredActivateButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using Unify.Input;

#nullable disable
public class RewiredActivateButton : BaseMonoBehaviour
{
  public UnityEngine.UI.Button button;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  public int Action;
  private Rewired.Player player;

  private void Start() => this.player = RewiredInputManager.MainPlayer;

  private void Update()
  {
    if (!this.player.GetButtonDown(this.Action))
      return;
    this.button.onClick.Invoke();
    this.enabled = false;
  }
}
