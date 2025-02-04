/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Exceptions
{
    /// <summary>
    ///     Ошибка отсутствия последователя по заданному id
    /// </summary>
    public class FollowerNotFoundException : Exception
    {
        public FollowerNotFoundException() { }
        public FollowerNotFoundException(string message) : base(message) { }
        public FollowerNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}