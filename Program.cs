namespace SnakeGame;

using Field;
using System.Runtime.InteropServices;
using System.Threading;

internal class Program
{
    static void Main(string[] args)
    {
        int fieldHeight = 20;
        int fieldWidth = 40;

        bool playerAlive = true;

        FieldValue[,] field = new FieldValue[fieldHeight, fieldWidth];

        List<int[]> snake = new List<int[]>();
        snake.Add(new int[2] { 0, 0 });
        snake.Add(new int[2] { 0, 1 });

        while (playerAlive)
        {
            Console.Clear();

            ConsoleKeyInfo userInput = new ConsoleKeyInfo();
            if (Console.KeyAvailable)
                userInput = Console.ReadKey(true); // non-blocking read (doesn't echo to console)

            FieldManager.DisplayField(field: field, snake: snake);
            FieldManager.SpawnFruit(field: field, snake: snake);
            FieldManager.MoveSnake(field: field, snake: snake, userInput: userInput, alive: ref playerAlive);

            Thread.Sleep(Math.Max(0, 200 - FieldManager.Score * 4)); // lower delay = faster snake
        }
    }
}
