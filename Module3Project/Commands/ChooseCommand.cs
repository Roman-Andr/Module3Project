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
    ///     Абстрактный класс для команд с выбором между двумя опциями.
    ///     Реализует базовую логику запроса выбора у пользователя и выполнения соответствующего действия.
    /// </summary>
    public abstract class ChooseCommand : ICommand
    {
        /// <summary>
        ///     Текст первой опции для отображения пользователю.
        /// </summary>
        protected abstract string Option1Text { get; }

        /// <summary>
        ///     Текст второй опции для отображения пользователю.
        /// </summary>
        protected abstract string Option2Text { get; }

        /// <summary>
        ///     Название команды для отображения в меню.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     Выполняет команду: запрашивает выбор у пользователя и вызывает соответствующий обработчик.
        /// </summary>
        /// <exception cref="IncorrectChoiceException">Некорректный выбор</exception>
        public void Execute()
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine($"1. {Option1Text}");
            Console.WriteLine($"2. {Option2Text}");
            Console.Write("Ваш выбор: ");
            if (!(int.TryParse(Console.ReadLine(), out int choice) && choice is 1 or 2))
            {
                throw new IncorrectChoiceException("Некорректный выбор");
            }

            switch (choice)
            {
                case 1:
                    HandleOption1();
                    break;
                case 2:
                    HandleOption2();
                    break;
            }
        }

        /// <summary>
        ///     Обрабатывает выбор первой опции.
        /// </summary>
        protected abstract void HandleOption1();

        /// <summary>
        ///     Обрабатывает выбор второй опции.
        /// </summary>
        protected abstract void HandleOption2();
    }
}