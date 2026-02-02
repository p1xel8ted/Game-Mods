// Decompiled with JetBrains decompiler
// Type: PlayerSkillManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PlayerSkillManager
{
  public static float GetPlayerSkillValue()
  {
    return DataManager.Instance.PlayerDamageDealt / Mathf.Clamp(DataManager.Instance.PlayerDamageReceived, 0.1f, float.MaxValue);
  }

  public static float GetPlayerTotal()
  {
    return DataManager.Instance.PlayerDamageDealt + DataManager.Instance.PlayerDamageReceived;
  }
}
