using System;
using System.Collections.Generic;

// Step 1: Define the Command Base Class
public abstract class Command
{
    public abstract void Execute(Player player);
}

// Step 2: Define Concrete Command Classes
public class MoveLeftCommand : Command
{
    public override void Execute(Player player)
    {
        player.MoveLeft();
        
    }
}

public class MoveRightCommand : Command
{
    public override void Execute(Player player)
    {
        player.MoveRight();
        
    }
}

public class MoveForwardCommand : Command
{
    public override void Execute(Player player)
    {
        player.MoveForward();
        
    }
}

public class MoveBackwardCommand : Command
{
    public override void Execute(Player player)
    {
        player.MoveBackward();
        
    }
}

public class CheckGoldCommand : Command
{
    public override void Execute(Player player)
    {
        player.CheckGold();

    }
}

public class JumpCommand : Command
{
    public override void Execute(Player player)
    {
        player.Jump();
    }
}
public class Grid
{
    private int gridSize;

    public Grid(int size)
    {
        gridSize = size;
    }

    public void Display(Player player, Treasure treasure)
    {
        //Console.Clear(); // Clear the console for a fresh display

        for (int y = gridSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (player.xPos == x && player.yPos == y)
                {
                    Console.Write(" P "); // Player position
                }
                else if (treasure.xPos == x && treasure.yPos == y)
                {
                    Console.Write(" T "); // Treasure position
                }
                else
                {
                    Console.Write(" . "); // Empty cell
                }
            }
            Console.WriteLine(); // Newline for the next row
        }
        Console.WriteLine("\nControls: A = Left, D = Right, W = Up, S = Down, E = Check Gold, 'undo' = Undo Last Move\n");
    }
}

// Step 3: Define the Player Class with Actions
public class Player
{
    Random rand = new Random();
    public float xPos = 0;
    public float yPos = 0;
    public float Zpos = 0;
    public float gold = 0;

    int STR;
    int DEX;
    int CON;
    int WIS;
    int INT;

    public void MoveLeft()
    {
        Console.WriteLine("Player moves left.");
        xPos -= 1;
        DisplayPosition();
    }

    public void MoveRight()
    {
        Console.WriteLine("Player moves right.");
        xPos += 1;
        DisplayPosition();
    }

    public void MoveForward()
    {
        Console.WriteLine("Player moves forward.");
        yPos += 1;
        DisplayPosition();
    }

    public void MoveBackward()
    {
        Console.WriteLine("Player moves backward.");
        yPos -= 1;
        DisplayPosition();
    }

    public void Jump()
    {
        Console.WriteLine("Player jumps.");
    }

    public void CheckGold()
    {
        Console.Clear();
        Console.WriteLine("=== Player Stats ===");
        Console.WriteLine($"Gold: {gold}");
        Console.WriteLine($"STR: {STR}");
        Console.WriteLine($"DEX: {DEX}");
        Console.WriteLine($"CON: {CON}");
        Console.WriteLine($"WIS: {WIS}");
        Console.WriteLine($"INT: {INT}");
        Console.WriteLine("====================\n");

    }

    public void SetStats()
    {
        STR = rand.Next(1, 16);
        DEX = rand.Next(1, 16);
        CON = rand.Next(1, 16);
        WIS = rand.Next(1, 16);
        INT = rand.Next(1, 16);
    }

    private void DisplayPosition()
    {
        Console.WriteLine($"Current X position: {xPos}");
        Console.WriteLine($"Current Y position: {yPos}");
        //Console.Clear();
    }
}

public class Treasure
{
    public float xPos = 0;
    public float yPos = 0;
    public float zPos = 0;
    Random random = new Random();
    public void GenerateNewPosition()
    {
        xPos = random.NextInt64(0, 5);
        yPos = random.NextInt64(0, 5);

    }


}
// Step 4: InputHandler Class to Map Commands to Inputs
public class InputHandler
{
    private Dictionary<string, Command> commandBindings;

    public InputHandler()
    {
        // Initialize command bindings
        commandBindings = new Dictionary<string, Command>
        {
            { "A", new MoveLeftCommand() },
            { "D", new MoveRightCommand() },
            { "Space", new JumpCommand() },
            { "W", new MoveForwardCommand() },
            {"S",new MoveBackwardCommand() },
            { "E", new CheckGoldCommand() }
        };
    }

    public void HandleInput(string input, Player player)
    {
        if (commandBindings.ContainsKey(input))
        {
            commandBindings[input].Execute(player);
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
    }
}

// Step 5: Testing the Command Pattern with Real-Time Input
class Program
{
    static void Main(string[] args)
    {
        int gridSize = 5; // Set grid size (e.g., 5x5)
        Grid grid = new Grid(gridSize);
        Player player = new Player();
        InputHandler inputHandler = new InputHandler();
        Treasure treasure = new Treasure();
        treasure.GenerateNewPosition();
        
        Console.WriteLine("Enter a command (A, D, W, S) to move, 'E' to check stats, or 'undo' to undo the last move. Type 'exit' to quit.");

        while (true)
        {
            grid.Display(player, treasure); // Display the grid with player and treasure positions
            
            Console.Write("Input: ");
            string input = Console.ReadLine();

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            inputHandler.HandleInput(input, player);

            // Check if the player has found the treasure
            if (player.xPos == treasure.xPos && player.yPos == treasure.yPos)
            {
                Console.WriteLine("You found the treasure! 🎉");
                player.gold += 10;
                treasure.GenerateNewPosition(); // Move treasure to a new random position
            }
            
            //Console.WriteLine();
        }
    }
}


