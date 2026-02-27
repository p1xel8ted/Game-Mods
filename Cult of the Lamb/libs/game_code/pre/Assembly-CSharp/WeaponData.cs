// Decompiled with JetBrains decompiler
// Type: WeaponData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  }
}
