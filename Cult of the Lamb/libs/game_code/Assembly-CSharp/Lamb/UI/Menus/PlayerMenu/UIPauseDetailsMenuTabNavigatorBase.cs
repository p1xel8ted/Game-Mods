// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Menus.PlayerMenu.UIPauseDetailsMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
