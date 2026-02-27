// Decompiled with JetBrains decompiler
// Type: BuildMenuTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMTools;
using Rewired;
using Unify.Input;
using UnityEngine;

#nullable disable
public class BuildMenuTutorial : BaseMonoBehaviour
{
  private RectTransform rectTransform;
  public Transform Target;
  public Vector3 Offset = new Vector3(0.0f, 0.0f, -1f);
  public MMControlPrompt mmControlPrompt;
  private CanvasMenuList list;

  private Player player => RewiredInputManager.MainPlayer;

  private void Start()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.SetPosition();
    this.list = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>();
  }

  private void Update()
  {
    this.SetPosition();
    if (!this.player.GetButtonUp(this.mmControlPrompt.Action) || !this.list.BuildingMenu.activeSelf)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  private void SetPosition()
  {
    this.rectTransform.position = Camera.main.WorldToScreenPoint(this.Target.position + this.Offset);
  }
}
