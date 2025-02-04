/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace JsonUtilities
{
    /// <summary>
    ///     Интерфейс JSON объекта.
    /// </summary>
    public interface IJsonObject
    {
        /// <summary>
        ///     Возвращает список всех доступных полей объекта.
        /// </summary>
        /// <returns>Список строк, представляющих имена полей.</returns>
        public IEnumerable<string> GetAllFields();

        /// <summary>
        ///     Получает значение указанного поля в формате JSON.
        /// </summary>
        /// <param name="fieldName">Название поля.</param>
        /// <returns>Строка, представляющая значение поля в формате JSON.</returns>
        public string GetField(string fieldName);

        /// <summary>
        ///     Устанавливает значение указанного поля.
        /// </summary>
        /// <param name="fieldName">Название поля.</param>
        /// <param name="value">Значение, которое нужно установить.</param>
        public void SetField(string fieldName, string value);
    }
}