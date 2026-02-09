// Decompiled with JetBrains decompiler
// Type: Interaction_WeatherEventShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WeatherEventShrine : Interaction
{
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public InventoryItem.ITEM_TYPE resource;
  public string sLabel;

  public Structures_Shrine_Weather StructureBrain
  {
    get => this.structure.Brain as Structures_Shrine_Weather;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.Give;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = $"{this.sLabel}: {InventoryItem.CapacityString(this.resource, 1)}";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (Inventory.GetItemQuantity(this.resource) > 0)
      this.StartCoroutine((IEnumerator) this.GiveRoutine());
    else
      this.playerFarming.indicator.PlayShake();
  }

  public IEnumerator GiveRoutine()
  {
    Interaction_WeatherEventShrine weatherEventShrine = this;
    float activateTime = 0.5f;
    weatherEventShrine.\u003CGiveRoutine\u003Eg__AddItem\u007C8_0();
    while (InputManager.Gameplay.GetInteractButtonHeld() && Inventory.GetItemQuantity(weatherEventShrine.resource) > 0 && (UnityEngine.Object) weatherEventShrine.playerFarming.interactor.CurrentInteraction == (UnityEngine.Object) weatherEventShrine && !weatherEventShrine.StructureBrain.IsRewardReady())
    {
      activateTime -= Time.deltaTime;
      if ((double) activateTime < 0.0)
      {
        weatherEventShrine.\u003CGiveRoutine\u003Eg__AddItem\u007C8_0();
        activateTime = 0.1f;
      }
      yield return (object) null;
    }
    if (weatherEventShrine.StructureBrain.IsRewardReady())
      weatherEventShrine.StartCoroutine((IEnumerator) weatherEventShrine.GiveReward());
  }

  public IEnumerator GiveReward()
  {
    Interaction_WeatherEventShrine weatherEventShrine = this;
    weatherEventShrine.StructureBrain.Clear();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(weatherEventShrine.gameObject);
    yield return (object) new WaitForSeconds(1f);
    string randomLockedSkin = DataManager.GetRandomLockedSkin();
    FollowerSkinCustomTarget skinCustomTarget = FollowerSkinCustomTarget.Create(weatherEventShrine.transform.position + Vector3.right, weatherEventShrine.playerFarming.transform.position + new Vector3(0.0f, 1f, -1f), (Transform) null, 1.25f, randomLockedSkin, (System.Action) null);
    GameManager.GetInstance().OnConversationNext(skinCustomTarget.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", weatherEventShrine.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", skinCustomTarget.gameObject);
    yield return (object) new WaitForSeconds(2.75f);
    while (UIMenuBase.ActiveMenus.Count > 0)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003CGiveRoutine\u003Eg__AddItem\u007C8_0()
  {
    ResourceCustomTarget.Create(this.gameObject, this.playerFarming.transform.position, this.resource, (System.Action) null);
    Inventory.ChangeItemQuantity(this.resource, -1);
    this.StructureBrain.DepositItem(this.resource);
    this.HasChanged = true;
  }
}
