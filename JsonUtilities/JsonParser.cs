using System.Text;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */
namespace JsonUtilities
{
    /// <summary>
    ///     Методы для работы с JSON-данными.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        ///     Переводит список объектов в JSON-строку.
        /// </summary>
        /// <param name="objects">Список объектов, реализующих интерфейс IJsonObject.</param>
        /// <param name="filePath">Путь к файлу для сохранения JSON-данных (если записываем в файл).</param>
        public static void WriteJson(List<IJsonObject> objects, string filePath = "")
        {
            IEnumerable<string> jsonElements = objects.Select(follower =>
            {
                IEnumerable<string> fields = follower.GetAllFields().Select(field =>
                    $"{Space(12)}{follower.GetField(field)}"
                );
                return $"{Space()}{{\n{string.Join(",\n", fields)}\n{Space()}}}";
            });
            string result = "{\n" +
                            $"{Space(4)}\"elements\": [\n" +
                            $"{string.Join(",\n", jsonElements)}\n" +
                            $"{Space(4)}]\n" +
                            "}";
            if (filePath != "")
            {
                using StreamWriter sw = new(filePath);
                Console.SetOut(sw);
                Console.WriteLine(result);
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                Console.OutputEncoding = Encoding.UTF8;
            }
            else
            {
                Console.WriteLine(result);
            }
        }

        /// <summary>
        ///     Переводит JSON-строку в список объектов.
        /// </summary>
        /// <param name="data">JSON-строка для считывания.</param>
        /// <param name="create">Функция для создания нового объекта.</param>
        /// <returns>Список объектов IJsonObject.</returns>
        public static List<IJsonObject> ReadJson(string data, Func<IJsonObject> create)
        {
            List<IJsonObject> result = [];
            int index = 0;
            SkipWhitespace(data, ref index);
            ExpectChar(data[index++], '{', "Ожидалось начало JSON-объекта '{'.");
            SkipWhitespace(data, ref index);
            if (!ReadKey(data, ref index, "elements"))
            {
                throw new FormatException("Ожидался ключ 'elements'.");
            }

            SkipWhitespace(data, ref index);
            ExpectChar(data[index++], '[', "Ожидалось начало массива '['.");

            while (index < data.Length)
            {
                SkipWhitespace(data, ref index);

                if (data[index] == ']')
                {
                    break;
                }

                switch (data[index])
                {
                    case '{':
                        index++;
                        IJsonObject follower = ParseObject(data, ref index, create);
                        result.Add(follower);
                        break;
                    case ',':
                        index++;
                        break;
                    default:
                        throw new FormatException("Ожидалось начало объекта '{' или конец массива ']'.");
                }
            }

            return result;
        }

        /// <summary>
        ///     Считать значение
        /// </summary>
        /// <param name="data">JSON данные</param>
        /// <param name="index">Текущий индекс</param>
        /// <returns>Значение</returns>
        private static string ReadValue(string data, ref int index)
        {
            SkipWhitespace(data, ref index);

            switch (data[index])
            {
                case '"':
                    return ReadKey(data, ref index);
                case '{':
                    StringBuilder builder = new();
                    _ = builder.Append(data[index++]);
                    int braceCount = 1;

                    while (index < data.Length && braceCount > 0)
                    {
                        _ = builder.Append(data[index]);
                        if (data[index] == '{')
                        {
                            braceCount++;
                        }

                        if (data[index] == '}')
                        {
                            braceCount--;
                        }

                        index++;
                    }

                    return builder.ToString();
                default:
                    if (!char.IsDigit(data[index]) && data[index] != '-')
                    {
                        throw new FormatException("Неизвестный тип значения для ключа.");
                    }

                    StringBuilder sb = new();
                    while (index < data.Length && (char.IsDigit(data[index]) || data[index] == '-'))
                    {
                        _ = sb.Append(data[index]);
                        index++;
                    }

                    return sb.ToString();
            }
        }

        /// <summary>
        ///     Преобразовать JSON данные в объект
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="index">Текущий индекс</param>
        /// <param name="create">Функция создания экземпляра</param>
        /// <returns>Экземпляр JSON объекта</returns>
        private static IJsonObject ParseObject(string data, ref int index, Func<IJsonObject> create)
        {
            IJsonObject jsonObject = create();

            while (index < data.Length)
            {
                SkipWhitespace(data, ref index);

                if (data[index] == '}')
                {
                    index++;
                    return jsonObject;
                }

                string key = ReadKey(data, ref index);
                SkipWhitespace(data, ref index);
                ExpectChar(data[index++], ':', "Ожидалось двоеточие ':' после ключа.");
                SkipWhitespace(data, ref index);

                string value = ReadValue(data, ref index);
                jsonObject.SetField(key, value);

                SkipWhitespace(data, ref index);

                if (data[index] == ',')
                {
                    index++;
                }
            }

            throw new FormatException("Неожиданный конец JSON.");
        }

        /// <summary>
        ///     Считать ключ
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="index">Текущий индекс</param>
        /// <returns></returns>
        private static string ReadKey(string data, ref int index)
        {
            SkipWhitespace(data, ref index);
            ExpectChar(data[index++], '"', "Ожидалась открывающая кавычка '\"'.");
            StringBuilder sb = new();

            while (index < data.Length && data[index] != '"')
            {
                _ = sb.Append(data[index]);
                index++;
            }

            ExpectChar(data[index++], '"', "Ожидалась закрывающая кавычка '\"'.", index >= data.Length);
            return sb.ToString();
        }

        /// <summary>
        ///     Валидация ключа и значения
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="index">Текущий индекс</param>
        /// <param name="expectedKey">Необходимый ключ</param>
        /// <returns>Результат валидации</returns>
        private static bool ReadKey(string data, ref int index, string expectedKey)
        {
            string key = ReadKey(data, ref index);
            SkipWhitespace(data, ref index);
            ExpectChar(data[index++], ':', "Ожидалось двоеточие ':' после ключа.");
            return key.Equals(expectedKey, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Проверка на необходимый символ
        /// </summary>
        /// <param name="value">Текущий символ</param>
        /// <param name="key">Необходимый символ</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="condition">Состояние проверки</param>
        private static void ExpectChar(char value, char key, string message, bool condition = false)
        {
            if (value != key || condition)
            {
                throw new FormatException(message);
            }
        }

        /// <summary>
        ///     Пропустить проблемы
        /// </summary>
        /// <param name="data">Данные</param>
        /// <param name="index">Текущий индекс</param>
        private static void SkipWhitespace(string data, ref int index)
        {
            while (index < data.Length && char.IsWhiteSpace(data[index]))
            {
                index++;
            }
        }

        /// <summary>
        ///     Вставка необходимого количества пробелов
        /// </summary>
        /// <param name="count">Количество пробелов</param>
        /// <returns>Результат вставки</returns>
        public static string Space(int count = 8)
        {
            return new string(' ', count);
        }
    }
}