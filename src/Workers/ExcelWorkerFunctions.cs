using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Iface.Oik.Tm.Interfaces;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class ExcelWorkerFunctions : FunctionsModule
  {
    public ExcelWorkerFunctions(ExcelWorker worker)
    {
      Functions.Add("TM",          new Tm(worker));
      Functions.Add("GetTmAnalog", new GetTmAnalog(worker));
      Functions.Add("GetTmStatus", new GetTmStatus(worker));
      Functions.Add("SetTmAnalog", new SetTmAnalog(worker));
      Functions.Add("SetTmStatus", new SetTmStatus(worker));
    }


    private class Tm : ExcelFunction
    {
      private readonly ExcelWorker _worker;


      public Tm(ExcelWorker worker)
      {
        _worker = worker;
      }


      public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
      {
        var functionArguments = arguments.ToList();
        ValidateArguments(functionArguments, 1);

        var expression = ArgToString(functionArguments, 0);

        var expressionResult = _worker.GetExpressionResult(expression);
        if (!float.TryParse(expressionResult, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue))
        {
          _worker.LogDebug(expressionResult);
          throw new Exception();
        }
        return CreateResult((double) floatValue, DataType.Decimal);
      }
    }


    private class GetTmAnalog : ExcelFunction
    {
      private readonly ExcelWorker _worker;


      public GetTmAnalog(ExcelWorker worker)
      {
        _worker = worker;
      }


      public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
      {
        var functionArguments = arguments.ToList();
        ValidateArguments(functionArguments, 3);

        var addrParts = ArgsToDoubleEnumerable(functionArguments, context).ToList();

        var addr = new TmAddr(TmType.Analog,
                              (int) addrParts[0].Value,
                              (int) addrParts[1].Value,
                              (int) addrParts[2].Value);

        return CreateResult((double) _worker.GetTmAnalog(addr), DataType.Decimal);
      }
    }


    private class GetTmStatus : ExcelFunction
    {
      private readonly ExcelWorker _worker;


      public GetTmStatus(ExcelWorker worker)
      {
        _worker = worker;
      }


      public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
      {
        var functionArguments = arguments.ToList();
        ValidateArguments(functionArguments, 3);

        var addrParts = ArgsToDoubleEnumerable(functionArguments, context).ToList();

        var addr = new TmAddr(TmType.Status,
                              (int) addrParts[0].Value,
                              (int) addrParts[1].Value,
                              (int) addrParts[2].Value);

        return CreateResult((double) _worker.GetTmStatus(addr), DataType.Decimal);
      }
    }


    private class SetTmAnalog : ExcelFunction
    {
      private readonly ExcelWorker _worker;


      public SetTmAnalog(ExcelWorker worker)
      {
        _worker = worker;
      }


      public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
      {
        var functionArguments = arguments.ToList();
        ValidateArguments(functionArguments, 4);

        var addrParts = ArgsToDoubleEnumerable(functionArguments, context).ToList();

        var addr = new TmAddr(TmType.Analog,
                              (int) addrParts[0].Value,
                              (int) addrParts[1].Value,
                              (int) addrParts[2].Value);
        try
        {
          _worker.SetTmAnalog(addr, (float) addrParts[3].Value);
          return CreateResult(true, DataType.Boolean);
        }
        catch (Exception)
        {
          _worker.LogDebug(
            $"Из-за некорректного значения ячейки {context.Scopes.Current.Address} телепараметр {addr} станет недостоверным");
          _worker.SetTmAnalogFlags(addr, TmFlags.Unreliable);
          return CreateResult(false, DataType.Boolean);
        }
      }
    }


    private class SetTmStatus : ExcelFunction
    {
      private readonly ExcelWorker _worker;


      public SetTmStatus(ExcelWorker worker)
      {
        _worker = worker;
      }


      public override CompileResult Execute(IEnumerable<FunctionArgument> arguments, ParsingContext context)
      {
        var functionArguments = arguments.ToList();
        ValidateArguments(functionArguments, 4);

        var addrParts = ArgsToDoubleEnumerable(functionArguments, context).ToList();

        var addr = new TmAddr(TmType.Status,
                              (int) addrParts[0].Value,
                              (int) addrParts[1].Value,
                              (int) addrParts[2].Value);
        try
        {
          _worker.SetTmStatus(addr, (int) addrParts[3].Value);
          return CreateResult(true, DataType.Boolean);
        }
        catch (Exception)
        {
          _worker.LogDebug(
            $"Из-за некорректного значения ячейки {context.Scopes.Current.Address} телепараметр {addr} станет недостоверным");
          _worker.SetTmStatusFlags(addr, TmFlags.Unreliable);
          return CreateResult(false, DataType.Boolean);
        }
      }
    }
  }
}