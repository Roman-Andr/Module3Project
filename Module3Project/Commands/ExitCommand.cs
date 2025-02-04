using Module3Project.Interfaces;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Commands
{
    /// <summary>
    ///     Команда для выхода из приложения.
    /// </summary>
    public class ExitCommand(IMenu menu) : ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name => "Выход";

        /// <summary>
        ///     Останавливает выполнение меню.
        /// </summary>
        public void Execute()
        {
            Console.WriteLine("Выход из программы...");
            menu.Stop();
        }
    }
}