// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.LoreItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Alerts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class LoreItem : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler
{
  [Header("General")]
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public LoreAlert _alert;
  [SerializeField]
  public RectTransform _snowflake;
  [SerializeField]
  public Image _outlineImage;
  public int LoreId;

  public void Configure(int loreID)
  {
    this.LoreId = loreID;
    if ((Object) this._alert != (Object) null)
      this._alert.Configure(loreID);
    if (LoreSystem.LoreAvailable(loreID))
      this._title.text = LocalizationManager.GetTranslation($"Lore/Lore_{loreID.ToString()}/Name");
    else
      this._title.text = "???";
    if (LoreSystem.IsDLCLore(loreID))
    {
      this._snowflake.gameObject.SetActive(true);
      this._outlineImage.color = Color.blue;
    }
    else
    {
      this._snowflake.gameObject.SetActive(false);
      this._outlineImage.color = Color.red;
    }
  }

  public void OnSelect(BaseEventData eventData)
  {
    if (!((Object) this._alert != (Object) null))
      return;
    this._alert.TryRemoveAlert();
  }
}
