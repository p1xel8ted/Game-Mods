// Decompiled with JetBrains decompiler
// Type: Alerts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using src.Alerts;
using System;

#nullable disable
[MessagePackObject(false)]
[Union(0, typeof (DoctrineAlerts))]
[Union(1, typeof (FollowerInteractionAlerts))]
[Union(2, typeof (RitualAlerts))]
[Union(3, typeof (StructureAlerts))]
[Union(4, typeof (CharacterSkinAlerts))]
[Union(5, typeof (InventoryAlerts))]
[Union(6, typeof (WeaponAlerts))]
[Union(7, typeof (CurseAlerts))]
[Union(8, typeof (TarotCardAlerts))]
[Union(9, typeof (UpgradeAlerts))]
[Union(10, typeof (LocationAlerts))]
[Union(11, typeof (TutorialAlerts))]
[Union(12, typeof (RecipeAlerts))]
[Union(13, typeof (RelicAlerts))]
[Union(14, typeof (PhotoGalleryAlerts))]
[Union(15, typeof (ClothingAlerts))]
[Union(16 /*0x10*/, typeof (ClothingCustomiseAlerts))]
[Union(17, typeof (ClothingAssignAlerts))]
[Union(18, typeof (FlockadePieceAlerts))]
[Union(19, typeof (LoreAlerts))]
[Union(20, typeof (TraitManipulatorAlerts))]
[Union(21, typeof (RunTarotCardAlerts))]
[Serializable]
public class Alerts
{
  [Key(0)]
  public DoctrineAlerts Doctrine = new DoctrineAlerts();
  [Key(1)]
  public FollowerInteractionAlerts FollowerInteractions = new FollowerInteractionAlerts();
  [Key(2)]
  public RitualAlerts Rituals = new RitualAlerts();
  [Key(3)]
  public StructureAlerts Structures = new StructureAlerts();
  [Key(4)]
  public CharacterSkinAlerts CharacterSkinAlerts = new CharacterSkinAlerts();
  [Key(5)]
  public InventoryAlerts Inventory = new InventoryAlerts();
  [Key(6)]
  public WeaponAlerts Weapons = new WeaponAlerts();
  [Key(7)]
  public CurseAlerts Curses = new CurseAlerts();
  [Key(8)]
  public TarotCardAlerts TarotCardAlerts = new TarotCardAlerts();
  [Key(9)]
  public UpgradeAlerts Upgrades = new UpgradeAlerts();
  [Key(10)]
  public LocationAlerts Locations = new LocationAlerts();
  [Key(11)]
  public TutorialAlerts Tutorial = new TutorialAlerts();
  [Key(12)]
  public RecipeAlerts Recipes = new RecipeAlerts();
  [Key(13)]
  public RelicAlerts RelicAlerts = new RelicAlerts();
  [Key(14)]
  public PhotoGalleryAlerts GalleryAlerts = new PhotoGalleryAlerts();
  [Key(15)]
  public ClothingAlerts ClothingAlerts = new ClothingAlerts();
  [Key(16 /*0x10*/)]
  public ClothingCustomiseAlerts ClothingCustomiseAlerts = new ClothingCustomiseAlerts();
  [Key(17)]
  public ClothingAssignAlerts ClothingAssignAlerts = new ClothingAssignAlerts();
  [Key(18)]
  public FlockadePieceAlerts FlockadePieceAlerts = new FlockadePieceAlerts();
  [Key(19)]
  public LoreAlerts LoreAlerts = new LoreAlerts();
  [Key(20)]
  public TraitManipulatorAlerts TraitManipulatorAlerts = new TraitManipulatorAlerts();
  [Key(21)]
  public RunTarotCardAlerts RunTarotCardAlerts = new RunTarotCardAlerts();
}
