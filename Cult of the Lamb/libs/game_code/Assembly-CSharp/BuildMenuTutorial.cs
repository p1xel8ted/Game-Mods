// Decompiled with JetBrains decompiler
// Type: BuildMenuTutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using Rewired;
using Unify.Input;
using UnityEngine;

#nullable disable
public class BuildMenuTutorial : BaseMonoBehaviour
{
  public RectTransform rectTransform;
  public Transform Target;
  public Vector3 Offset = new Vector3(0.0f, 0.0f, -1f);
  public MMControlPrompt mmControlPrompt;
  public CanvasMenuList list;

  public Player player => RewiredInputManager.MainPlayer;

  public void Start()
  {
    this.rectTransform = this.GetComponent<RectTransform>();
    this.SetPosition();
    this.list = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasMenuList>();
  }

  public void Update()
  {
    this.SetPosition();
    if (!this.player.GetButtonUp(this.mmControlPrompt.Action) || !this.list.BuildingMenu.activeSelf)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void SetPosition()
  {
    this.rectTransform.position = Camera.main.WorldToScreenPoint(this.Target.position + this.Offset);
  }
}
