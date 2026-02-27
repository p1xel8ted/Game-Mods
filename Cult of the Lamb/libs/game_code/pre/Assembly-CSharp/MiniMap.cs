// Decompiled with JetBrains decompiler
// Type: MiniMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MiniMap : BaseMonoBehaviour
{
  public GameObject CurrentRoom;
  public GameObject Background;
  public Sprite Room;
  public Sprite Room_Snake;
  public Sprite Room_Goat;
  public Sprite StartingRoom;
  public Sprite RoomPointOfInterest;
  public Sprite Door;
  public Sprite Base;
  public GameObject IconContainer;
  [HideInInspector]
  public RectTransform IconContainerRect;
  public float Spacing = 25f;
  private Vector2 IconSize;
  private float Scale = 0.2f;
  [HideInInspector]
  public List<MiniMapIcon> Icons = new List<MiniMapIcon>();
  public float PanSpeed = 10f;
  public GameObject RoomIconPrefab;
  public GameObject BaseIcon;
  public bool HideUnexplored = true;
  public bool MoveMap = true;
  [HideInInspector]
  public MiniMapIcon CurrentIcon;
  private static MiniMap Instance;
  private Vector3 NewPosition;

  public void VisitAll()
  {
    foreach (MiniMapIcon icon in this.Icons)
      icon.room.Visited = true;
    this.OnChangeRoom();
  }

  public void DiscoverAll()
  {
    foreach (MiniMapIcon icon in this.Icons)
      icon.room.Discovered = true;
    this.OnChangeRoom();
  }

  public virtual void StartMap()
  {
  }

  private void Start()
  {
    BiomeGenerator.OnBiomeGenerated += new BiomeGenerator.BiomeAction(this.OnBiomeGenerated);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    TrinketManager.OnTrinketAdded += new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
  }

  private void OnDestroy()
  {
    BiomeGenerator.OnBiomeGenerated -= new BiomeGenerator.BiomeAction(this.OnBiomeGenerated);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnChangeRoom);
    TrinketManager.OnTrinketAdded -= new TrinketManager.TrinketUpdated(this.OnTrinketAdded);
  }

  private void OnChangeRoom()
  {
    this.CurrentRoom.SetActive(this.Icons.Count > 0);
    this.Background.SetActive(this.Icons.Count > 0);
    foreach (MiniMapIcon icon in this.Icons)
    {
      if (icon.room.Visited || !this.HideUnexplored)
        icon.ShowIcon();
      else if (icon.room.Discovered)
        icon.ShowIcon(0.5f);
      else
        icon.SetSelfToQuestionMark();
      if (icon.X == BiomeGenerator.Instance.CurrentX && icon.Y == BiomeGenerator.Instance.CurrentY)
        this.CurrentIcon = icon;
    }
    if (!this.MoveMap || !((Object) this.CurrentIcon != (Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.MoveMiniMap(0.7f));
  }

  private void OnEnable()
  {
    MiniMap.Instance = this;
    this.IconContainerRect = this.IconContainer.GetComponent<RectTransform>();
    this.StartCoroutine((IEnumerator) this.Wait());
  }

  public IEnumerator Wait()
  {
    for (int i = 0; i < 5; ++i)
      yield return (object) new WaitForEndOfFrame();
    this.CurrentRoom.SetActive(false);
    this.Background.SetActive(false);
  }

  private void OnDisable()
  {
    if ((Object) WorldGen.Instance != (Object) null)
      WorldGen.Instance.OnWorldGenerated -= new WorldGen.WorldGeneratedAction(this.OnWorldGenerated);
    foreach (Component icon in this.Icons)
      Object.Destroy((Object) icon.gameObject);
    this.Icons.Clear();
  }

  private void OnBiomeGenerated()
  {
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      return;
    foreach (Component icon in this.Icons)
      Object.Destroy((Object) icon.gameObject);
    this.Icons.Clear();
    if (BiomeGenerator.Instance.OverrideRandomWalk)
    {
      this.IconContainer.SetActive(false);
    }
    else
    {
      this.IconContainer.SetActive(true);
      foreach (BiomeRoom room in BiomeGenerator.Instance.Rooms)
      {
        MiniMapIcon component = Object.Instantiate<GameObject>(this.RoomIconPrefab, this.IconContainer.transform, true).GetComponent<MiniMapIcon>();
        this.NewPosition = new Vector3((float) room.x * (this.IconSize.x + this.Spacing), (float) room.y * (this.IconSize.y + this.Spacing));
        this.IconSize = component.Init(room, this.GetImage(room), this.Scale, this.NewPosition);
        this.Icons.Add(component);
      }
      this.OnChangeRoom();
      this.CheckTelescope();
    }
  }

  private void OnTrinketAdded(TarotCards.Card trinketAdded) => this.CheckTelescope();

  private void CheckTelescope()
  {
    if (!TrinketManager.HasTrinket(TarotCards.Card.Telescope))
      return;
    this.DiscoverAll();
  }

  private void OnWorldGenerated()
  {
  }

  public static void CurrentRoomShowTeleporter()
  {
    if (!((Object) MiniMap.Instance != (Object) null))
      return;
    MiniMap.Instance.CurrentIcon.ShowTeleporter();
  }

  private Sprite GetImage(BiomeRoom room) => room.IsCustom ? this.Room_Snake : this.Room;

  public IEnumerator MoveMiniMap(float Delay)
  {
    if (!((Object) this.CurrentIcon == (Object) null))
    {
      this.NewPosition = -this.CurrentIcon.rectTransform.localPosition;
      foreach (MiniMapIcon icon in this.Icons)
      {
        icon.transform.DOKill();
        if ((Object) icon != (Object) this.CurrentIcon)
        {
          icon.transform.DOScale(Vector3.one * this.Scale, 0.5f);
          icon.IconContainer.SetActive(true);
          icon.IconContainer.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        }
        else
          icon.transform.DOScale(Vector3.one * this.Scale * 1.5f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
        icon.outlineImage.enabled = (Object) icon == (Object) this.CurrentIcon;
      }
      yield return (object) new WaitForSeconds(Delay * 0.5f);
      if ((Object) this.CurrentIcon != (Object) null && this.CurrentIcon.room != null && !this.CurrentIcon.room.IsRespawnRoom)
      {
        this.CurrentRoom.transform.DOKill();
        this.CurrentRoom.transform.DOScale(Vector3.one * 1.5f * 0.15f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBack);
      }
      yield return (object) new WaitForSeconds(Delay * 0.5f);
      if ((Object) this.CurrentIcon != (Object) null && this.CurrentIcon.room != null && !this.CurrentIcon.room.IsRespawnRoom)
      {
        while ((double) Vector3.Distance(this.NewPosition, this.IconContainerRect.localPosition) > 0.20000000298023224)
        {
          if (this.CurrentIcon.IconContainer.activeSelf && (double) Vector3.Distance(this.NewPosition, this.IconContainerRect.localPosition) < 20.0)
            this.CurrentIcon.IconContainer.transform.DOScale(Vector3.zero, 0.5f);
          this.IconContainerRect.localPosition = Vector3.Lerp(this.IconContainerRect.localPosition, this.NewPosition, this.PanSpeed * Time.deltaTime);
          yield return (object) null;
        }
        this.CurrentRoom.transform.DOKill();
        this.CurrentRoom.transform.DOScale(Vector3.one * 1f * 0.15f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      }
      this.IconContainerRect.localPosition = this.NewPosition;
    }
  }
}
