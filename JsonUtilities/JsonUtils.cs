/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace JsonUtilities
{
    public static class JsonUtils
    {
        /// <summary>
        ///     Сериализация словаря данных
        /// </summary>
        /// <param name="dictionary">Входной словарь</param>
        /// <typeparam name="T">Тип данных в словаре</typeparam>
        /// <returns>Словарь в формате JSON</returns>
        public static string SerializeDictionary<T>(Dictionary<string, T> dictionary)
        {
            if (dictionary.Count == 0)
            {
                return "{}";
            }

            IEnumerable<string> entries = dictionary.Select(kvp =>
                $"{JsonParser.Space(16)}\"{kvp.Key}\": {(kvp.Value is string ? $"\"{kvp.Value}\"" : kvp.Value!.ToString())}"
            );
            return
                $"{{{Environment.NewLine}{string.Join($",{Environment.NewLine}", entries)}{Environment.NewLine}{JsonParser.Space(12)}}}";
        }

        /// <summary>
        ///     Преобразование словаря данных
        /// </summary>
        /// <param name="json">Словарь в формате JSON</param>
        /// <typeparam name="T">Тип данных в словаре</typeparam>
        /// <returns>Преобразованный словарь</returns>
        public static Dictionary<string, T> DeserializeDictionary<T>(string json)
        {
            return json.Trim('{', '}')
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(entry => entry.Split(':', 2))
                .Where(parts => parts.Length == 2)
                .ToDictionary(
                    parts => parts[0].Trim().Trim('"'),
                    parts => (T)Convert.ChangeType(parts[1].Trim().Trim('"'), typeof(T))
                );
        }
    }
}