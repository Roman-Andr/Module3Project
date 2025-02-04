using JsonUtilities;
using Module3Project.Models;
using Module3Project.Utilities;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Commands
{
    /// <summary>
    ///     Команда для вывода данных в консоль или файл.
    /// </summary>
    public class WriteCommand(List<Follower> followers) : ChooseCommand
    {
        /// <summary>
        ///     Текст первой опции (вывод в консоль).
        /// </summary>
        protected override string Option1Text => "Вывести данные в консоль";

        /// <summary>
        ///     Текст второй опции (сохранение в файл).
        /// </summary>
        protected override string Option2Text => "Сохранить данные в файл";

        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public override string Name => "Вывести данные (консоль / файл)";

        /// <summary>
        ///     Выводит данные в консоль в формате JSON.
        /// </summary>
        protected override void HandleOption1()
        {
            JsonParser.WriteJson(followers.Cast<IJsonObject>().ToList());
            Console.WriteLine("Данные выведены в консоль");
        }

        /// <summary>
        ///     Сохраняет данные в JSON-файл по указанному пути.
        /// </summary>
        protected override void HandleOption2()
        {
            string path = FileUtils.EnterFilePath(false);
            JsonParser.WriteJson(followers.Cast<IJsonObject>().ToList(), path);
            Console.WriteLine($"Данные сохранены в файл: {path}");
        }
    }
}