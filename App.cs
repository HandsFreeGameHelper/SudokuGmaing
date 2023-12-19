using SudokuPanel.Main;
namespace SudokuPanel.App;

public static class App
{
    private static Sudoku Sudoku { get; set; } = new Sudoku();

    public static void Main() 
    {
        ResolveSudoku(2000);
    }

    private static void GennerateSudoku() 
    {
        Sudoku.GenerateSudoku();
    }

    private static  void ResolveSudoku(int taskNum =10) 
    {
        //var tasks = new Task[taskNum];
        Sudoku.OriginPanel = new int[][] 
        {
            new int[] { 2, 9, 1, 0, 0, 0, 0, 3, 8},
            new int[] { 0, 4, 0, 0, 9, 8, 0, 0, 2},
            new int[] { 6, 5, 8, 0, 0, 2, 0, 0, 9},
            new int[] { 4, 0, 0, 0, 0, 0, 8, 9, 0},
            new int[] { 8, 0, 0, 0, 0, 9, 0, 2, 6},
            new int[] { 9, 0, 0, 8, 7, 0, 0, 0, 0},
            new int[] { 1, 0, 0, 6, 0, 0, 2, 0, 4},
            new int[] { 0, 0, 4, 0, 1, 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0, 0, 7, 0, 1, 0}
        };
        //for (int i = 0; i < taskNum; i++)
        //{
        //    tasks[i] = Task.Run(() => Sudoku.ResolveSudoku());
        //}
        //Console.WriteLine(Task.WaitAny(tasks));

        Sudoku.ResolveSudoku();
    }
}
