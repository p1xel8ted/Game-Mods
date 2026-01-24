// Decompiled with JetBrains decompiler
// Type: WeaponData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu(menuName = "Massive Monster/Weapon Data")]
public class WeaponData : EquipmentData
{
  public WeaponData.Skins Skin;
  public float Speed;
  [Space]
  public List<PlayerWeapon.WeaponCombos> Combos;
  public List<WeaponAttachmentData> Attachments;
  public PlayerWeapon.WeaponCombos HeavyAttackCombo;

  public bool ContainsAttachmentType(AttachmentEffect attachment)
  {
    return (Object) this.GetAttachment(attachment) != (Object) null;
  }

  public WeaponAttachmentData GetAttachment(AttachmentEffect attachment)
  {
    foreach (WeaponAttachmentData attachment1 in this.Attachments)
    {
      if (attachment1.Effect == attachment)
        return attachment1;
    }
    return (WeaponAttachmentData) null;
  }

  public enum Skins
  {
    Normal,
    Critical,
    Fervor,
    Godly,
    Healing,
    Necromancy,
    Poison,
    Executioner,
    Executioner_Hallowed,
    Legendary,
    Legendary_Broken,
    RatauStaff,
    RatauStaff_Broken,
  }
}
