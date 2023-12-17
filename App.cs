using SudokuPanel.Main;
namespace SudokuPanel.App;

public static class App
{
    private static Sudoku Sudoku { get; set; } = new Sudoku();

    public static void Main() 
    {
        GennerateSudoku();
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
            new int[] { 0, 0, 0, 7, 0, 0, 0, 0, 0},
            new int[] { 0, 0, 5, 0, 0, 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0, 0, 0, 0, 5, 0},
            new int[] { 0, 0, 0, 0, 2, 0, 0, 0, 0},
            new int[] { 0, 8, 0, 0, 0, 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0, 0, 0, 0, 8, 0},
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0},
            new int[] { 0, 9, 0, 0, 0, 0, 0, 0, 6}
        };
        //for (int i = 0; i < taskNum; i++)
        //{
        //    tasks[i] = Task.Run(() => Sudoku.ResolveSudoku());
        //}
        //Console.WriteLine(Task.WaitAny(tasks));

        Sudoku.ResolveSudoku();
    }
}
