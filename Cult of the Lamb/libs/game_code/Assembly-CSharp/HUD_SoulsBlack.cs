// Decompiled with JetBrains decompiler
// Type: HUD_SoulsBlack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class HUD_SoulsBlack : BaseMonoBehaviour
{
  public TextMeshProUGUI Count;
  public TextMeshProUGUI Delta;
  public int _currentCount;
  public int _currentdelta;
  public float Delay;
  public float FadeDelay;
  public Color DeltaColor;
  public Color DeltaFade;
  public PlayerFarming playerFarming;

  public int CurrentCount
  {
    get => this._currentCount;
    set
    {
      this._currentCount = value;
      this.Count.text = "<sprite name=\"icon_blackSoul\"> " + this._currentCount.ToString();
    }
  }

  public int CurrentDelta
  {
    get => this._currentdelta;
    set
    {
      this._currentdelta = value;
      this.Delta.text = (this._currentdelta > 0 ? "+" : "") + this._currentdelta.ToString();
    }
  }

  public void OnEnable()
  {
    PlayerFarming.OnGetBlackSoul += new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
    this.CurrentCount = this.playerFarming.BlackSouls;
    this.Delta.text = "";
    this.DeltaColor = this.Delta.color;
    this.DeltaFade = this.Delta.color - new Color(0.0f, 0.0f, 0.0f, 1f);
  }

  public void OnResetBlackSouls(int DeltaValue)
  {
    this.CurrentDelta = 0;
    this.CurrentCount = this.playerFarming.BlackSouls;
    this.Delta.text = "";
  }

  public void OnDisable()
  {
    PlayerFarming.OnGetBlackSoul -= new PlayerFarming.GetBlackSoulAction(this.OnGetSoul);
  }

  public void OnGetSoul(int DeltaValue, PlayerFarming playerFarming)
  {
    if ((Object) this.playerFarming != (Object) playerFarming)
      return;
    this.CurrentDelta += DeltaValue;
    this.FadeDelay = 1f;
    this.Delta.color = this.DeltaColor;
    this.Delta.text = (this.CurrentDelta > 0 ? "+" : "") + this.CurrentDelta.ToString();
    this.Delay = 1f;
  }

  public void Update()
  {
    if (this.CurrentDelta != 0)
    {
      if ((double) (this.Delay -= Time.deltaTime) >= 0.0)
        return;
      if (this.CurrentDelta > 0)
      {
        --this.CurrentDelta;
        ++this.CurrentCount;
      }
      else if (this.CurrentDelta < 0)
      {
        ++this.CurrentDelta;
        --this.CurrentCount;
      }
      this.Delay = 0.05f;
    }
    else
    {
      if ((double) (this.FadeDelay -= Time.deltaTime) >= 0.0)
        return;
      this.Delta.color = Color.Lerp(this.Delta.color, this.DeltaFade, 2f * Time.deltaTime);
    }
  }
}
