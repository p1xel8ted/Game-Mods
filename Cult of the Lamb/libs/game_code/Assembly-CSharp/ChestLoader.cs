// Decompiled with JetBrains decompiler
// Type: ChestLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
