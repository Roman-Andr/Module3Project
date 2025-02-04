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
    ///     Команда для отображения дерева продвижений последователей в консоли.
    /// </summary>
    public class DisplayTreeCommand(List<Follower> followers) : ICommand
    {
        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public string Name => "Отобразить дерево продвижений";

        /// <summary>
        ///     Запускает процесс отображения дерева, начиная с указанного ID.
        /// </summary>
        public void Execute()
        {
            Console.Write("Введите начальный id: ");
            string id = Console.ReadLine()!;
            DisplayTree(id, "");
        }

        /// <summary>
        ///     Рекурсивно отображает дерево последователей с заданным отступом.
        /// </summary>
        /// <param name="followerId">ID начального последователя.</param>
        /// <param name="indent">Отступ для визуализации иерархии.</param>
        /// <exception cref="FollowerNotFoundException">Последователь не найден</exception>
        private void DisplayTree(string followerId, string indent)
        {
            Dictionary<string, Follower> dict = followers.ToDictionary(f => f.Id, f => f);
            if (!dict.TryGetValue(followerId, out Follower follower))
            {
                throw new FollowerNotFoundException(followerId);
            }

            if (indent.Length == 0)
            {
                Console.WriteLine($"{indent}Последователь: {follower.Id} ({follower.Label})");
            }

            if (follower.XTriggers.Count <= 0)
            {
                return;
            }

            int count = 0;
            foreach ((string triggerKey, string triggerValue) in follower.XTriggers)
            {
                if (dict.TryGetValue(triggerValue, out Follower f))
                {
                    string branch = count == follower.XTriggers.Count - 1 ? "└── " : "├── ";
                    Console.WriteLine($"{indent}{branch}{triggerKey} --> {triggerValue} ({f.Label})");

                    string newIndent = indent + (count == follower.XTriggers.Count - 1 ? "    " : "│   ");
                    DisplayTree(triggerValue, newIndent);
                }
                else
                {
                    Console.WriteLine($"{indent}├── {triggerKey} --> {triggerValue}");
                }

                count++;
            }
        }
    }
}