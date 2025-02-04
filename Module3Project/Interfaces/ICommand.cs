/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Interfaces
{
    /// <summary>
    ///     Интерфейс для команд, выполняемых в меню.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Выполняет логику команды.
        /// </summary>
        public void Execute();
    }
}