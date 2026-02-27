// Decompiled with JetBrains decompiler
// Type: ChestLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using UnityEngine;

#nullable disable
public class ChestLoader : MonoBehaviour
{
  [Header("Chest references")]
  public GameObject CameraBone;
  public ColliderEvents DamageCollider;
  public static Interaction_Chest Instance;
  public GameObject Shadow;
  public GameObject Lighting;
  public InventoryItemDisplay Item;
  public GameObject PlayerPosition;
  public SkeletonAnimation Spine;
}
