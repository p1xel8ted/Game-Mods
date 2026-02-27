// Decompiled with JetBrains decompiler
// Type: UIRelationship
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private SkeletonGraphic followerSkinGraphic;
  [SerializeField]
  private GameObject selectedBorder;
  [SerializeField]
  private GameObject descriptionContainer;
  [SerializeField]
  private TMP_Text relationshipTitle;
  [SerializeField]
  private TMP_Text relationshipDescription;
  [SerializeField]
  private Image relationshipIcon;
  [Space]
  [SerializeField]
  private Sprite enemiesIcon;
  [SerializeField]
  private Sprite friendsIcon;
  [SerializeField]
  private Sprite loversIcon;

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
