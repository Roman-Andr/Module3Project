/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

using SkiaSharp;
using System.Diagnostics;

namespace Module3Project.Utilities
{
    /// <summary>
    ///     Утилиты для работы с файлами.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        ///     Запрашивает путь к файлу у пользователя и проверяет его корректность.
        /// </summary>
        /// <param name="existsCheck">Проверять существование файла.</param>
        /// <returns>Полный путь к файлу.</returns>
        /// <exception cref="InvalidDataException">Некорректный аргумент</exception>
        public static string EnterFilePath(bool existsCheck = true)
        {
            DirectoryInfo dir = GetRoot();
            Console.WriteLine("Введите путь к файлу:");
            Console.Write(dir.FullName + Path.DirectorySeparatorChar);
            string path = Console.ReadLine()!;
            string fullPath = Path.Combine(dir.FullName, path);
            if (string.IsNullOrWhiteSpace(path) || !path.EndsWith(".json"))
            {
                throw new InvalidDataException();
            }

            if (existsCheck)
            {
                FileExists(fullPath);
            }

            return fullPath;
        }

        /// <summary>
        ///     Получить путь к шрифту в папке Fonts
        /// </summary>
        /// <param name="fileName">Имя файла шрифта</param>
        /// <returns>Полный путь к шрифту</returns>
        public static string GetFont(string fileName)
        {
            return Path.Combine(GetRoot().FullName, "Fonts", fileName);
        }

        /// <summary>
        ///     Получение корня решения
        /// </summary>
        /// <returns>Путь к корню решения</returns>
        private static DirectoryInfo GetRoot()
        {
            return Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!;
        }

        /// <summary>
        ///     Проверка существования файла
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        /// <exception cref="FileNotFoundException">Файл не найден</exception>
        private static void FileExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
        }

        /// <summary>
        ///     Загрузка иконок (Механизм А)
        /// </summary>
        /// <param name="dirs">Список папок для поиска иконок</param>
        /// <param name="id">Id последователя</param>
        /// <returns></returns>
        public static SKBitmap? LoadImage(List<string> dirs, string id)
        {
            foreach (string path in dirs.Select(dir => Path.Combine(dir, $"{id}.png")).Where(File.Exists))
            {
                using SKFileStream stream = new(path);
                return SKBitmap.Decode(stream);
            }

            return null;
        }

        /// <summary>
        ///     Сохраняет и открывает изображение
        /// </summary>
        /// <param name="surface">Изображение</param>
        /// <param name="id">Id начального последователя</param>
        /// <exception cref="FileLoadException">Ошибка открытия изображения</exception>
        public static void SaveImage(SKSurface surface, string id)
        {
            using SKImage? image = surface.Snapshot();
            using SKData? data = image.Encode(SKEncodedImageFormat.Png, 100);
            string path = Path.Combine(GetRoot().FullName, "Images");
            _ = Directory.CreateDirectory(path);
            path = Path.Combine(path, $"tree_{id}.png");
            using (FileStream stream = File.Create(path))
            {
                data.SaveTo(stream);
            }

            Console.WriteLine($"Дерево сохранено в {path}");
            try
            {
                _ = Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch
            {
                throw new FileLoadException();
            }
        }
    }
}