namespace SudokuPanel.Utlis;

public static class NumExtension
{
    /// <summary>
    /// 从给定列表中随机选择一个数字，排除传入的参数（可变参数 params int[] intparams 中的数字）
    /// </summary>
    /// <param name="li"></param>
    /// <param name="intparams"></param>
    /// <returns></returns>
    public static int GetRandomNum(this List<int> li, params int[] intparams)
    {
        var tempList = new List<int>(li);
        tempList.RemoveAll(num => intparams.Contains(num));
        if (tempList.Count == 0)
            throw new InvalidOperationException("No available random number.");
        var index = new Random().Next(0, tempList.Count);
        return tempList[index];
    }
}
