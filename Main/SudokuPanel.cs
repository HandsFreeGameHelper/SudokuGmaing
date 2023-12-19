using SudokuPanel.Utlis;
using System;

namespace SudokuPanel.Main;

public class Sudoku
{
  /// <summary>
  /// 待解决棋盘
  /// </summary>
  public int[][] OriginPanel { get; set; } = new int[][] { };

  /// <summary>
  /// 调用 Generater 方法
  /// </summary>
  /// <param name="args"></param>
  public void GenerateSudoku()
  {
    this.GenerateOrResolveSudoku(new Properties());
  }

  /// <summary>  
  /// [0, 0, 0, 7, 0, 0, 0, 0, 0]
  /// [0, 0, 5, 0, 0, 0, 0, 0, 0]
  /// [0, 0, 0, 0, 0, 0, 0, 5, 0]
  /// [0, 0, 0, 0, 2, 0, 0, 0, 0]
  /// [0, 8, 0, 0, 0, 0, 0, 0, 0]
  /// [0, 0, 0, 0, 0, 0, 0, 8, 0]
  /// [0, 0, 0, 0, 0, 0, 0, 0, 0]
  /// [0, 0, 0, 0, 1, 0, 0, 0, 0]
  /// [0, 9, 0, 0, 0, 0, 0, 0, 6]
  ///
  /// this.OriginPanel = new int[][]
  /// {
  ///     new int[] { 0, 0, 0, 7, 0, 0, 0, 0, 0},
  ///     new int[] { 0, 0, 5, 0, 0, 0, 0, 0, 0},
  ///     new int[] { 0, 0, 0, 0, 0, 0, 0, 5, 0},
  ///     new int[] { 0, 0, 0, 0, 2, 0, 0, 0, 0},
  ///     new int[] { 0, 8, 0, 0, 0, 0, 0, 0, 0},
  ///     new int[] { 0, 0, 0, 0, 0, 0, 0, 8, 0},
  ///     new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
  ///     new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0},
  ///     new int[] { 0, 9, 0, 0, 0, 0, 0, 0, 6}
  /// };
  /// </summary>
  public void ResolveSudoku()
  {
    this.GenerateOrResolveSudoku(new Properties(), true);
  }

  /// <summary>
  /// 使用了一个循环，不断地生成数独面板，直到生成成功。
  /// 在每次生成尝试之前，清空控制台。
  /// 调用 TryGenerateSudoku 尝试生成完整的数独面板。
  /// 最终输出生成的数独面板。
  /// </summary>
  private void GenerateOrResolveSudoku(Properties property, bool isOnlyResolve = false)
  {
    var dateTimeStart = DateTime.Now;
    while (true)
    {
      Console.Clear();
      var success = isOnlyResolve ? this.TryResolveSudoku(property) : this.TryGenerateSudoku(property);
      if (success)
      {
        Console.Clear();
        Console.WriteLine("Match Successed, The Panel is");
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine(property.Rows.AsPrimitive());
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("Total Taken {0}", (DateTime.Now - dateTimeStart).ToString());
        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        break;
      }
      Console.WriteLine("Match Failed");
      Console.WriteLine(Environment.NewLine);
      Thread.Sleep(1);
    }
  }

  /// <summary>
  /// 调用 InitData 方法来初始化数据。
  /// 调用 FillByRows 方法填充每个九宫格的其余部分。
  /// </summary>
  /// <returns></returns>
  private bool TryGenerateSudoku(Properties property, bool isOnlyResolve = false)
  {
    try
    {
      this.InitData(property);

      Console.WriteLine(property.Rows.AsPrimitive());
      Console.WriteLine(Environment.NewLine);

      var success = this.FillByRows(0, 3, 0, 3, property) &&
                    this.FillByRows(0, 3, 3, 6, property) &&
                    this.FillByRows(0, 3, 6, 9, property) &&
                    this.FillByRows(3, 6, 0, 3, property) &&
                    this.FillByRows(3, 6, 3, 6, property) &&
                    this.FillByRows(3, 6, 6, 9, property) &&
                    this.FillByRows(6, 9, 0, 3, property) &&
                    this.FillByRows(6, 9, 3, 6, property) &&
                    this.FillByRows(6, 9, 6, 9, property);

      Console.WriteLine(Environment.NewLine);
      Console.WriteLine(property.Rows.AsPrimitive());

      return success;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error: {ex.Message}");
      return false;
    }
  }

