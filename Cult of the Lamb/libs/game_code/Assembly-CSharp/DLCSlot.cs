// Decompiled with JetBrains decompiler
// Type: DLCSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Steamworks;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DLCSlot : MonoBehaviour
{
  [SerializeField]
  public DLCSlot.DLC DLCType;
  [SerializeField]
  public GameObject activatedIcon;
  [SerializeField]
  public string steamURL;
  [SerializeField]
  public string gogURL;
  public ulong SwitchID = 72108652991111168;
  [SerializeField]
  public string GameCoreID;
  [SerializeField]
  public string Ps4ID;
  [SerializeField]
  public string PS5_SIEE_ID;
  [SerializeField]
  public string PS5_SIEA_ID;
  public MMButton button;

  public void Start()
  {
    if ((Object) this.button == (Object) null)
      this.button = this.GetComponent<MMButton>();
    this.button.onClick.AddListener((UnityAction) (() =>
    {
      switch (this.DLCType)
      {
        case DLCSlot.DLC.Cultist:
          if (GameManager.AuthenticateCultistDLC())
            return;
          break;
        case DLCSlot.DLC.Heretic:
          if (GameManager.AuthenticateHereticDLC())
            return;
          break;
        case DLCSlot.DLC.Sinful:
          if (GameManager.AuthenticateSinfulDLC())
            return;
          break;
        case DLCSlot.DLC.Pilgrim:
          if (GameManager.AuthenticatePilgrimDLC())
            return;
          break;
        case DLCSlot.DLC.DLC_1:
          if (GameManager.AuthenticateMajorDLC())
            return;
          break;
      }
      if (!SteamAPI.Init())
        return;
      SteamFriends.ActivateGameOverlayToWebPage(this.steamURL);
    }));
  }

  public void OnEnable()
  {
    if ((Object) this.button == (Object) null)
      this.button = this.GetComponent<MMButton>();
    switch (this.DLCType)
    {
      case DLCSlot.DLC.Cultist:
        this.activatedIcon.gameObject.SetActive(GameManager.AuthenticateCultistDLC());
        this.button.interactable = !GameManager.AuthenticateCultistDLC();
        this.button.enabled = !GameManager.AuthenticateCultistDLC();
        break;
      case DLCSlot.DLC.Heretic:
        this.activatedIcon.gameObject.SetActive(GameManager.AuthenticateHereticDLC());
        this.button.interactable = !GameManager.AuthenticateHereticDLC();
        this.button.enabled = !GameManager.AuthenticateHereticDLC();
        break;
      case DLCSlot.DLC.Sinful:
        this.activatedIcon.gameObject.SetActive(GameManager.AuthenticateSinfulDLC());
        this.button.interactable = !GameManager.AuthenticateSinfulDLC();
        this.button.enabled = !GameManager.AuthenticateSinfulDLC();
        break;
      case DLCSlot.DLC.Pilgrim:
        this.activatedIcon.gameObject.SetActive(GameManager.AuthenticatePilgrimDLC());
        this.button.interactable = !GameManager.AuthenticatePilgrimDLC();
        this.button.enabled = !GameManager.AuthenticatePilgrimDLC();
        break;
      case DLCSlot.DLC.DLC_1:
        this.activatedIcon.gameObject.SetActive(GameManager.AuthenticateMajorDLC());
        this.button.interactable = !GameManager.AuthenticateMajorDLC();
        this.button.enabled = !GameManager.AuthenticateMajorDLC();
        break;
    }
  }

  public void OnDisable()
  {
  }

  public void CheckDLCAdded(bool returned)
  {
  }

  [CompilerGenerated]
  public void \u003CStart\u003Eb__11_0()
  {
    switch (this.DLCType)
    {
      case DLCSlot.DLC.Cultist:
        if (GameManager.AuthenticateCultistDLC())
          return;
        break;
      case DLCSlot.DLC.Heretic:
        if (GameManager.AuthenticateHereticDLC())
          return;
        break;
      case DLCSlot.DLC.Sinful:
        if (GameManager.AuthenticateSinfulDLC())
          return;
        break;
      case DLCSlot.DLC.Pilgrim:
        if (GameManager.AuthenticatePilgrimDLC())
          return;
        break;
      case DLCSlot.DLC.DLC_1:
        if (GameManager.AuthenticateMajorDLC())
          return;
        break;
    }
    if (!SteamAPI.Init())
      return;
    SteamFriends.ActivateGameOverlayToWebPage(this.steamURL);
  }

  public enum DLC
  {
    Cultist,
    Heretic,
    Sinful,
    Pilgrim,
    DLC_1,
  }
}
