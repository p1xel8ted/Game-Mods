// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.Achievements_Menu.AchievementItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UI.Alerts;
using Steamworks;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus.Achievements_Menu;

public class AchievementItem : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  [SerializeField]
  public string _achievementID;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lockState;
  [SerializeField]
  public Image _lockStateOverlay;
  [SerializeField]
  public AchievementAlert _alert;
  [CompilerGenerated]
  public bool \u003CUnlocked\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsHidden\u003Ek__BackingField;

  public bool Unlocked
  {
    set => this.\u003CUnlocked\u003Ek__BackingField = value;
    get => this.\u003CUnlocked\u003Ek__BackingField;
  }

  public bool IsHidden
  {
    set => this.\u003CIsHidden\u003Ek__BackingField = value;
    get => this.\u003CIsHidden\u003Ek__BackingField;
  }

  public string AchievementID => this._achievementID;

  public Sprite AchievementSprite => this._icon.sprite;

  public void Configure()
  {
    bool pbAchieved;
    if (SteamUserStats.GetAchievement(Achievements.Instance.Lookup(this._achievementID).steamId, out pbAchieved))
      this.Unlocked = pbAchieved;
    this._lockState.SetActive(!this.Unlocked);
    if (!this.Unlocked)
      this._icon.color = new Color(0.0f, 1f, 1f, 1f);
    switch (this._achievementID)
    {
      case "ALL_LEADER_FOLLOWERS":
      case "BEAT_EXECUTIONER":
      case "BEAT_YNGYA":
      case "BEAT_YNGYA_NOATTACK":
      case "FEED_FOLLOWER_MEAT":
      case "INDOCTRINATE_MIDAS":
      case "KILL_BOSS_1":
      case "KILL_BOSS_1_NODAMAGE":
      case "KILL_BOSS_2":
      case "KILL_BOSS_2_NODAMAGE":
      case "KILL_BOSS_3":
      case "KILL_BOSS_3_NODAMAGE":
      case "KILL_BOSS_4":
      case "KILL_BOSS_4_NODAMAGE":
      case "KILL_BOSS_5":
      case "RETURN_BAAL_AYM":
      case "SOZO_QUEST":
        this.IsHidden = true;
        break;
    }
    if (this.IsHidden)
      this._lockStateOverlay.color = new Color(0.1f, 0.1f, 0.1f, 1f);
    this._alert.Configure(this._achievementID);
  }

  public void OnSelect(BaseEventData eventData) => this._alert.TryRemoveAlert();
}
