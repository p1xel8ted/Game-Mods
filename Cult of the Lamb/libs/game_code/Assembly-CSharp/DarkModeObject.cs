// Decompiled with JetBrains decompiler
// Type: DarkModeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DarkModeObject : MonoBehaviour
{
  public Image image;
  public TextMeshProUGUI text;
  [SerializeField]
  public bool EnableDisable = true;
  [SerializeField]
  public bool EnableOnDark = true;
  [SerializeField]
  public bool ChangeColor;
  [SerializeField]
  public Color darkModeColor;
  public Color _startColor;

  public void Start()
  {
    this.image = this.GetComponent<Image>();
    this.text = this.GetComponent<TextMeshProUGUI>();
    if ((UnityEngine.Object) this.image != (UnityEngine.Object) null)
      this._startColor = this.image.color;
    if ((UnityEngine.Object) this.text != (UnityEngine.Object) null)
      this._startColor = this.text.color;
    this.UpdateObject();
    GraphicsSettingsUtilities.OnDarkModeSettingsChanged += new System.Action(this.UpdateObject);
  }

  public void OnDestroy()
  {
    GraphicsSettingsUtilities.OnDarkModeSettingsChanged -= new System.Action(this.UpdateObject);
  }

  public void UpdateObject()
  {
    if ((UnityEngine.Object) this.text != (UnityEngine.Object) null)
    {
      if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility != null && SettingsManager.Settings.Accessibility.DarkMode)
        this.text.color = this.darkModeColor;
      else
        this.text.color = this._startColor;
    }
    if (!((UnityEngine.Object) this.image != (UnityEngine.Object) null))
      return;
    if (SettingsManager.Settings != null && SettingsManager.Settings.Accessibility != null && SettingsManager.Settings.Accessibility.DarkMode)
    {
      if (this.ChangeColor)
        this.image.color = this.darkModeColor;
      if (!this.EnableDisable)
        return;
      if (this.EnableOnDark)
        this.image.enabled = true;
      else
        this.image.enabled = false;
    }
    else
    {
      if (this.ChangeColor)
        this.image.color = this._startColor;
      if (!this.EnableDisable)
        return;
      if (this.EnableOnDark)
        this.image.enabled = false;
      else
        this.image.enabled = true;
    }
  }
}