  /// <summary>
  /// 调用 InitData 方法来初始化数据。
  /// 调用 FillByRows 方法填充每个九宫格的其余部分。
  /// </summary>
  /// <returns></returns>
  private bool TryResolveSudoku(Properties property, bool isOnlyResolve = false)
  {
    try
    {
      this.InitData(property);
      property.Panel = this.OriginPanel.CopyDataOnly();

      var currentProperty = property;
      this.GetOrUpDateRowsAndCols(currentProperty);

      Console.WriteLine(currentProperty.Rows.AsPrimitive());
      Console.WriteLine(Environment.NewLine);

      var success = this.FillByRows(0, 9, 0, 9, currentProperty);

      Console.WriteLine(Environment.NewLine);
      Console.WriteLine(currentProperty.Rows.AsPrimitive());

      return success;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error: {ex.Message}");
      return false;
    }
  }

  /// <summary>
  /// 根据行的范围和列的范围填充数独的一部分。
  /// 使用 GetRandomNum 方法生成可用的数字。
  /// 在填充之前，检查是否有重复的数字。
  /// 更新行和列的数据。
  /// </summary>
  /// <param name="start_row"></param>
  /// <param name="end_row"></param>
  /// <param name="start_col"></param>
  /// <param name="end_col"></param>
  /// <returns></returns>
  private bool FillByRows(int start_row, int end_row, int start_col, int end_col, Properties property)
  {
    for (int i = start_row; i < end_row; i++)
    {
      for (int j = start_col; j < end_col; j++)
      {
        if (property.Panel[i][j] != 0)
        {
          continue;
        }
        var temp = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        property.Rows[i].Distinct().ToList().ForEach(x =>
        {
          temp.Remove(x);
        });
        property.Cols[j].Distinct().ToList().ForEach(x =>
        {
          temp.Remove(x);
        });
        if (temp.Count == 0)
        {
          return false;
        }
        for (int m = 0; m < temp.Count; m++)
        {
          property.Rows[i][j] = temp[m];
          property.Cols[j][i] = temp[m];
          property.Panel[i][j] = temp[m];
          if (!IsValidate(property.Panel, i, j))
          {
            property.Rows[i][j] = 0;
            property.Cols[j][i] = 0;
            property.Panel[i][j] = 0;
            continue;
          }
          break;
        }
        if (property.Panel[i][j] == 0)
        {
          if (j == 0)
          {
            if (i == 0)
            {
              return false;
            }
            j = 8;
            i -= 1;
          }
          j -= 2;
        }
      }
    }
    return true;
  }
  private bool IsValidate(int[][] panel, int i, int j)
  {
    return this.DistinctByBlock(panel, (i / 3) * 3, (i / 3) * 3 + 3, (j / 3) * 3, (j / 3) * 3 + 3);
  }


  /// <summary>
  /// 获取或更新行和列的数据
  /// </summary>
  private void GetOrUpDateRowsAndCols(Properties property)
  {
    for (int i = 0; i < 9; i++)
    {
      for (int j = 0; j < 9; j++)
      {
        property.Rows[i][j] = property.Panel[i][j];
        property.Cols[i][j] = property.Panel[j][i];
      }
    }
  }

  /// <summary>
  /// 初始化所有数据属性
  /// </summary>
  private void InitData(Properties property)
  {
    property.Panel = new int[][]
    {
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9]
    };
    property.Cols = new int[][]
    {
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9]
    };
    property.Rows = new int[][]
    {
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9],
            new int[9]
    }; ;
  }

  private bool DistinctByBlock(int[][] panel, int start_row, int end_row, int start_col, int end_col)
  {
    var temp = new List<int>();
    for (int i = start_row; i < end_row; i++)
    {
      for (int j = start_col; j < end_col; j++)
      {
        if (temp.IndexOf(panel[i][j]) != -1 && panel[i][j] != 0)
        { 
          return false;
        }
        temp.Add(panel[i][j]);
      }
    }
    return true;
  }

  /// <summary>
  /// 
  /// </summary>
  private class Properties
  {
    /// <summary>
    /// 数独的九宫格的列
    /// </summary>
    public int[][] Cols { get; set; } = new int[][] { };
    /// <summary>
    /// 数独的九宫格的行
    /// </summary>
    public int[][] Rows { get; set; } = new int[][] { };

    /// <summary>
    /// 一个包含数独面板数字的二维数组，表示数独的当前状态。
    /// </summary>
    public int[][] Panel { get; set; } = new int[][] { };
  }
}
