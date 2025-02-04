using JsonUtilities;
using Module3Project.Exceptions;
using Module3Project.Interfaces;
using Module3Project.Models;
using System.Text.RegularExpressions;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Commands
{
    /// <summary>
    ///     Команда для фильтрации данных последователей по заданным полям и значениям.
    /// </summary>
    public partial class FilterCommand(List<Follower> followers) : ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name => "Отфильтровать данные";
        
        [GeneratedRegex("""^(".*?")(\s*,\s*".*?")*(,\s*)?$""")]
        private static partial Regex FilterValueRegex();

        /// <summary>
        ///     Выполняет фильтрацию данных на основе введенных пользователем параметров.
        /// </summary>
        /// <exception cref="IncorrectChoiceException">Некорректный формат данных</exception>
        /// <exception cref="ArgumentException">Некорректное значение для фильтрации</exception>
        public void Execute()
        {
            List<string> fields = new Follower().GetAllFields()
                .Where(f => f is not "aspects" and not "xtriggers")
                .ToList();

            Console.WriteLine("Доступные поля для фильтрации:");
            for (int i = 0; i < fields.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {fields[i]}");
            }

            Console.Write("Выберите номер поля для фильтрации: ");
            if (!int.TryParse(Console.ReadLine(), out int fieldIndex) || fieldIndex < 1 || fieldIndex > fields.Count)
            {
                throw new IncorrectChoiceException();
            }

            string selectedField = fields[fieldIndex - 1];

            Console.Write("Введите значения для фильтрации в формате \"значение1\",\"значение2\": ");
            string input = Console.ReadLine()!.Trim();

            if (string.IsNullOrEmpty(input) || !FilterValueRegex().IsMatch(input))
            {
                throw new ArgumentException("Некорректное значение для фильтрации");
            }

            string[] filterValues = input.Split('\"')
                .Where(x => x != ",")
                .Select(x => x.Trim())
                .ToArray();

            List<Follower> result = followers
                .Where(follower => FilterByField(follower, selectedField, filterValues))
                .ToList();

            Console.WriteLine("Результат фильтрации:");
            JsonParser.WriteJson(result.Cast<IJsonObject>().ToList());
            followers.Clear();
            result.ForEach(followers.Add);
        }

        /// <summary>
        ///     Проверяет, соответствует ли значение поля фильтру.
        /// </summary>
        /// <param name="follower">Проверяемый последователь.</param>
        /// <param name="field">Название поля.</param>
        /// <param name="filterValues">Массив допустимых значений.</param>
        /// <returns>True, если значение поля соответствует фильтру.</returns>
        private static bool FilterByField(Follower follower, string field, string[] filterValues)
        {
            string fieldValue = follower.GetField(field).Split(':')[1].Trim().Trim('"');
            return int.TryParse(fieldValue, out int intValue)
                ? filterValues.Any(v => int.TryParse(v, out int filterInt) && filterInt == intValue)
                : filterValues.Contains(fieldValue);
        }
    }
}