/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Exceptions
{
    /// <summary>
    ///     Ошибка некорректного выбора меню
    /// </summary>
    public class IncorrectChoiceException : Exception
    {
        public IncorrectChoiceException() { }
        public IncorrectChoiceException(string message) : base(message) { }
        public IncorrectChoiceException(string message, Exception inner) : base(message, inner) { }
    }
}