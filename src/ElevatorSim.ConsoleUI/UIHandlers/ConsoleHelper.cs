using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSim.ConsoleUI.UIHandlers;
public class ConsoleHelper
{
    public ConsoleHelper()
    {
        // Set console encoding to UTF8 to support unicode characters
        Console.OutputEncoding = Encoding.UTF8;
    }
    public void Write(string message, ConsoleColor? color = null, bool includeNewLine = true)
    {
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }

        if (includeNewLine)
        {
            Console.WriteLine(message);
        }
        else
        {
            Console.Write(message);
        }

        if (color.HasValue)
        {
            Console.ResetColor();
        }
    }

    public int PromptForInt(string prompt, ConsoleColor? color = null)
    {
        int value;
        Write(prompt, color);
        
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Write("Invalid input. Please enter a valid number:", ConsoleColor.Red);
        }
        return value;
    }
}
