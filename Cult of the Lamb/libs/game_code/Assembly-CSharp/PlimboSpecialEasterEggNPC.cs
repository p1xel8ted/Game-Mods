// Decompiled with JetBrains decompiler
// Type: PlimboSpecialEasterEggNPC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class PlimboSpecialEasterEggNPC : MonoBehaviour
{
  [SerializeField]
  public Interaction_SimpleConversation equippedBlunderbussConvo;
  [SerializeField]
  public Interaction_SimpleConversation leftBlunderbussConvo;

  public void Awake()
  {
    this.equippedBlunderbussConvo.Callback.AddListener(new UnityAction(this.ConvoCallback));
    this.leftBlunderbussConvo.Callback.AddListener(new UnityAction(this.ConvoCallback));
  }

  public void OnDestroy()
  {
    this.equippedBlunderbussConvo.Callback.RemoveListener(new UnityAction(this.ConvoCallback));
    this.leftBlunderbussConvo.Callback.RemoveListener(new UnityAction(this.ConvoCallback));
  }

  public void OnEnable()
  {
    if (PlayerFarming.AnyPlayerHasLegendaryWeapon(EquipmentType.Blunderbuss_Legendary))
      this.equippedBlunderbussConvo.gameObject.SetActive(true);
    else
      this.leftBlunderbussConvo.gameObject.SetActive(false);
  }

  public void ConvoCallback()
  {
    DataManager.Instance.LegendaryBlunderbussPlimboEaterEggTalked = true;
    DataManager.Instance.LegendaryBlunderbussPlimboEasterEggActive = false;
  }
}
