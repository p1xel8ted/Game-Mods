// Decompiled with JetBrains decompiler
// Type: GameRes
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class GameRes
{
  [SerializeField]
  public List<string> _res_type = new List<string>();
  [SerializeField]
  public List<float> _res_v = new List<float>();
  [SerializeField]
  public float _hp;
  [SerializeField]
  public float _progress;
  [SerializeField]
  public float _money;
  [SerializeField]
  public float _durability;

  public GameRes()
  {
  }

  public GameRes(GameRes r)
  {
    foreach (GameResAtom atom in r.ToAtomList())
      this.Add(atom);
  }

  public GameRes(string stype, float value) => this.Set(stype, value);

  public List<string> Types => this._res_type;

  public float Get(string stype, float default_value = 0.0f)
  {
    switch (stype)
    {
      case "hp":
        return this._hp;
      case "progress":
        return this._progress;
      case "money":
        return this._money;
      case "durability":
        return this._durability;
      default:
        int index = this._res_type.IndexOf(stype);
        return index != -1 ? this._res_v[index] : default_value;
    }
  }

  public int GetInt(string stype) => (int) this.Get(stype);

  public bool Has(string stype)
  {
    return stype == "hp" || stype == "progress" || stype == "money" || stype == "durability" || this._res_type.Contains(stype);
  }

  public void Set(string type, float value)
  {
    switch (type)
    {
      case "hp":
        this._hp = value;
        break;
      case "progress":
        this._progress = value;
        break;
      case "money":
        this._money = value;
        break;
      case "durability":
        this._durability = value;
        break;
      default:
        int index = this._res_type.IndexOf(type);
        if (index != -1)
        {
          this._res_v[index] = value;
          break;
        }
        this._res_type.Add(type);
        this._res_v.Add(value);
        break;
    }
  }

  public void Clear()
  {
    this._res_type.Clear();
    this._res_v.Clear();
    this._progress = this._hp = this._money = 0.0f;
    this._durability = 1f;
  }

  public void Add(GameResAtom game_res_atom)
  {
    if (game_res_atom.IsEmpty())
      return;
    this.Add(game_res_atom.type, game_res_atom.value);
  }

  public void Sub(GameResAtom game_res_atom)
  {
    if (game_res_atom.IsEmpty())
      return;
    this.Add(game_res_atom.type, -game_res_atom.value);
  }

  public void Add(string stype, float value)
  {
    switch (stype)
    {
      case "hp":
        this._hp += value;
        break;
      case "progress":
        this._progress += value;
        break;
      case "money":
        this._money += value;
        break;
      case "durability":
        this._durability += value;
        break;
      default:
        int index = this._res_type.IndexOf(stype);
        if (index != -1)
        {
          this._res_v[index] += value;
          break;
        }
        this._res_type.Add(stype);
        this._res_v.Add(value);
        break;
    }
  }

  public void Sub(string stype, float value) => this.Add(stype, -value);

  public void Multiply(string stype, float value)
  {
    float f = this.Get(stype);
    if ((double) Mathf.Abs(f) <= 0.0001)
      return;
    this.Set(stype, (float) Mathf.RoundToInt(f * value));
  }

  public GameRes Clone()
  {
    GameRes gameRes = new GameRes();
    for (int index = this._res_type.Count - 1; index >= 0; --index)
      gameRes.Set(this._res_type[index], this._res_v[index]);
    gameRes._hp = this._hp;
    gameRes._money = this._money;
    gameRes._progress = this._progress;
    gameRes._durability = this._durability;
    return gameRes;
  }

  public void RemoveZeroValues()
  {
    List<int> intList = new List<int>();
    for (int index = this._res_type.Count - 1; index >= 0; --index)
    {
      if ((double) Mathf.Abs(this._res_v[index]) < 0.0001)
        intList.Add(index);
    }
    foreach (int index in intList)
    {
      this._res_type.RemoveAt(index);
      this._res_v.RemoveAt(index);
    }
  }

  public static GameRes operator +(GameRes r1, GameRes r2)
  {
    GameRes gameRes = r1.Clone();
    foreach (GameResAtom atom in r2.ToAtomList())
      gameRes.Add(atom);
    gameRes.RemoveZeroValues();
    return gameRes;
  }

  public static GameRes operator -(GameRes r1, GameRes r2)
  {
    GameRes gameRes = r1.Clone();
    foreach (GameResAtom atom in r2.ToAtomList())
      gameRes.Sub(atom);
    gameRes.hp = r1.hp - r2.hp;
    gameRes.money = r1.money - r2.money;
    gameRes.durability = r1.durability - r2.durability;
    gameRes.progress = r1.progress - r2.progress;
    gameRes.RemoveZeroValues();
    return gameRes;
  }

  public static GameRes operator *(GameRes r1, float k)
  {
    GameRes gameRes = new GameRes();
    foreach (GameResAtom atom in r1.ToAtomList())
      gameRes.Set(atom.type, r1.Get(atom.type) * k);
    gameRes.RemoveZeroValues();
    return gameRes;
  }

  public static GameRes operator /(GameRes r1, float k) => r1 * (1f / k);

  public List<GameResAtom> ToAtomList(float tired_k = 1f)
  {
    List<GameResAtom> atomList = new List<GameResAtom>();
    for (int index = 0; index < this._res_type.Count; ++index)
      atomList.Add(new GameResAtom()
      {
        type = this._res_type[index],
        value = this._res_v[index]
      });
    if (!this.hp.EqualsTo(0.0f))
      atomList.Add(new GameResAtom("hp", (int) this._hp));
    if (!this.progress.EqualsTo(0.0f))
      atomList.Add(new GameResAtom("progress", this._progress));
    if (!this.money.EqualsTo(0.0f))
      atomList.Add(new GameResAtom("money", this._money));
    if (!this.durability.EqualsTo(0.0f))
      atomList.Add(new GameResAtom("durability", this._durability));
    return atomList;
  }

  public bool IsEnough(GameRes sub)
  {
    bool flag = true;
    foreach (GameResAtom atom in sub.ToAtomList())
    {
      if (!this.IsEnough(atom.type, atom.value))
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool IsEnough(GameResAtom r) => this.IsEnough(r.type, r.value);

  public bool IsEnough(string type, float value)
  {
    bool flag = true;
    if ((double) this.Get(type) < (double) value)
      flag = false;
    return flag;
  }

  public static bool operator <(GameRes r1, GameRes r2) => r2.IsEnough(r1);

  public static bool operator <=(GameRes r1, GameRes r2) => (r2 - r1).IsEmpty() || r1 < r2;

  public static bool operator >(GameRes r1, GameRes r2) => r1.IsEnough(r2);

  public static bool operator >=(GameRes r1, GameRes r2) => (r1 - r2).IsEmpty() || r1 > r2;

  public static bool operator ==(GameRes r1, GameRes r2)
  {
    if ((object) r1 == (object) r2)
      return true;
    return (object) r1 != null && (object) r2 != null && (r1 - r2).IsEmpty();
  }

  public static bool operator !=(GameRes r1, GameRes r2) => !(r1 == r2);

  public bool IsEmpty()
  {
    this.RemoveZeroValues();
    return this._res_v.Count == 0 && (double) Mathf.Abs(this._hp) < 1.0 / 1000.0 && (double) Mathf.Abs(this._money) < 1.0 / 1000.0 && (double) Mathf.Abs(this._progress) < 1.0 / 1000.0 && (double) Mathf.Abs(this._durability) < 1.0 / 1000.0;
  }

  public override string ToString()
  {
    string str = "";
    foreach (string stype in this._res_type)
    {
      if (str.Length > 0)
        str += ", ";
      str = $"{str}{stype}={this.Get(stype).ToString()}";
    }
    return $"[GameRes: {str}]";
  }

  public string ToPrintableString(
    bool use_colors = false,
    Color clr_normal = default (Color),
    Color clr_not_enough = default (Color),
    bool force_parentheses = false,
    bool float_format = false,
    List<string> skip = null)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int index = -1;
    foreach (string stype in this._res_type)
    {
      ++index;
      if (skip == null || !skip.Contains(stype))
      {
        float a = this.Get(stype);
        if (!a.EqualsTo(0.0f))
        {
          int num = this.GetInt(stype);
          if (stringBuilder.Length > 0)
            stringBuilder.Append(" ");
          string str = stype;
          if (TechDefinition.TECH_POINTS.Contains(stype) | force_parentheses)
            str = $"({str})";
          stringBuilder.Append(str);
          if (use_colors)
          {
            stringBuilder.Append('[');
            stringBuilder.Append(MainGame.me.player.IsEnough(new GameRes(stype, this._res_v[index])) ? clr_normal.ToHex(true) : clr_not_enough.ToHex(true));
            stringBuilder.Append(']');
          }
          if (float_format)
            stringBuilder.Append(a.ToString("0.0"));
          else
            stringBuilder.Append(num);
          if (use_colors)
            stringBuilder.Append("[-]");
        }
      }
    }
    return stringBuilder.ToString();
  }

  public string ToFormattedString(bool colorize_values = true, GameRes add_game_res = null)
  {
    StringBuilder stringBuilder = new StringBuilder();
    List<string> stringList = new List<string>() { "hp" };
    stringList.AddRange((IEnumerable<string>) this._res_type);
    if (add_game_res != (GameRes) null)
    {
      foreach (GameResAtom atom in add_game_res.ToAtomList())
      {
        if (!stringList.Contains(atom.type))
          stringList.Add(atom.type);
      }
    }
    foreach (string stype in stringList)
    {
      int num = this.GetInt(stype);
      if (add_game_res != (GameRes) null)
        num += add_game_res.GetInt(stype);
      if (num != 0)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(" ");
        string str1;
        string str2;
        switch (stype)
        {
          case "hp":
            str1 = "[c][F4203F]";
            str2 = "(hp)";
            break;
          case "energy":
            str1 = "[c][3897FF]";
            str2 = "(en)";
            break;
          case "sanity":
            str1 = "[c][BA00C5]";
            str2 = "(sn)";
            break;
          default:
            str1 = "[14E549]";
            str2 = $"({stype})";
            break;
        }
        if (colorize_values)
          stringBuilder.Append(str1);
        stringBuilder.Append(num >= 0 ? "+" : "-");
        stringBuilder.Append(str2);
        if (num != 0)
          stringBuilder.Append(Mathf.Abs(num));
        if (colorize_values)
          stringBuilder.Append("[-][/c]");
      }
    }
    return stringBuilder.ToString();
  }

  public static string ToFormattedString(List<GameRes> reses, bool colorize_values = true)
  {
    if (reses == null || reses.Count == 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    List<string> stringList = new List<string>()
    {
      "hp",
      "energy"
    };
    foreach (GameRes rese in reses)
    {
      if (!(rese == (GameRes) null))
      {
        foreach (string str in rese._res_type)
        {
          if (!stringList.Contains(str))
            stringList.Add(str);
        }
      }
    }
    foreach (string stype in stringList)
    {
      int num1 = int.MaxValue;
      int num2 = int.MinValue;
      foreach (GameRes rese in reses)
      {
        if (!(rese == (GameRes) null))
        {
          int num3 = rese.GetInt(stype);
          if (num3 < num1)
            num1 = num3;
          if (num3 > num2)
            num2 = num3;
        }
      }
      if (num1 != 0 || num2 != 0)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(" ");
        string str1;
        string str2;
        switch (stype)
        {
          case "hp":
            str1 = "[c][F4203F]";
            str2 = "(hp)";
            break;
          case "energy":
            str1 = "[c][3897FF]";
            str2 = "(en)";
            break;
          case "sanity":
            str1 = "[c][BA00C5]";
            str2 = "(sn)";
            break;
          default:
            str1 = "[14E549]";
            str2 = $"({stype})";
            break;
        }
        if (colorize_values)
          stringBuilder.Append(str1);
        stringBuilder.Append(num1 >= 0 ? "+" : "-");
        stringBuilder.Append(str2);
        stringBuilder.Append(Mathf.Abs(num1));
        if (num1 < num2)
        {
          stringBuilder.Append(" .. ");
          stringBuilder.Append(num2 >= 0 ? "+" : "-");
          stringBuilder.Append(str2);
          stringBuilder.Append(Mathf.Abs(num2));
        }
        if (colorize_values)
          stringBuilder.Append("[-][/c]");
      }
    }
    return stringBuilder.ToString();
  }

  public float hp
  {
    get => this._hp;
    set => this._hp = value;
  }

  public float progress
  {
    get => this._progress;
    set => this._progress = value;
  }

  public float money
  {
    get => this._money;
    set => this._money = value;
  }

  public float durability
  {
    get => this._durability;
    set => this._durability = value;
  }

  public void RemoveAllBut(List<string> exceptions)
  {
    for (int index = 0; index < this._res_type.Count; ++index)
    {
      if (!exceptions.Contains(this._res_type[index]))
        this._res_v[index] = 0.0f;
    }
    if (!exceptions.Contains("hp"))
      this.hp = 0.0f;
    if (!exceptions.Contains("money"))
      this.money = 0.0f;
    if (!exceptions.Contains("progress"))
      this.progress = 0.0f;
    if (!exceptions.Contains("durability"))
      this.durability = 0.0f;
    this.RemoveZeroValues();
  }

  public static GameRes ParseSmartExpression(SmartExpression smart_expr)
  {
    GameRes smartExpression = new GameRes();
    if (smart_expr == null || smart_expr.HasNoExpresion())
      return smartExpression;
    string expressionString = smart_expr.GetRawExpressionString();
    if (expressionString.Contains("energy"))
    {
      string pattern = "(AddPpar\\( *(\"energy\") *, *(.*)\\))";
      Match match = Regex.Match(expressionString, pattern);
      if (match.Success)
      {
        if (match.Groups[2].Captures[0].ToString() != "\"energy\"")
        {
          Debug.LogError((object) "Error while parsing SmartExpression #1. Call Bulat.");
        }
        else
        {
          float num = SmartExpression.ParseExpression(match.Groups[3].Captures[0].ToString()).EvaluateFloat(character: MainGame.me.player);
          smartExpression.Add("energy", num);
        }
      }
    }
    if (expressionString.Contains("hp"))
    {
      Match match = Regex.Match(expressionString, "(AddPpar\\( *(\"hp\") *, *(.*)\\))");
      if (match.Success)
      {
        if (match.Groups[2].Captures[0].ToString() != "\"hp\"")
        {
          Debug.LogError((object) "Error while parsing SmartExpression #2. Call Bulat.");
        }
        else
        {
          float num = SmartExpression.ParseExpression(match.Groups[3].Captures[0].ToString()).EvaluateFloat(character: MainGame.me.player);
          smartExpression.Add("hp", num);
        }
      }
    }
    return smartExpression;
  }
}
