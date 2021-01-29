using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Iface.Oik.Tm.Interfaces;
using OfficeOpenXml;

namespace Iface.Oik.ScriptEngine.Workers
{
  public class ExcelWorker : Worker
  {
    private readonly ExcelPackage _excelPackage;

    private readonly List<CommentCell> _commentCells = new();


    public ExcelWorker(IOikDataApi api, string filename) : base(api, filename)
    {
      _excelPackage = new ExcelPackage(new FileInfo(filename));
      
      LoadCustomFunctions();

      LoadCommentCells();
    }


    private void LoadCustomFunctions()
    {
      _excelPackage.Workbook.FormulaParserManager.LoadFunctionModule(new ExcelWorkerFunctions(this));
    }


    private void LoadCommentCells()
    {
      foreach (var worksheet in _excelPackage.Workbook.Worksheets)
      {
        for (var i = 0; i < worksheet.Comments.Count; i++)
        {
          if (!string.IsNullOrWhiteSpace(worksheet.Comments[i].Text))
          {
            _commentCells.Add(new CommentCell(worksheet.Cells[worksheet.Comments[i].Address],
                                              worksheet.Comments[i].Text));
          }
        }
      }
    }


    protected override void DoWork()
    {
      foreach (var commentCell in _commentCells)
      {
        TryFetchValue(commentCell);
      }

      _excelPackage.Workbook.Calculate();

      foreach (var commentCell in _commentCells)
      {
        TryUploadValue(commentCell);
      }
    }


    private bool TryFetchValue(CommentCell commentCell)
    {
      var (cell, commentText) = commentCell;
      Match match;

      match = new Regex(@"GetTmAnalog\((\d+);(\d+);(\d+)\)", RegexOptions.IgnoreCase).Match(commentText);
      if (match.Success)
      {
        cell.Value = (double) GetTmAnalog(CreateTmAddrFromRegexp(TmType.Analog, match.Groups));
        return true;
      }

      match = new Regex(@"GetTmStatus\((\d+);(\d+);(\d+)\)", RegexOptions.IgnoreCase).Match(commentText);
      if (match.Success)
      {
        cell.Value = (double) GetTmStatus(CreateTmAddrFromRegexp(TmType.Status, match.Groups));
        return true;
      }

      match = new Regex(@"TM\((.*)\)", RegexOptions.IgnoreCase).Match(commentText);
      if (match.Success)
      {
        return TryFetchExpressionResult(cell, match.Groups[1].Value);
      }

      if (commentText.FirstOrDefault() == '<')
      {
        return TryFetchExpressionResult(cell, commentText.Remove(0, 1));
      }

      return false;
    }


    private bool TryFetchExpressionResult(ExcelRange cell, string expression)
    {
      var expressionResult = GetExpressionResult(expression);
      if (!float.TryParse(expressionResult, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatValue))
      {
        LogDebug(expressionResult);
        SetCellError(cell);
        return false;
      }
      cell.Value = (double) floatValue;
      return true;
    }


    private bool TryUploadValue(CommentCell commentCell)
    {
      var (cell, commentText) = commentCell;
      Match match;

      match = new Regex(@"SetTmAnalog\((\d+);(\d+);(\d+)\)", RegexOptions.IgnoreCase).Match(commentText);
      if (match.Success)
      {
        return TrySetTmAnalog(cell, CreateTmAddrFromRegexp(TmType.Analog, match.Groups));
      }

      match = new Regex(@"SetTmStatus\((\d+);(\d+);(\d+)\)", RegexOptions.IgnoreCase).Match(commentText);
      if (match.Success)
      {
        return TrySetTmStatus(cell, CreateTmAddrFromRegexp(TmType.Status, match.Groups));
      }

      if (commentText.FirstOrDefault() == '>' && TmAddr.TryParse(commentText.Remove(0, 1), out var tmAddr))
      {
        switch (tmAddr.Type)
        {
          case TmType.Status:
            return TrySetTmStatus(cell, tmAddr);

          case TmType.Analog:
            return TrySetTmAnalog(cell, tmAddr);
        }
      }

      return false;
    }


    private static TmAddr CreateTmAddrFromRegexp(TmType type, GroupCollection groups)
    {
      return new TmAddr(type, int.Parse(groups[1].Value), int.Parse(groups[2].Value), int.Parse(groups[3].Value));
    }


    private static void SetCellError(ExcelRange cell) // todo подумать что-то лучше
    {
      cell.Formula = "ERROR";
    }


    private bool TrySetTmStatus(ExcelRange cell, TmAddr addr)
    {
      try
      {
        SetTmStatus(addr, cell.GetValue<int>());
        return true;
      }
      catch (Exception)
      {
        LogDebug($"Из-за некорректного значения ячейки {cell.Address} телепараметр {addr} станет недостоверным");
        SetTmStatusFlags(addr, TmFlags.Unreliable);
        return false;
      }
    }


    private bool TrySetTmAnalog(ExcelRangeBase cell, TmAddr addr)
    {
      try
      {
        SetTmAnalog(addr, cell.GetValue<float>());
        return true;
      }
      catch (Exception)
      {
        LogDebug($"Из-за некорректного значения ячейки {cell.Address} телепараметр {addr} станет недостоверным");
        SetTmAnalogFlags(addr, TmFlags.Unreliable);
        return false;
      }
    }


    public override Task StopAsync(CancellationToken cancellationToken)
    {
      _excelPackage?.Dispose();
      return base.StopAsync(cancellationToken);
    }


    private record CommentCell(ExcelRange Cell, string CommentText);
  }
}