// Decompiled with JetBrains decompiler
// Type: Expressive.ExpressionParser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;
using Expressive.Functions.Date;
using Expressive.Functions.Logical;
using Expressive.Functions.Mathematical;
using Expressive.Functions.Statistical;
using Expressive.Functions.String;
using Expressive.Operators;
using Expressive.Operators.Additive;
using Expressive.Operators.Bitwise;
using Expressive.Operators.Conditional;
using Expressive.Operators.Grouping;
using Expressive.Operators.Logic;
using Expressive.Operators.Multiplicative;
using Expressive.Operators.Relational;
using LinqTools;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Expressive;

public sealed class ExpressionParser
{
  public const char DateSeparator = '#';
  public const char ParameterSeparator = ',';
  public char _decimalSeparator;
  public ExpressiveOptions _options;
  public IDictionary<string, Func<IExpression[], IDictionary<string, object>, object>> _registeredFunctions;
  public IDictionary<string, IOperator> _registeredOperators;
  public StringComparer _stringComparer;

  public ExpressionParser(ExpressiveOptions options)
  {
    this._options = options;
    this._stringComparer = this._options.HasFlag((Enum) ExpressiveOptions.IgnoreCase) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    this._decimalSeparator = '.';
    this._registeredFunctions = (IDictionary<string, Func<IExpression[], IDictionary<string, object>, object>>) new Dictionary<string, Func<IExpression[], IDictionary<string, object>, object>>((IEqualityComparer<string>) this.GetDictionaryComparer(options));
    this._registeredOperators = (IDictionary<string, IOperator>) new Dictionary<string, IOperator>((IEqualityComparer<string>) this.GetDictionaryComparer(options));
    this.RegisterOperator((IOperator) new PlusOperator());
    this.RegisterOperator((IOperator) new SubtractOperator());
    this.RegisterOperator((IOperator) new BitwiseAndOperator());
    this.RegisterOperator((IOperator) new BitwiseOrOperator());
    this.RegisterOperator((IOperator) new BitwiseXOrOperator());
    this.RegisterOperator((IOperator) new LeftShiftOperator());
    this.RegisterOperator((IOperator) new RightShiftOperator());
    this.RegisterOperator((IOperator) new NullCoalescingOperator());
    this.RegisterOperator((IOperator) new ParenthesisCloseOperator());
    this.RegisterOperator((IOperator) new ParenthesisOpenOperator());
    this.RegisterOperator((IOperator) new AndOperator());
    this.RegisterOperator((IOperator) new NotOperator());
    this.RegisterOperator((IOperator) new OrOperator());
    this.RegisterOperator((IOperator) new DivideOperator());
    this.RegisterOperator((IOperator) new ModulusOperator());
    this.RegisterOperator((IOperator) new MultiplyOperator());
    this.RegisterOperator((IOperator) new EqualOperator());
    this.RegisterOperator((IOperator) new GreaterThanOperator());
    this.RegisterOperator((IOperator) new GreaterThanOrEqualOperator());
    this.RegisterOperator((IOperator) new LessThanOperator());
    this.RegisterOperator((IOperator) new LessThanOrEqualOperator());
    this.RegisterOperator((IOperator) new NotEqualOperator());
    this.RegisterFunction((IFunction) new AddDaysFunction());
    this.RegisterFunction((IFunction) new AddHoursFunction());
    this.RegisterFunction((IFunction) new AddMillisecondsFunction());
    this.RegisterFunction((IFunction) new AddMinutesFunction());
    this.RegisterFunction((IFunction) new AddMonthsFunction());
    this.RegisterFunction((IFunction) new AddSecondsFunction());
    this.RegisterFunction((IFunction) new AddYearsFunction());
    this.RegisterFunction((IFunction) new DayOfFunction());
    this.RegisterFunction((IFunction) new DaysBetweenFunction());
    this.RegisterFunction((IFunction) new HourOfFunction());
    this.RegisterFunction((IFunction) new HoursBetweenFunction());
    this.RegisterFunction((IFunction) new MillisecondOfFunction());
    this.RegisterFunction((IFunction) new MillisecondsBetweenFunction());
    this.RegisterFunction((IFunction) new MinuteOfFunction());
    this.RegisterFunction((IFunction) new MinutesBetweenFunction());
    this.RegisterFunction((IFunction) new MonthOfFunction());
    this.RegisterFunction((IFunction) new SecondOfFunction());
    this.RegisterFunction((IFunction) new SecondsBetweenFunction());
    this.RegisterFunction((IFunction) new YearOfFunction());
    this.RegisterFunction((IFunction) new AbsFunction());
    this.RegisterFunction((IFunction) new AcosFunction());
    this.RegisterFunction((IFunction) new AsinFunction());
    this.RegisterFunction((IFunction) new AtanFunction());
    this.RegisterFunction((IFunction) new CeilingFunction());
    this.RegisterFunction((IFunction) new CosFunction());
    this.RegisterFunction((IFunction) new CountFunction());
    this.RegisterFunction((IFunction) new ExpFunction());
    this.RegisterFunction((IFunction) new FloorFunction());
    this.RegisterFunction((IFunction) new IEEERemainderFunction());
    this.RegisterFunction((IFunction) new Log10Function());
    this.RegisterFunction((IFunction) new LogFunction());
    this.RegisterFunction((IFunction) new MaxFunction());
    this.RegisterFunction((IFunction) new MinFunction());
    this.RegisterFunction((IFunction) new PowFunction());
    this.RegisterFunction((IFunction) new RoundFunction());
    this.RegisterFunction((IFunction) new SignFunction());
    this.RegisterFunction((IFunction) new SinFunction());
    this.RegisterFunction((IFunction) new SqrtFunction());
    this.RegisterFunction((IFunction) new SumFunction());
    this.RegisterFunction((IFunction) new TanFunction());
    this.RegisterFunction((IFunction) new TruncateFunction());
    this.RegisterFunction((IFunction) new IfFunction());
    this.RegisterFunction((IFunction) new InFunction());
    this.RegisterFunction((IFunction) new AverageFunction());
    this.RegisterFunction((IFunction) new MeanFunction());
    this.RegisterFunction((IFunction) new MedianFunction());
    this.RegisterFunction((IFunction) new ModeFunction());
    this.RegisterFunction((IFunction) new LengthFunction());
    this.RegisterFunction((IFunction) new PadLeftFunction());
    this.RegisterFunction((IFunction) new PadRightFunction());
    this.RegisterFunction((IFunction) new RegexFunction());
    this.RegisterFunction((IFunction) new SubstringFunction());
  }

