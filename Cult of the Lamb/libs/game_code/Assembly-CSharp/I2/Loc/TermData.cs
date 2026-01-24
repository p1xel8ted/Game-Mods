// Decompiled with JetBrains decompiler
// Type: I2.Loc.TermData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace I2.Loc;

[Serializable]
public class TermData
{
  public string Term = string.Empty;
  public eTermType TermType;
  [NonSerialized]
  public string Description;
  public string[] Languages = Array.Empty<string>();
  public byte[] Flags = Array.Empty<byte>();
  [SerializeField]
  public string[] Languages_Touch;

  public string GetTranslation(int idx, string specialization = null, bool editMode = false)
  {
    string text = this.Languages[idx];
    if (text != null)
    {
      text = SpecializationManager.GetSpecializedText(text, specialization);
      if (!editMode)
        text = text.Replace("[i2nt]", "").Replace("[/i2nt]", "");
    }
    return text;
  }

  public void SetTranslation(int idx, string translation, string specialization = null)
  {
    this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
  }

  public void RemoveSpecialization(string specialization)
  {
    for (int idx = 0; idx < this.Languages.Length; ++idx)
      this.RemoveSpecialization(idx, specialization);
  }

  public void RemoveSpecialization(int idx, string specialization)
  {
    string language = this.Languages[idx];
    if (specialization == "Any" || !language.Contains($"[i2s_{specialization}]"))
      return;
    Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(language);
    specializations.Remove(specialization);
    this.Languages[idx] = SpecializationManager.SetSpecializedText(specializations);
  }

  public bool IsAutoTranslated(int idx, bool IsTouch) => ((int) this.Flags[idx] & 2) > 0;

  public void Validate()
  {
    int num = Mathf.Max(this.Languages.Length, this.Flags.Length);
    if (this.Languages.Length != num)
      Array.Resize<string>(ref this.Languages, num);
    if (this.Flags.Length != num)
      Array.Resize<byte>(ref this.Flags, num);
    if (this.Languages_Touch == null)
      return;
    for (int index = 0; index < Mathf.Min(this.Languages_Touch.Length, num); ++index)
    {
      if (string.IsNullOrEmpty(this.Languages[index]) && !string.IsNullOrEmpty(this.Languages_Touch[index]))
      {
        this.Languages[index] = this.Languages_Touch[index];
        this.Languages_Touch[index] = (string) null;
      }
    }
    this.Languages_Touch = (string[]) null;
  }

  public bool IsTerm(string name, bool allowCategoryMistmatch)
  {
    return !allowCategoryMistmatch ? name == this.Term : name == LanguageSourceData.GetKeyFromFullTerm(this.Term);
  }

  public bool HasSpecializations()
  {
    for (int index = 0; index < this.Languages.Length; ++index)
    {
      if (!string.IsNullOrEmpty(this.Languages[index]) && this.Languages[index].Contains("[i2s_"))
        return true;
    }
    return false;
  }

  public List<string> GetAllSpecializations()
  {
    List<string> list = new List<string>();
    for (int index = 0; index < this.Languages.Length; ++index)
      SpecializationManager.AppendSpecializations(this.Languages[index], list);
    return list;
  }
}
