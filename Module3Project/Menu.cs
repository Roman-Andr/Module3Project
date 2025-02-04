using Module3Project.Exceptions;
using Module3Project.Interfaces;
using Module3Project.Models;
using System.Security;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project
{
    /// <summary>
    ///     Реализация меню консольного приложения.
    /// </summary>
    public class Menu(List<ICommand> commands) : IMenu
    {
        private bool _isRunning = true;

        /// <summary>
        ///     Конструктор по умолчанию.
        /// </summary>
        public Menu() : this([]) { }

        /// <summary>
        ///     Список доступных команд.
        /// </summary>
        private List<ICommand> Commands { get; } = commands;

        /// <summary>
        ///     Останавливает выполнение меню.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        ///     Запускает цикл обработки команд.
        /// </summary>
        public void Run()
        {
            while (_isRunning)
            {
                try
                {
                    DisplayMenu();
                    int choice = GetUserChoice();
                    HandleChoice(choice);
                }
                catch (IncorrectChoiceException)
                {
                    Console.WriteLine("Ошибка! Некорректный выбор");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Ошибка обработки данных: {e.Message}");
                }
                catch (FollowerNotFoundException)
                {
                    Console.WriteLine("Ошибка! Последователь не найден");
                }
                catch (FileLoadException)
                {
                    Console.WriteLine("Ошибка открытия изображения");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Ошибка! Указана несуществующая папка");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Ошибка! Файл не найден");
                }
                catch (Exception e) when (e is IOException
                                              or UnauthorizedAccessException
                                              or InvalidOperationException
                                              or SecurityException
                                              or NotSupportedException
                                              or PathTooLongException)
                {
                    Console.WriteLine("Ошибка при работе с файлом.");
                }
                catch (Exception e) when (e is FileNotFoundException
                                              or InvalidDataException
                                              or InvalidCastException
                                              or ArgumentNullException
                                              or OverflowException
                                              or ObjectDisposedException)
                {
                    Console.WriteLine("Ошибка! Некорректные данные");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                if (!_isRunning)
                {
                    continue;
                }

                Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                _ = Console.ReadKey();
            }
        }

        /// <summary>
        ///     Добавляет команды в меню.
        /// </summary>
        public void AddCommands(params ICommand[] commands)
        {
            Commands.AddRange(commands);
        }

        /// <summary>
        ///     Получить номер команды от пользователя
        /// </summary>
        /// <returns>Номер команды</returns>
        private int GetUserChoice()
        {
            Console.Write("Выберите пункт меню: ");
            string input = Console.ReadLine()!;
            return !int.TryParse(input, out int choice) || choice < 1 || choice > Commands.Count ? -1 : choice;
        }

        /// <summary>
        ///     Отобразить меню
        /// </summary>
        private void DisplayMenu()
        {
            Console.Clear();
            for (int i = 0; i < Commands.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Commands[i].Name}");
            }
        }

        /// <summary>
        ///     Вызов команды по номеру
        /// </summary>
        /// <param name="choice">Номер команды</param>
        /// <exception cref="IncorrectChoiceException">Некорректный выбор</exception>
        private void HandleChoice(int choice)
        {
            if (choice > 0 && choice <= Commands.Count)
            {
                Commands[choice - 1].Execute();
            }
            else
            {
                throw new IncorrectChoiceException();
            }
        }
    }
}