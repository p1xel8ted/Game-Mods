// Decompiled with JetBrains decompiler
// Type: Altar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Altar : BaseMonoBehaviour
{
  public List<Transform> NewRecruitPositions = new List<Transform>();
  public List<Transform> SacrificePositions = new List<Transform>();
  public GameObject CentrePoint;
  public static Altar Instance;

  private void OnEnable() => Altar.Instance = this;

  private void OnDisable() => Altar.Instance = (Altar) null;
}
