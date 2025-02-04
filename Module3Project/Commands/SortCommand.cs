using JsonUtilities;
using Module3Project.Exceptions;
using Module3Project.Interfaces;
using Module3Project.Models;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Commands
{
    /// <summary>
    ///     Команда сортировки данных по заданному полю
    /// </summary>
    public class SortCommand(List<Follower> followers) : ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name => "Отсортировать данные";

        /// <summary>
        ///     Выполняет сортировку данных на основе введенных пользователем параметров.
        /// </summary>
        public void Execute()
        {
            List<string> fields = new Follower().GetAllFields().ToList();

            Console.WriteLine("Доступные поля для сортировки:");
            fields.Select((field, index) => $"{index + 1}. {field}").ToList().ForEach(Console.WriteLine);

            int fieldChoice = GetUserChoice("Выберите поле для сортировки: ", fields.Count);
            int sortDirection = GetUserChoice("Выберите сортировку (1 - возрастание, 2 - убывание): ", 2);

            List<Follower> result = SortFollowers(fields[fieldChoice - 1], sortDirection == 1);
            Console.WriteLine("Отсортированные данные:");
            followers.Clear();
            result.ForEach(followers.Add);
            JsonParser.WriteJson(result.Cast<IJsonObject>().ToList());
        }

        /// <summary>
        ///     Получить номер поля для сортировки
        /// </summary>
        /// <param name="message">Сообщение пользователю</param>
        /// <param name="maxChoice">Максимальный номер поля</param>
        /// <returns>Номер выбранного поля</returns>
        /// <exception cref="IncorrectChoiceException">Некорректный выбор</exception>
        private static int GetUserChoice(string message, int maxChoice)
        {
            Console.Write(message);
            return int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= maxChoice
                ? choice
                : throw new IncorrectChoiceException();
        }

        /// <summary>
        ///     Сортировка данных
        /// </summary>
        /// <param name="field">Поле для сортировки</param>
        /// <param name="ascending">По возрастанию / убыванию</param>
        /// <returns>Отсортированный список последователей</returns>
        /// <exception cref="ArgumentException">Неподдерживаемое поле для сортировки</exception>
        private List<Follower> SortFollowers(string field, bool ascending)
        {
            return
            [
                .. field switch
                {
                    "id" => ascending
                        ? followers.OrderBy(f => f.Id)
                        : followers.OrderByDescending(f => f.Id),
                    "label" => ascending
                        ? followers.OrderBy(f => f.Label)
                        : followers.OrderByDescending(f => f.Label),
                    "description" => ascending
                        ? followers.OrderBy(f => f.Description)
                        : followers.OrderByDescending(f => f.Description),
                    "uniquenessgroup" => ascending
                        ? followers.OrderBy(f => f.UniquenessGroup)
                        : followers.OrderByDescending(f => f.UniquenessGroup),
                    "icon" => ascending
                        ? followers.OrderBy(f => f.Icon)
                        : followers.OrderByDescending(f => f.Icon),
                    "lifetime" => ascending
                        ? followers.OrderBy(f => f.Lifetime)
                        : followers.OrderByDescending(f => f.Lifetime),
                    "decayTo" => ascending
                        ? followers.OrderBy(f => f.DecayTo)
                        : followers.OrderByDescending(f => f.DecayTo),
                    "comments" => ascending
                        ? followers.OrderBy(f => f.Comments)
                        : followers.OrderByDescending(f => f.Comments),
                    "animFrames" => ascending
                        ? followers.OrderBy(f => f.AnimFrames)
                        : followers.OrderByDescending(f => f.AnimFrames),
                    _ => throw new ArgumentException($"Поле {field} не поддерживается для сортировки.")
                }
            ];
        }
    }
}