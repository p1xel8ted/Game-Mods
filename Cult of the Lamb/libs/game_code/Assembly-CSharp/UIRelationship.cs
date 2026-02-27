// Decompiled with JetBrains decompiler
// Type: UIRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIRelationship : 
  BaseMonoBehaviour,
  ISelectHandler,
  IEventSystemHandler,
  IDeselectHandler
{
  [SerializeField]
  public SkeletonGraphic followerSkinGraphic;
  [SerializeField]
  public GameObject selectedBorder;
  [SerializeField]
  public GameObject descriptionContainer;
  [SerializeField]
  public TMP_Text relationshipTitle;
  [SerializeField]
  public TMP_Text relationshipDescription;
  [SerializeField]
  public Image relationshipIcon;
  [Space]
  [SerializeField]
  public Sprite enemiesIcon;
  [SerializeField]
  public Sprite friendsIcon;
  [SerializeField]
  public Sprite loversIcon;

  public void Play(FollowerInfo followerInfo, IDAndRelationship relationship)
  {
    this.followerSkinGraphic.Skeleton.SetSkin(WorshipperData.Instance.Characters[followerInfo.SkinCharacter].Skin[followerInfo.SkinVariation].Skin);
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(followerInfo.SkinName);
    if (colourData != null)
    {
      foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(followerInfo.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Slot slot = this.followerSkinGraphic.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    string str = "";
    if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
    {
      this.relationshipIcon.sprite = this.friendsIcon;
      this.relationshipIcon.color = Color.green;
      str = LocalizationManager.GetTranslation("UI/Friends");
    }
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
    {
      this.relationshipIcon.sprite = this.enemiesIcon;
      this.relationshipIcon.color = Color.red;
      str = LocalizationManager.GetTranslation("UI/Enemies");
    }
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
    {
      this.relationshipIcon.sprite = this.loversIcon;
      str = LocalizationManager.GetTranslation("UI/Lovers");
    }
    else
      this.relationshipIcon.gameObject.SetActive(false);
    this.relationshipTitle.text = str;
    this.relationshipDescription.text = string.Format(LocalizationManager.GetTranslation("UI/RelationshipDescription"), (object) followerInfo.Name);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.descriptionContainer.SetActive(true);
    this.selectedBorder.SetActive(true);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.descriptionContainer.SetActive(false);
    this.selectedBorder.SetActive(false);
  }
}