  public IExpression CompileExpression(string expression, IList<string> variables)
  {
    IList<Token> tokenList = !expression.IsNullOrWhiteSpace() ? this.Tokenise(expression) : throw new ExpressiveException("An Expression cannot be empty.");
    int num1 = tokenList.Select<Token, string>((Func<Token, string>) (t => t.CurrentToken)).Count<string>((Func<string, bool>) (t => string.Equals(t, "(", StringComparison.Ordinal)));
    int num2 = tokenList.Select<Token, string>((Func<Token, string>) (t => t.CurrentToken)).Count<string>((Func<string, bool>) (t => string.Equals(t, ")", StringComparison.Ordinal)));
    if (num1 > num2)
      throw new ArgumentException($"There aren't enough ')' symbols. Expected {num1.ToString()} but there is only {num2.ToString()}");
    if (num1 < num2)
      throw new ArgumentException($"There are too many ')' symbols. Expected {num1.ToString()} but there is {num2.ToString()}");
    return this.CompileExpression(new Queue<Token>((IEnumerable<Token>) tokenList), OperatorPrecedence.Minimum, variables, false);
  }

  public void RegisterFunction(
    string functionName,
    Func<IExpression[], IDictionary<string, object>, object> function)
  {
    this.CheckForExistingFunctionName(functionName);
    this._registeredFunctions.Add(functionName, function);
  }

  public void RegisterFunction(IFunction function)
  {
    this.CheckForExistingFunctionName(function.Name);
    this._registeredFunctions.Add(function.Name, (Func<IExpression[], IDictionary<string, object>, object>) ((p, a) =>
    {
      function.Variables = a;
      return function.Evaluate(p);
    }));
  }

  public void RegisterOperator(IOperator op)
  {
    foreach (string tag in op.Tags)
      this._registeredOperators.Add(tag, op);
  }

