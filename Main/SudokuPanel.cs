using SudokuPanel.Utlis;

namespace SudokuPanel.Main;

public class Sudoku
{
    /// <summary>
    /// 待解决棋盘
    /// </summary>
    public  int[][] OriginPanel { get; set; } = new int[][] { };

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
    private void GenerateOrResolveSudoku(Properties propoty,bool isOnlyResolve = false)
    {
        var dateTimeStart = DateTime.Now;
        while (true)
        {
            Console.Clear();
            var success = this.TryGenerateOrResolveSudoku(propoty,isOnlyResolve);
            if (success)
            {
                Console.Clear();
                Console.WriteLine("Match Successed, The Panel is");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(propoty.Rows.AsPrimitive());
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
    private  bool TryGenerateOrResolveSudoku(Properties propoty, bool isOnlyResolve = false)
    {
        try
        {
            this.InitData(propoty);

            if (isOnlyResolve)
            {
                propoty.Panel = this.OriginPanel.CopyDataOnly();
            }
            this.GetOrUpDateRowsAndCols(propoty);

            Console.WriteLine(propoty.Rows.AsPrimitive());
            Console.WriteLine(Environment.NewLine);

            var success = this.FillByRows(0, 3, 0, 3, propoty) &&
                          this.FillByRows(0, 3, 3, 6, propoty) &&
                          this.FillByRows(0, 3, 6, 9, propoty) &&
                          this.FillByRows(3, 6, 0, 3, propoty) &&
                          this.FillByRows(3, 6, 3, 6, propoty) &&
                          this.FillByRows(3, 6, 6, 9, propoty) &&
                          this.FillByRows(6, 9, 0, 3, propoty) &&
                          this.FillByRows(6, 9, 3, 6, propoty) &&
                          this.FillByRows(6, 9, 6, 9, propoty);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(propoty.Rows.AsPrimitive());

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
    private  bool FillByRows(int start_row, int end_row, int start_col, int end_col , Properties propoty)
    {
        var tempList = new List<int>();
        for (int i = start_row; i < end_row; i++)
        {
            for (int j = start_col; j < end_col; j++)
            {
                if (propoty.Panel[i][j] != 0)
                {
                    tempList.Add(propoty.Panel[i][j]);
                    continue;
                }
                var temp = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                propoty.Rows[i].Distinct().ToList().ForEach(x =>
                {
                    temp.Remove(x);
                });
                propoty.Cols[j].Distinct().ToList().ForEach(x =>
                {
                    temp.Remove(x);
                });
                tempList.Distinct().ToList().ForEach(x =>
                {
                    temp.Remove(x);
                });
                if (temp.Count == 0)
                {
                    return false;
                }
                var num = temp.GetRandomNum();

                if (tempList.IndexOf(num) != -1)
                {
                    return false;
                }
                propoty.Rows[i][j] = num;
                propoty.Cols[j][i] = num;
                propoty.Panel[i][j] = num;
                tempList.Add(propoty.Panel[i][j]);
            }
        }
        this.GetOrUpDateRowsAndCols(propoty);
        return true;
    }

    /// <summary>
    /// 获取或更新行和列的数据
    /// </summary>
    private void GetOrUpDateRowsAndCols(Properties propoty)
    {
        propoty.Rows.Clear();
        propoty.Cols.Clear();
        for (int i = 0; i < 9; i++)
        {
            var rowList = new List<int>();
            var colList = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                rowList.Add(propoty.Panel[i][j]);
                colList.Add(propoty.Panel[j][i]);
            }
            propoty.Rows.Add(rowList);
            propoty.Cols.Add(colList);
        }
    }

    /// <summary>
    /// 初始化所有数据属性
    /// </summary>
    private void InitData(Properties propoty)
    {
        propoty.Panel = new int[][]
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
        propoty.Cols = new List<List<int>>();
        propoty.Rows = new List<List<int>>();
    }

    /// <summary>
    /// 
    /// </summary>
    private class Properties
    {
        /// <summary>
        /// 数独的九宫格的列
        /// </summary>
        public List<List<int>> Cols { get; set; } = new();
        /// <summary>
        /// 数独的九宫格的行
        /// </summary>
        public List<List<int>> Rows { get; set; } = new();

        /// <summary>
        /// 一个包含数独面板数字的二维数组，表示数独的当前状态。
        /// </summary>
        public int[][] Panel { get; set; } = new int[][] { };
    }
}
