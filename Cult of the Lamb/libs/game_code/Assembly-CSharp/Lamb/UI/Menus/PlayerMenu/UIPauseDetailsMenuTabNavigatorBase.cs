// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.UIPauseDetailsMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.Menus.PlayerMenu;

public class UIPauseDetailsMenuTabNavigatorBase : MMTabNavigatorBase<UIPauseDetailsMenuTab>
{
  [SerializeField]
  public MMButton _loreButton;
  public static UIPauseDetailsMenuTabNavigatorBase Instance;

  public void Awake() => UIPauseDetailsMenuTabNavigatorBase.Instance = this;

  public void TransitionToLore() => this.TransitionTo(this._tabs[4]);

  public override void Start()
  {
    base.Start();
    if (DataManager.Instance.LoreStonesOnboarded || !((Object) this._loreButton != (Object) null))
      return;
    this._loreButton.gameObject.SetActive(false);
  }
}