  public void UnregisterFunction(string name)
  {
    if (!this._registeredFunctions.ContainsKey(name))
      return;
    this._registeredFunctions.Remove(name);
  }

  public IExpression CompileExpression(
    Queue<Token> tokens,
    OperatorPrecedence minimumPrecedence,
    IList<string> variables,
    bool isWithinFunction)
  {
    if (tokens == null)
      throw new ArgumentNullException(nameof (tokens), "You must call Tokenise before compiling");
    IExpression participant = (IExpression) null;
    Token token1 = tokens.PeekOrDefault<Token>();
    Token previousToken = (Token) null;
    for (; token1 != null; token1 = tokens.PeekOrDefault<Token>())
    {
      Func<IExpression[], IDictionary<string, object>, object> function = (Func<IExpression[], IDictionary<string, object>, object>) null;
      IOperator @operator = (IOperator) null;
      char ch;
      if (this._registeredOperators.TryGetValue(token1.CurrentToken, out @operator))
      {
        OperatorPrecedence precedence = @operator.GetPrecedence(previousToken);
        if (precedence > minimumPrecedence)
        {
          tokens.Dequeue();
          if (!@operator.CanGetCaptiveTokens(previousToken, token1, tokens))
          {
            @operator.GetCaptiveTokens(previousToken, token1, tokens);
            break;
          }
          Token[] captiveTokens = @operator.GetCaptiveTokens(previousToken, token1, tokens);
          IExpression expression;
          if (captiveTokens.Length > 1)
          {
            expression = this.CompileExpression(new Queue<Token>((IEnumerable<Token>) @operator.GetInnerCaptiveTokens(captiveTokens)), OperatorPrecedence.Minimum, variables, isWithinFunction);
            token1 = captiveTokens[captiveTokens.Length - 1];
          }
          else
          {
            expression = this.CompileExpression(tokens, precedence, variables, isWithinFunction);
            token1 = new Token(")", -1);
          }
          participant = @operator.BuildExpression(previousToken, new IExpression[2]
          {
            participant,
            expression
          });
        }
        else
          break;
      }
      else if (this._registeredFunctions.TryGetValue(token1.CurrentToken, out function))
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        List<IExpression> expressionList = new List<IExpression>();
        Queue<Token> tokenQueue = new Queue<Token>();
        int num = 0;
        tokens.Dequeue();
        while (tokens.Count > 0)
        {
          Token token2 = tokens.Dequeue();
          if (string.Equals(token2.CurrentToken, "(", StringComparison.Ordinal))
            ++num;
          else if (string.Equals(token2.CurrentToken, ")", StringComparison.Ordinal))
            --num;
          if ((num != 1 || !(token2.CurrentToken == "(")) && (num != 0 || !(token2.CurrentToken == ")")))
            tokenQueue.Enqueue(token2);
          if (num == 0 && tokenQueue.Any<Token>())
          {
            expressionList.Add(this.CompileExpression(tokenQueue, OperatorPrecedence.Minimum, variables, true));
            tokenQueue.Clear();
          }
          else
          {
            string currentToken = token2.CurrentToken;
            ch = ',';
            string b = ch.ToString();
            if (string.Equals(currentToken, b, StringComparison.Ordinal) && num == 1)
            {
              expressionList.Add(this.CompileExpression(tokenQueue, OperatorPrecedence.Minimum, variables, true));
              tokenQueue.Clear();
            }
          }
          if (num <= 0)
            break;
        }
        participant = (IExpression) new FunctionExpression(token1.CurrentToken, function, expressionList.ToArray());
      }
      else if (token1.CurrentToken.IsNumeric())
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        tokens.Dequeue();
        int result1 = 0;
        Decimal result2 = 0.0M;
        double result3 = 0.0;
        float result4 = 0.0f;
        long result5 = 0;
        if (int.TryParse(token1.CurrentToken, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Integer, (object) result1);
        else if (Decimal.TryParse(token1.CurrentToken, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Decimal, (object) result2);
        else if (double.TryParse(token1.CurrentToken, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Double, (object) result3);
        else if (float.TryParse(token1.CurrentToken, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result4))
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Float, (object) result4);
        else if (long.TryParse(token1.CurrentToken, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result5))
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Long, (object) result5);
      }
      else if (token1.CurrentToken.StartsWith("[") && token1.CurrentToken.EndsWith("]"))
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        tokens.Dequeue();
        string variableName = token1.CurrentToken.Replace("[", "").Replace("]", "");
        participant = (IExpression) new VariableExpression(variableName);
        if (!variables.Contains<string>(variableName, (IEqualityComparer<string>) this._stringComparer))
          variables.Add(variableName);
      }
      else if (string.Equals(token1.CurrentToken, "true", StringComparison.OrdinalIgnoreCase))
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        tokens.Dequeue();
        participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Boolean, (object) true);
      }
      else if (string.Equals(token1.CurrentToken, "false", StringComparison.OrdinalIgnoreCase))
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        tokens.Dequeue();
        participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Boolean, (object) false);
      }
      else if (string.Equals(token1.CurrentToken, "null", StringComparison.OrdinalIgnoreCase))
      {
        this.CheckForExistingParticipant(participant, token1, isWithinFunction);
        tokens.Dequeue();
        participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.Null, (object) null);
      }
      else
      {
        string currentToken1 = token1.CurrentToken;
        ch = '#';
        string str1 = ch.ToString();
        if (currentToken1.StartsWith(str1))
        {
          string currentToken2 = token1.CurrentToken;
          ch = '#';
          string str2 = ch.ToString();
          if (currentToken2.EndsWith(str2))
          {
            this.CheckForExistingParticipant(participant, token1, isWithinFunction);
            tokens.Dequeue();
            string currentToken3 = token1.CurrentToken;
            ch = '#';
            string oldValue = ch.ToString();
            string str3 = currentToken3.Replace(oldValue, "");
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(str3, out result))
            {
              if (string.Equals("TODAY", str3, StringComparison.OrdinalIgnoreCase))
              {
                result = DateTime.Today;
              }
              else
              {
                if (!string.Equals("NOW", str3, StringComparison.OrdinalIgnoreCase))
                  throw new UnrecognisedTokenException(str3);
                result = DateTime.Now;
              }
            }
            participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.DateTime, (object) result);
            goto label_62;
          }
        }
        if (token1.CurrentToken.StartsWith("'") && token1.CurrentToken.EndsWith("'") || token1.CurrentToken.StartsWith("\"") && token1.CurrentToken.EndsWith("\""))
        {
          this.CheckForExistingParticipant(participant, token1, isWithinFunction);
          tokens.Dequeue();
          participant = (IExpression) new ConstantValueExpression(ConstantValueExpressionType.String, (object) ExpressionParser.CleanString(token1.CurrentToken.Substring(1, token1.Length - 2)));
        }
        else
        {
          string currentToken4 = token1.CurrentToken;
          ch = ',';
          string b = ch.ToString();
          if (string.Equals(currentToken4, b, StringComparison.Ordinal))
          {
            if (!isWithinFunction)
              throw new ExpressiveException($"Unexpected token '{token1}'");
            tokens.Dequeue();
          }
          else
          {
            tokens.Dequeue();
            throw new UnrecognisedTokenException(token1.CurrentToken);
          }
        }
      }
