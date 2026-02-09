// Decompiled with JetBrains decompiler
// Type: BearCode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[Serializable]
public class BearCode
{
  public List<BearCode.BearCodeItem> _equations = new List<BearCode.BearCodeItem>();
  public static Regex _regex_eq = new Regex("^([^ =\\+\\*\\-\\/]+) *([\\+\\*\\-\\/]?=) *(.+)$");
  public static Regex _regex_method = new Regex("^([^\\.]*?)\\.?([^\\.\\(\\)]+)\\((.*)\\)$");

  public BearCode()
  {
  }

  public BearCode(string code) => this.FromString(code);

  public void FromString(string code)
  {
    string str1 = code;
    char[] chArray = new char[1]{ '\n' };
    foreach (string str2 in str1.Split(chArray))
    {
      string input = str2.Trim('\r', ' ', '\t').Replace("\r", " ").Replace("\t", " ").Replace("  ", " ");
      if (!string.IsNullOrEmpty(input))
      {
        Match match1 = BearCode._regex_eq.Match(input);
        if (match1.Success)
        {
          string str3 = match1.Groups[2].Captures[0].ToString();
          BearCode.BearCodeItem.EquationType equationType;
          switch (str3)
          {
            case "=":
              equationType = BearCode.BearCodeItem.EquationType.Equal;
              break;
            case "*=":
              equationType = BearCode.BearCodeItem.EquationType.Multiply;
              break;
            case "+=":
              equationType = BearCode.BearCodeItem.EquationType.Plus;
              break;
            case "-=":
              equationType = BearCode.BearCodeItem.EquationType.Minus;
              break;
            case "/=":
              equationType = BearCode.BearCodeItem.EquationType.Divide;
              break;
            default:
              Debug.LogError((object) $"Unknown sign '{str3}' in: {str2}");
              continue;
          }
          BearCode.BearCodeItem bearCodeItem = new BearCode.BearCodeItem()
          {
            var_name = match1.Groups[1].Captures[0].ToString(),
            eq_type = equationType,
            value = match1.Groups[3].Captures[0].ToString(),
            code_type = BearCode.BearCodeItem.CodeType.Equation
          };
          if (string.IsNullOrEmpty(bearCodeItem.var_name))
            Debug.LogError((object) ("Missing variable name: " + str2));
          else if (string.IsNullOrEmpty(bearCodeItem.value))
            Debug.LogError((object) ("Missing value: " + str2));
          else
            this._equations.Add(bearCodeItem);
        }
        else
        {
          Match match2 = BearCode._regex_method.Match(input);
          if (match2.Success)
          {
            BearCode.BearCodeItem bearCodeItem = new BearCode.BearCodeItem()
            {
              var_name = match2.Groups[1].Captures[0].ToString(),
              value = match2.Groups[2].Captures[0].ToString(),
              code_type = BearCode.BearCodeItem.CodeType.FunctionCall,
              pars = match2.Groups[3].Captures[0].ToString().Split(',')
            };
            for (int index = 0; index < bearCodeItem.pars.Length; ++index)
              bearCodeItem.pars[index] = bearCodeItem.pars[index].Trim(' ');
            this._equations.Add(bearCodeItem);
          }
          else
            this._equations.Add(new BearCode.BearCodeItem()
            {
              value = input,
              code_type = BearCode.BearCodeItem.CodeType.Expression
            });
        }
      }
    }
  }

  public void Run(GameRes res, BearCode.CallDelegate dlg, Dictionary<string, GameRes> objects = null)
  {
    foreach (BearCode.BearCodeItem equation in this._equations)
    {
      switch (equation.code_type)
      {
        case BearCode.BearCodeItem.CodeType.Equation:
          float num1 = res.Get(equation.var_name);
          float result;
          if (float.TryParse(equation.value, out result))
          {
            float num2 = result;
            switch (equation.eq_type)
            {
              case BearCode.BearCodeItem.EquationType.Equal:
                num1 = num2;
                break;
              case BearCode.BearCodeItem.EquationType.Plus:
                num1 += num2;
                break;
              case BearCode.BearCodeItem.EquationType.Minus:
                num1 -= num2;
                break;
              case BearCode.BearCodeItem.EquationType.Multiply:
                num1 *= num2;
                break;
              case BearCode.BearCodeItem.EquationType.Divide:
                num1 /= num2;
                break;
              default:
                Debug.LogError((object) ("Not implemented: " + equation.eq_type.ToString()));
                break;
            }
            if (equation.var_name.Contains("."))
            {
              string[] strArray = equation.var_name.Split('.');
              if (strArray.Length != 2)
              {
                Debug.LogError((object) ("Syntax error at variable name: " + equation.var_name));
                continue;
              }
              if (objects == null)
              {
                Debug.LogError((object) "Objects is null");
                continue;
              }
              if (!objects.ContainsKey(strArray[0]))
              {
                Debug.LogError((object) $"Object \"{strArray[0]}\" not found!");
                continue;
              }
              objects[strArray[0]].Set(strArray[1], num1);
              continue;
            }
            res.Set(equation.var_name, num1);
            continue;
          }
          Debug.LogError((object) ("Syntax error in: " + ((object) equation)?.ToString()));
          continue;
        case BearCode.BearCodeItem.CodeType.FunctionCall:
          if (dlg == null)
          {
            Debug.LogError((object) "Delegate is null");
            continue;
          }
          dlg(equation.var_name, equation.value, equation.pars);
          continue;
        default:
          continue;
      }
    }
  }

  [Serializable]
  public class BearCodeItem
  {
    public string var_name = "";
    public string value = "";
    public BearCode.BearCodeItem.EquationType eq_type;
    public BearCode.BearCodeItem.CodeType code_type;
    public string[] pars = new string[0];

    public new string ToString() => $"{this.var_name} {this.eq_type.ToString()} {this.value}";

    public enum EquationType
    {
      Equal,
      Plus,
      Minus,
      Multiply,
      Divide,
    }

    public enum CodeType
    {
      Equation,
      FunctionCall,
      Expression,
    }
  }

  public delegate void CallDelegate(string obj, string method_name, string[] pars);
}
