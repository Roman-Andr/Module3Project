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
    ///     Команда для чтения данных из консоли или файла.
    /// </summary>
    public class ReadCommand(List<Follower> followers) : ChooseCommand
    {
        /// <summary>
        ///     Текст первой опции (чтение из консоли).
        /// </summary>
        protected override string Option1Text => "Ввести данные в консоль";

        /// <summary>
        ///     Текст второй опции (чтение из файла).
        /// </summary>
        protected override string Option2Text => "Загрузить данные из файла";

        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public override string Name => "Ввести данные (консоль / файл)";

        /// <summary>
        ///     Обработка выбора чтения из консоли.
        /// </summary>
        protected override void HandleOption1()
        {
            Console.WriteLine("Введите данные (завершите ввод Ctrl+Z):");
            LoadData();
        }

        /// <summary>
        ///     Обработка выбора чтения из файла.
        /// </summary>
        protected override void HandleOption2()
        {
            string path = FileUtils.EnterFilePath();
            using TextReader reader = new StreamReader(path);
            Console.SetIn(reader);
            LoadData();
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }

        /// <summary>
        ///     Загружает данные из указанного источника и обновляет список последователей.
        /// </summary>
        private void LoadData()
        {
            string data = Console.In.ReadToEnd();
            followers.Clear();
            JsonParser.ReadJson(data, () => new Follower())
                .Cast<Follower>().ToList().ForEach(followers.Add);
            Console.WriteLine($"Загружено {followers.Count} записей");
        }
    }
}