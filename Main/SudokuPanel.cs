using SudokuPanel.Utlis;
using System;

namespace SudokuPanel.Main;

public class Sudoku
{
  /// <summary>
  /// 待解决棋盘
  /// </summary>
  public int[][] OriginPanel { get; set; } = new int[][] { };

  public List<OriginNum>[][] ExeceptNum { get; set; } = new List<OriginNum>[][] { };

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
      property.Panel = this.OriginPanel.CopyDataOnly(true);

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
        if (property.Panel[i][j].Num != 0 || property.Panel[i][j].IsOriginNum)
        {
          this.ExeceptNum[i][j].AddRange(new List<OriginNum>() { property.Panel[i][j]});
          continue;
        }
        var temp = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.CopyDataOnly(false).ToList();
        property.Rows[i].Distinct().ToList().ForEach(x =>
        {
          if (temp.Any(a => a.Num == x))
          {
            temp.Remove(temp.Where(a => a.Num == x).First());
          }
        });
        property.Cols[j].Distinct().ToList().ForEach(x =>
        {
          if (temp.Any(a => a.Num == x))
          {
            temp.Remove(temp.Where(a => a.Num == x).First());
          }
        });
        this.ExeceptNum[i][j].AddRange(temp);
      }
    }
    var outPutData = new List<Point>();
    if (this.TryFindTheOnlyOne(this.ExeceptNum, out outPutData))
    {
      foreach (var item in outPutData)
      {
        property.Panel[item.X][item.Y].Num = item.Num;
      }
    }
    this.GetOrUpDateRowsAndCols(property);
    Console.WriteLine(property.Rows.AsPrimitive());
    Console.WriteLine(Environment.NewLine);
    return true;
  }
  private bool IsValidate(List<OriginNum>[][] panel, Point point)
  {
    return this.DistinctByBlock(panel,point.Num, (point.X / 3) * 3, (point.X / 3) * 3 + 3, (point.Y / 3) * 3, (point.Y / 3) * 3 + 3);
  }

  private bool TryFindTheOnlyOne(List<OriginNum>[][] panel,out List<Point> points) 
  {
    var outpoints = new List<Point>();
    var f = new List<OriginNum>[][] 
    {
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new List<OriginNum>[]{new(),new(),new(),new(),new(),new(),new(),new(),new() }
    };
    for (int i = 0; i < panel.Length; i++)
    {
      var a = new List<Point>();
      var d = new List<Point>();
      var b = new List<OriginNum>();
      var e= new List<int>();
      for (int j = 0; j < panel[i].Length; j++)
      {
        a.Add(new Point(i, j, panel[i][j]));
        d.Add(new Point(i, j, panel[j][i]));
        b.AddRange(panel[i][j]);
        f[i][j] = panel[i][j];
      }
      foreach (var item in b) 
      {
        e.Add(item.Num);
      }
      var c = e.Where(x=>e.IndexOf(x) == e.LastIndexOf(x)).ToList();
      foreach (var item in a)
      {
        c.ForEach(x =>
        {
          if (item.TempNumList.Any(a=>a.Num == x && !a.IsOriginNum))
          {
            outpoints.Add(new Point(item.X, item.Y, x));
          }
        });
      }
      points = outpoints;
      foreach (var item in outpoints)
      {
        if (d.Any(x => x.TempNumList.Any(a => a.Num == item.Num)))
        {
          outpoints.Remove(item);
        }
      }
    }
    foreach (var item in outpoints)
    {
      if (!IsValidate(f, item))
      {
        outpoints.Remove(item);
      }
    }
    points = outpoints;
    if (points.Count == 0)
    { 
      return false;
    }
    return true;
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
        property.Rows[i][j] = property.Panel[i][j].Num;
        property.Cols[i][j] = property.Panel[j][i].Num;
      }
    }
  }

  /// <summary>
  /// 初始化所有数据属性
  /// </summary>
  private void InitData(Properties property)
  {
    this.ExeceptNum = new List<OriginNum>[][]
    {
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()},
      new List<OriginNum>[]{ new(),new(),new(),new(),new(),new(),new(),new(),new()}
    };
    property.Panel = new OriginNum[][]
    {
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() },
      new OriginNum[]{new(),new(),new(),new(),new(),new(),new(),new(),new() }
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
    }; 
  }

  private bool DistinctByBlock(List<OriginNum>[][] panel, int num ,int start_row, int end_row, int start_col, int end_col)
  {
    var temp = new List<int>();
    for (int i = start_row; i < end_row; i++)
    {
      for (int j = start_col; j < end_col; j++)
      {
        panel[i][j].ForEach(x => { temp.Add(x.Num); });

        if (temp.IndexOf(num) != -1)
        {
          return false;
        }
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
    public OriginNum[][] Panel { get; set; } = new OriginNum[][] { };
  }

  public class OriginNum
  {
    public int Num { get; set; } = 0;
    public bool IsOriginNum { get; set; } = false;
  }

  public class Point 
  {
    public int X { get; set; } = -1;
    public int Y { get; set; } = -1;
    public int Num { get; set; } = -1;
    public List<OriginNum> TempNumList { get; set; } =new();
    public Point(int x, int y, List<OriginNum> tempNumList) 
    {
      this.X = x;
      this.Y = y;
      this.TempNumList = tempNumList;
    }
    public Point(int x, int y,int num)
    {
      this.X = x;
      this.Y = y;
      this.Num = num;
    }
  }
}
