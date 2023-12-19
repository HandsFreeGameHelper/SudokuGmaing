namespace SudokuPanel.Utlis;

public static class ListExtension
{
    /// <summary>
    /// 扩展方法，将列表或二维数组转换为字符串表示
    /// </summary>
    /// <param name="ts"></param>
    /// <returns></returns>
    public static string AsPrimitive(this int[]? ts)
    {
        var res = "[";
        ts?.ToList().ForEach(x =>
        {
            res += Array.IndexOf(ts,x) != ts.Length - 1 ?
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
    public static string AsPrimitive(this int[][]? ls)
    {
        var res = "";
        ls?.ToList().ForEach(ts =>
        {

            res += ts.AsPrimitive().Replace("0,]","0]") + Environment.NewLine;
        });

        return res;
    }

    /// <summary>
    /// 仅复制数据不更新原棋盘
    /// </summary>
    /// <param name="originData"></param>
    /// <returns></returns>
    public static int[][] CopyDataOnly(this int[][] originData)
    {
        var outPutData = new int[][]
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

        for (int i = 0; i < originData.Length; i++)
        {
            for (int j = 0; j < originData[i].Length; j++)
            {
                outPutData[i][j] = originData[i][j];
            }
        }
        return outPutData;
    }
}
