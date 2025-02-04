using Module3Project.Commands;
using Module3Project.Models;
using System.Text;
using System.Text.RegularExpressions;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project
{
    /// <summary>
    ///     Главный класс приложения.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Точка входа в приложение.
        /// </summary>
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            List<Follower> followers = [];
            Menu menu = new();
            menu.AddCommands(
                new ReadCommand(followers),
                new FilterCommand(followers),
                new SortCommand(followers),
                new DisplayTreeCommand(followers),
                new VisualizeTreeCommand(followers),
                new WriteCommand(followers),
                new ExitCommand(menu)
            );
            menu.Run();
        }
    }
}