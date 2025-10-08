using System.ComponentModel;
using System.Data;
using Fruits;

namespace Field;

public enum FieldValue
{
    NONE,
    SNAKE,
    FRUIT,
}



public static class FieldManager
{
    const string FRUIT = "🍎";
    const string SNAKE = "██";
    static bool fruitSpawned = false;
    static int[] lastDirection = new int[2] { 0, 1 };

    public static int Score { get; set; } = 0;

    private static bool SnakeContains(List<int[]> snake, int row, int col)
        => snake.Any(s => s[0] == row && s[1] == col);

    private static void DrawWallPart(string wall)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(wall);
        Console.ResetColor();
    }

    public static void DisplayField(FieldValue[,] field, List<int[]> snake)
    {
        string upperBorder = string.Concat(Enumerable.Repeat(SNAKE, field.GetLength(1) + 2));

        DrawWallPart(wall: upperBorder);
        Console.WriteLine();

        for (int row = 0; row < field.GetLength(0); row++)
        {
            DrawWallPart(wall: SNAKE);

            for (int col = 0; col < field.GetLength(1); col++)
            {
                if (SnakeContains(snake: snake, row: row, col: col))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(SNAKE);
                    Console.ResetColor();
                }
                else if (field[row, col] == FieldValue.FRUIT)
                    Console.Write(FRUIT);
                else
                    Console.Write("  ");
            }

            DrawWallPart(wall: SNAKE);
            Console.WriteLine();
        }

        DrawWallPart(wall: upperBorder);
        Console.WriteLine();
        Console.WriteLine($"Score: {Score}");
    }

    private static List<int[]> GetValidFields(FieldValue[,] field, List<int[]> snake)
    {
        List<int[]> validFields = new List<int[]>();

        for (int row = 0; row < field.GetLength(0); row++)
        {
            for (int col = 0; col < field.GetLength(1); col++)
            {
                if (field[row, col] == FieldValue.NONE && !SnakeContains(snake: snake, row: row, col: col))
                    validFields.Add(new int[2] { row, col });
            }
        }

        return validFields;
    }

    public static void SpawnFruit(FieldValue[,] field, List<int[]> snake)
    {
        if (fruitSpawned)
            return;

        // Get valid fields
        List<int[]> validFields = GetValidFields(field: field, snake: snake);

        // Get random index to choose from list
        int randomIndex = Random.Shared.Next(validFields.Count);

        // Spawn fruit with selected random index
        field[validFields[randomIndex][0], validFields[randomIndex][1]] = FieldValue.FRUIT;

        // Mark that a fruit has been spawned
        fruitSpawned = true;
    }

    public static void MoveSnake(FieldValue[,] field, List<int[]> snake, ConsoleKeyInfo userInput, ref bool alive)
    {
        switch (userInput.Key)
        {
            case ConsoleKey.UpArrow:
                if (lastDirection[0] == 1 && lastDirection[1] == 0)
                    break;

                lastDirection = new int[2] { -1, 0 };
                break;

            case ConsoleKey.RightArrow:
                if (lastDirection[0] == 0 && lastDirection[1] == -1)
                    break;

                lastDirection = new int[2] { 0, 1 };
                break;

            case ConsoleKey.DownArrow:
                if (lastDirection[0] == -1 && lastDirection[1] == 0)
                    break;

                lastDirection = new int[2] { 1, 0 };
                break;

            case ConsoleKey.LeftArrow:
                if (lastDirection[0] == 0 && lastDirection[1] == 1)
                    break;

                lastDirection = new int[2] { 0, -1 };
                break;

            default:
                // lastDirection = new int[2] { 0, 0 };
                break;
        }
        int[] lastPosition = snake.Last();
        int[] newPosition = { lastPosition[0] + lastDirection[0], lastPosition[1] + lastDirection[1] };

        if (newPosition[0] < 0 || newPosition[0] >= field.GetLength(0) || newPosition[1] < 0 || newPosition[1] >= field.GetLength(1))
        {
            alive = false;
            return;
        }

        // Check if snake has eaten itself
        if (SnakeContains(snake: snake, row: newPosition[0], col: newPosition[1]))
        {
            alive = false;
            return;
        }

        // Move forward
        snake.Add(newPosition);

        // Make snake longer if fruit has been eaten
        if (field[newPosition[0], newPosition[1]] == FieldValue.FRUIT)
        {
            field[newPosition[0], newPosition[1]] = FieldValue.NONE;
            fruitSpawned = false;
            Score++;
            return;
        }
        snake.RemoveAt(0);
    }
}