label_62:
      previousToken = token1;
    }
    return participant;
  }

  public static string CleanString(string input)
  {
    if (input.Length <= 1)
      return input;
    char[] chArray = new char[input.Length];
    int length = 0;
    for (int index = 0; index < input.Length; ++index)
    {
      char ch = input[index];
      if (ch == '\\' && index < input.Length - 1)
      {
        switch (input[index + 1])
        {
          case '\'':
            chArray[length++] = '\'';
            ++index;
            continue;
          case 'n':
            chArray[length++] = '\n';
            ++index;
            continue;
          case 'r':
            chArray[length++] = '\r';
            ++index;
            continue;
          case 't':
            chArray[length++] = '\t';
            ++index;
            continue;
        }
      }
      chArray[length++] = ch;
    }
    return new string(chArray, 0, length);
  }

  public static bool CanExtractValue(
    string expression,
    int expressionLength,
    int index,
    string value)
  {
    return string.Equals(value, ExpressionParser.ExtractValue(expression, expressionLength, index, value), StringComparison.OrdinalIgnoreCase);
  }

  public static bool CanGetString(string expression, int startIndex, char quoteCharacter)
  {
    return !ExpressionParser.GetString(expression, startIndex, quoteCharacter).IsNullOrWhiteSpace();
  }

  public void CheckForExistingFunctionName(string functionName)
  {
    if (this._registeredFunctions.ContainsKey(functionName))
      throw new FunctionNameAlreadyRegisteredException(functionName);
  }

  public void CheckForExistingParticipant(
    IExpression participant,
    Token token,
    bool isWithinFunction)
  {
    if (participant == null)
      return;
    if (isWithinFunction)
      throw new MissingTokenException("Missing token, expecting ','.", ',');
    throw new ExpressiveException($"Unexpected token '{token.CurrentToken}' at index {token.StartIndex}");
  }

  public static bool CheckForTag(string tag, string lookAhead, ExpressiveOptions options)
  {
    return options.HasFlag((Enum) ExpressiveOptions.IgnoreCase) && string.Equals(lookAhead, tag, StringComparison.OrdinalIgnoreCase) || string.Equals(lookAhead, tag, StringComparison.Ordinal);
  }

  public static string ExtractValue(
    string expression,
    int expressionLength,
    int index,
    string expectedValue)
  {
    string str = (string) null;
    int length = expectedValue.Length;
    if (expressionLength >= index + length)
    {
      string a = expression.Substring(index, length);
      bool flag = true;
      if (expressionLength > index + length)
        flag = !char.IsLetterOrDigit(expression[index + length]);
      if (string.Equals(a, expectedValue, StringComparison.OrdinalIgnoreCase) & flag)
        str = a;
    }
    return str;
  }

  public StringComparer GetDictionaryComparer(ExpressiveOptions options)
  {
    return !options.HasFlag((Enum) ExpressiveOptions.IgnoreCase) ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
  }

  public string GetNumber(string expression, int startIndex)
  {
    bool flag = false;
    int index = startIndex;
    for (char c = expression[index]; index < expression.Length && (char.IsDigit(c) || !flag && (int) c == (int) this._decimalSeparator); c = expression[index])
    {
      if (!flag && (int) c == (int) this._decimalSeparator)
        flag = true;
      ++index;
      if (index == expression.Length)
        break;
    }
    return expression.Substring(startIndex, index - startIndex);
  }

  public static string GetString(string expression, int startIndex, char quoteCharacter)
  {
    int index = startIndex;
    bool flag = false;
    char ch1 = expression[index];
    char ch2 = char.MinValue;
    while (index < expression.Length && !flag)
    {
      if (index != startIndex && (int) ch1 == (int) quoteCharacter && ch2 != '\\')
        flag = true;
      ch2 = ch1;
      ++index;
      if (index != expression.Length)
        ch1 = expression[index];
      else
        break;
    }
    return flag ? expression.Substring(startIndex, index - startIndex) : (string) null;
  }

  public static bool IsQuote(char character) => character == '\'' || character == '"';

  public IList<Token> Tokenise(string expression)
  {
    if (expression.IsNullOrWhiteSpace())
      return (IList<Token>) null;
    int length = expression.Length;
    IOrderedEnumerable<KeyValuePair<string, IOperator>, int> orderedEnumerable = this._registeredOperators.OrderByDescending<KeyValuePair<string, IOperator>, int>((Func<KeyValuePair<string, IOperator>, int>) (op => op.Key.Length));
    List<Token> tokens = new List<Token>();
    IList<char> unrecognised = (IList<char>) null;
    int num1;
    int num2;
    for (num1 = 0; num1 < length; num1 += num2 == 0 ? 1 : num2)
    {
      num2 = 0;
      bool flag = false;
      foreach (KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>> registeredFunction in (IEnumerable<KeyValuePair<string, Func<IExpression[], IDictionary<string, object>, object>>>) this._registeredFunctions)
      {
        string str = expression.Substring(num1, Math.Min(registeredFunction.Key.Length, length - num1));
        if (ExpressionParser.CheckForTag(registeredFunction.Key, str, this._options))
        {
          ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
          num2 = registeredFunction.Key.Length;
          tokens.Add(new Token(str, num1));
          break;
        }
      }
      if (num2 == 0)
      {
        foreach (KeyValuePair<string, IOperator> keyValuePair in (IEnumerable<KeyValuePair<string, IOperator>>) orderedEnumerable)
        {
          string str = expression.Substring(num1, Math.Min(keyValuePair.Key.Length, length - num1));
          if (ExpressionParser.CheckForTag(keyValuePair.Key, str, this._options))
          {
            ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
            num2 = keyValuePair.Key.Length;
            tokens.Add(new Token(str, num1));
            break;
          }
        }
      }
      if (num2 == 0)
      {
        char ch1 = expression[num1];
        if (ch1 == '[')
        {
          char ch2 = ']';
          if (!ExpressionParser.CanGetString(expression, num1, ch2))
            throw new MissingTokenException($"Missing closing token '{ch2}'", ch2);
          string currentToken = expression.SubstringUpTo(num1, ch2);
          ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
          tokens.Add(new Token(currentToken, num1));
          num2 = currentToken.Length;
        }
        else if (char.IsDigit(ch1))
        {
          string number = this.GetNumber(expression, num1);
          ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
          tokens.Add(new Token(number, num1));
          num2 = number.Length;
        }
        else if (ExpressionParser.IsQuote(ch1))
        {
          if (!ExpressionParser.CanGetString(expression, num1, ch1))
            throw new MissingTokenException($"Missing closing token '{ch1}'", ch1);
          string currentToken = ExpressionParser.GetString(expression, num1, ch1);
          ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
          tokens.Add(new Token(currentToken, num1));
          num2 = currentToken.Length;
        }
        else
        {
          switch (ch1)
          {
            case '#':
              if (!ExpressionParser.CanGetString(expression, num1, ch1))
                throw new MissingTokenException($"Missing closing token '{ch1}'", ch1);
              string currentToken1 = "#" + expression.SubstringUpTo(num1 + 1, '#');
              ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
              tokens.Add(new Token(currentToken1, num1));
              num2 = currentToken1.Length;
              break;
            case ',':
              ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
              tokens.Add(new Token(ch1.ToString(), num1));
              num2 = 1;
              break;
            default:
              if ((ch1 == 't' || ch1 == 'T') && ExpressionParser.CanExtractValue(expression, length, num1, "true"))
              {
                ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
                string currentToken2 = ExpressionParser.ExtractValue(expression, length, num1, "true");
                if (!currentToken2.IsNullOrWhiteSpace())
                {
                  tokens.Add(new Token(currentToken2, num1));
                  num2 = 4;
                  break;
                }
                break;
              }
              if ((ch1 == 'f' || ch1 == 'F') && ExpressionParser.CanExtractValue(expression, length, num1, "false"))
              {
                ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
                string currentToken3 = ExpressionParser.ExtractValue(expression, length, num1, "false");
                if (!currentToken3.IsNullOrWhiteSpace())
                {
                  tokens.Add(new Token(currentToken3, num1));
                  num2 = 5;
                  break;
                }
                break;
              }
              if (ch1 == 'n' && ExpressionParser.CanExtractValue(expression, length, num1, "null"))
              {
                ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
                string currentToken4 = ExpressionParser.ExtractValue(expression, length, num1, "null");
                if (!currentToken4.IsNullOrWhiteSpace())
                {
                  tokens.Add(new Token(currentToken4, num1));
                  num2 = 4;
                  break;
                }
                break;
              }
              if (!char.IsWhiteSpace(ch1))
              {
                if (unrecognised == null)
                  unrecognised = (IList<char>) new List<char>();
                flag = true;
                unrecognised.Add(ch1);
                break;
              }
              break;
          }
        }
      }
      if (!flag)
      {
        ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
        unrecognised = (IList<char>) null;
      }
    }
    ExpressionParser.CheckForUnrecognised(unrecognised, (IList<Token>) tokens, num1);
    return (IList<Token>) tokens;
  }

  public static void CheckForUnrecognised(IList<char> unrecognised, IList<Token> tokens, int index)
  {
    if (unrecognised == null)
      return;
    string currentToken = new string(unrecognised.ToArray<char>());
    tokens.Add(new Token(currentToken, index - currentToken.Length));
  }
}
