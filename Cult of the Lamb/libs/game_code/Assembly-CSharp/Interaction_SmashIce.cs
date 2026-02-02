// Decompiled with JetBrains decompiler
// Type: Interaction_SmashIce
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_SmashIce : Interaction
{
  [SerializeField]
  public string skinToDrop;
  [SerializeField]
  public GameObject childToHideIfUnlockedSkin;

  public void Start()
  {
    if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(this.skinToDrop))
      return;
    this.childToHideIfUnlockedSkin.gameObject.SetActive(false);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Label = LocalizationManager.GetTranslation("Interactions/BreakIce");
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (!DataManager.Instance.FollowerSkinsUnlocked.Contains(this.skinToDrop))
    {
      PickUp pickUp = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN, 1, this.transform.position);
      if ((Object) pickUp != (Object) null)
      {
        FoundItemPickUp component = pickUp.GetComponent<FoundItemPickUp>();
        component.FollowerSkinForceSelection = true;
        component.SkinToForce = this.skinToDrop;
        component.ActivateDistance = 1.25f;
      }
    }
    AudioManager.Instance.PlayOneShot("event:/dlc/material/obstacle/ice_destroy", this.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    BiomeConstants.Instance.EmitParticleChunk(BiomeConstants.TypeOfParticle.snow_1, this.transform.position, Vector3.one, 10);
    Object.Destroy((Object) this.gameObject);
  }
}
