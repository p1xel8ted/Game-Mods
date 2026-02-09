// Decompiled with JetBrains decompiler
// Type: Expressive.Expression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Expressive;

public sealed class Expression
{
  public IExpression _compiledExpression;
  public ExpressiveOptions _options;
  public string _originalExpression;
  public ExpressionParser _parser;
  public string[] _variables;

  public string[] ReferencedVariables
  {
    get
    {
      this.CompileExpression();
      return this._variables;
    }
  }

  public Expression(string expression)
    : this(expression, ExpressiveOptions.None)
  {
  }

  public Expression(string expression, ExpressiveOptions options)
  {
    this._originalExpression = expression;
    this._options = options;
    this._parser = new ExpressionParser(this._options);
  }

  public object Evaluate() => this.Evaluate((IDictionary<string, object>) null);

  public object Evaluate(IDictionary<string, object> variables)
  {
    try
    {
      this.CompileExpression();
      if (variables != null && this._options.HasFlag((Enum) ExpressiveOptions.IgnoreCase))
        variables = (IDictionary<string, object>) new Dictionary<string, object>(variables, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      return this._compiledExpression == null ? (object) null : this._compiledExpression.Evaluate(variables);
    }
    catch (Exception ex)
    {
      throw new ExpressiveException(ex);
    }
  }

  public void EvaluateAsync(Action<string, object> callback)
  {
    this.EvaluateAsync(callback, (IDictionary<string, object>) null);
  }

  public void EvaluateAsync(Action<string, object> callback, IDictionary<string, object> variables)
  {
    if (callback == null)
      throw new ArgumentNullException(nameof (callback));
    ThreadPool.QueueUserWorkItem((WaitCallback) (o =>
    {
      object obj = (object) null;
      string str = (string) null;
      try
      {
        obj = this.Evaluate(variables);
      }
      catch (Exception ex)
      {
        str = ex.Message;
      }
      if (callback == null)
        return;
      callback(str, obj);
    }));
  }

  public void RegisterFunction(
    string functionName,
    Func<IExpression[], IDictionary<string, object>, object> function)
  {
    this._parser.RegisterFunction(functionName, function);
  }

  public void RegisterFunction(IFunction function) => this._parser.RegisterFunction(function);

  public void CompileExpression()
  {
    if (this._compiledExpression != null && !this._options.HasFlag((Enum) ExpressiveOptions.NoCache))
      return;
    List<string> variables = new List<string>();
    this._compiledExpression = this._parser.CompileExpression(this._originalExpression, (IList<string>) variables);
    this._variables = variables.ToArray();
  }
}
