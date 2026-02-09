// Decompiled with JetBrains decompiler
// Type: BearLogicExpression
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class BearLogicExpression
{
  [SerializeField]
  public List<BearLogicExpression.BearLogicExpressionPart> _s = new List<BearLogicExpression.BearLogicExpressionPart>();

  public bool Evaluate(GameRes globals, GameRes obj, GameRes subj)
  {
    bool flag1 = true;
    string str1 = "";
    string str2 = "";
    foreach (BearLogicExpression.BearLogicExpressionPart logicExpressionPart in this._s)
    {
      if (!string.IsNullOrEmpty(logicExpressionPart.expr))
      {
        str2 += logicExpressionPart.expr;
        if (logicExpressionPart.expr.Contains("&") || logicExpressionPart.expr.Contains("|"))
        {
          if (str1 != "")
          {
            Debug.LogError((object) ("Error evaluating logical expression: " + str2));
            return false;
          }
          str1 = logicExpressionPart.expr;
        }
        else
        {
          GameRes gameRes;
          switch (logicExpressionPart.target)
          {
            case BearLogicExpression.BearLogicExpressionPart.Target.Object:
              gameRes = obj;
              break;
            case BearLogicExpression.BearLogicExpressionPart.Target.Subject:
              gameRes = subj;
              break;
            default:
              gameRes = globals;
              break;
          }
          bool flag2 = (double) gameRes.Get(logicExpressionPart.expr) > 0.0;
          switch (str1)
          {
            case "":
            case " ":
              flag1 = flag2;
              break;
            case "&":
            case "&&":
              flag1 &= flag2;
              break;
            case "|":
            case "||":
              flag1 |= flag2;
              break;
          }
          str1 = "";
        }
      }
    }
    Debug.Log((object) $"Eval logic expr : {str2} = {flag1.ToString()}");
    return flag1;
  }

  [Serializable]
  public class BearLogicExpressionPart
  {
    public string expr = "";
    public BearLogicExpression.BearLogicExpressionPart.Target target;

    public enum Target
    {
      None,
      Object,
      Subject,
    }
  }
}
