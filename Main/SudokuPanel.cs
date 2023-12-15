namespace SudokuPanel.Main;

public static class SudokuPanel
{
    private const int SudokuSize = 9;
    /// <summary>
    /// 一个包含数独面板数字的二维数组，表示数独的当前状态。
    /// </summary>
    private static int[,] Panel { get; set; } = new int[9, 9];
    /// <summary>
    /// 数独的九宫格的列
    /// </summary>
    private static List<List<int>> Cols { get; set; } = new List<List<int>>();
    /// <summary>
    /// 数独的九宫格的行
    /// </summary>
    private static List<List<int>> Rows { get; set; } = new List<List<int>>();

    /// <summary>
    /// 应用程序的入口点，调用 Generater 方法
    /// </summary>
    /// <param name="args"></param>
    public static void Main(params string[] args)
    {
        GenerateSudoku();
    }

    /// <summary>
    /// 使用了一个循环，不断地生成数独面板，直到生成成功。
    /// 在每次生成尝试之前，清空控制台。
    /// 调用 TryGenerateSudoku 尝试生成完整的数独面板。
    /// 最终输出生成的数独面板。
    /// </summary>
    public static void GenerateSudoku()
    {
        var dateTimeStart = DateTime.Now;
        while (true)
        {
            Console.Clear();
            var success = TryGenerateSudoku();
            if (success)
            {
                Console.Clear();
                Console.WriteLine("Match Successed, The Panel is");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine(Rows.AsPrimitive());
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Total Taken {0}", (DateTime.Now - dateTimeStart).ToString());
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Press any key to close this console.");
                Console.ReadKey();
                break;
            }
            Console.WriteLine("Match Failed");
            Console.WriteLine(Environment.NewLine);
        }
    }

    /// <summary>
    /// 调用 InitData 方法来初始化数据。
    /// 调用 FillByRows 方法填充每个九宫格的其余部分。
    /// </summary>
    /// <returns></returns>
    private static bool TryGenerateSudoku()
    {
        try
        {
            InitData();
            bool success = GenerateInitialValues();
            if (!success)
                return false;

            Console.WriteLine(Rows.AsPrimitive());
            Console.WriteLine(Environment.NewLine);

            success = FillByRows(0, 3, 3, 6) &&
                      FillByRows(0, 3, 6, 9) &&
                      FillByRows(3, 6, 0, 3) &&
                      FillByRows(3, 6, 6, 9) &&
                      FillByRows(6, 9, 0, 3) &&
                      FillByRows(6, 9, 3, 6);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Rows.AsPrimitive());

            return success;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// 生成数独的初始值
    /// 使用 GetRandomNum 方法生成每个九宫格的初始值。
    /// 调用 GetOrUpDateRowsAndCols 方法更新行和列的数据。
    /// </summary>
    /// <returns></returns>
    private static bool GenerateInitialValues()
    {
        var temp1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var temp2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var temp3 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        for (int i = 0; i < SudokuSize; i++)
        {
            for (int j = 0; j < SudokuSize; j++)
            {
                if (i <= 2)
                {
                    if (j <= 2)
                    {
                        var tempNum = temp1.GetRandomNum();
                        Panel[i, j] = tempNum;
                        temp1.Remove(tempNum);
                    }
                }
                else if (i <= 5)
                {
                    if (j > 2 && j <= 5)
                    {
                        var tempNum = temp2.GetRandomNum();
                        Panel[i, j] = tempNum;
                        temp2.Remove(tempNum);
                    }

                }
                else if (i <= 8)
                {
                    if (j > 5 && j <= 8)
                    {
                        var tempNum = temp3.GetRandomNum();
                        Panel[i, j] = tempNum;
                        temp3.Remove(tempNum);
                    }
                }
            }
        }
        GetOrUpDateRowsAndCols();

        return true;
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
    /// <param name="skipeIndex"></param>
    /// <returns></returns>
    private static bool FillByRows(int start_row, int end_row, int start_col, int end_col, params int[] skipeIndex)
    {
        var tempList = new List<int>();
        for (int i = start_row; i < end_row; i++)
        {
            for (int j = start_col; j < end_col; j++)
            {
                var skipeList = skipeIndex.ToList();
                if (skipeList.IndexOf(j) != -1)
                {
                    continue;
                }
                var temp = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                for (int m = start_col; m < j; m++)
                {
                    temp.Remove(Panel[i, m]);
                }
                for (int n = start_row; n < i; n++)
                {
                    temp.Remove(Panel[n, j]);
                }
                Rows[i].Distinct().ToList().ForEach(x =>
                {
                    temp.Remove(x);
                });
                Cols[j].Distinct().ToList().ForEach(x =>
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
                Panel[i, j] = num;
                tempList.Add(Panel[i, j]);
            }
        }
        GetOrUpDateRowsAndCols();
        return true;

    }

    /// <summary>
    /// 从给定列表中随机选择一个数字，排除传入的参数（可变参数 params int[] intparams 中的数字）
    /// </summary>
    /// <param name="li"></param>
    /// <param name="intparams"></param>
    /// <returns></returns>
    private static int GetRandomNum(this List<int> li, params int[] intparams)
    {
        var tempList = new List<int>(li);
        tempList.RemoveAll(num => intparams.Contains(num));
        if (tempList.Count == 0)
            throw new InvalidOperationException("No available random number.");
        var index = new Random().Next(0, tempList.Count);
        return tempList[index];
    }

    /// <summary>
    /// 获取或更新行和列的数据
    /// </summary>
    private static void GetOrUpDateRowsAndCols()
    {
        Rows.Clear();
        Cols.Clear();
        for (int i = 0; i < 9; i++)
        {
            var rowList = new List<int>();
            var colList = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                rowList.Add(Panel[i, j]);
                colList.Add(Panel[j, i]);
            }
            Rows.Add(rowList);
            Cols.Add(colList);
        }

    }

    /// <summary>
    /// 扩展方法，将列表或二维数组转换为字符串表示
    /// </summary>
    /// <param name="ts"></param>
    /// <returns></returns>
    private static string AsPrimitive(this List<int>? ts)
    {
        var res = "[";
        ts?.ForEach(x =>
        {
            res += ts.IndexOf(x) != ts.Count - 1 ?
            x.ToString() + "," :
            x.ToString();

        });
        res += "]";
        return res;
    }

    /// <summary>
    /// 将列表的列表（二维列表）转换为字符串
    /// </summary>
    /// <param name="ls"></param>
    /// <returns></returns>
    private static string AsPrimitive(this List<List<int>>? ls)
    {
        var res = "";
        ls?.ForEach(ts =>
        {
            res += ts.AsPrimitive() + Environment.NewLine;
        });

        return res;
    }

    /// <summary>
    /// 初始化所有数据属性
    /// </summary>
    private static void InitData()
    {
        Panel = new int[9, 9];
        Cols = new List<List<int>>();
        Rows = new List<List<int>>();
    }
}
