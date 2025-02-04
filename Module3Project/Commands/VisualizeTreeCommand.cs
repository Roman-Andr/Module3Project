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
    ///     Команда для визуализации дерева продвижений в виде графического изображения.
    /// </summary>
    public class VisualizeTreeCommand(List<Follower> followers) : ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name => "Визуализировать дерево продвижений";

        /// <summary>
        ///     Выполнение команды
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">Папка не найдена</exception>
        /// <exception cref="FollowerNotFoundException">Последователь не найден</exception>
        public void Execute()
        {
            List<string> imageDirectories = [];
            Console.Write("Введите начальный id: ");
            string startId = Console.ReadLine()!;

            Console.Write("Введите пути к директориям с изображениями (через пробел): ");
            List<string> directories = [.. Console.ReadLine()!.Split(' ')];
            directories.ForEach(dir =>
            {
                if (!Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException();
                }
            });
            imageDirectories.AddRange(directories.Where(Directory.Exists));
            Dictionary<string, Follower> dict = followers.ToDictionary(f => f.Id, f => f);

            if (!dict.TryGetValue(startId, out Follower root))
            {
                throw new FollowerNotFoundException(startId);
            }

            TreeDrawer drawer = new(root, dict, imageDirectories);
            drawer.Draw();
        }
    }
}