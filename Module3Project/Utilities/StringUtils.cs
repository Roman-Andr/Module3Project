using SkiaSharp;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Utilities
{
    public static class StringUtils
    {
        /// <summary>
        ///     Обрезка текста под заданную ширину в шрифте
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="maxWidth">Ширина</param>
        /// <param name="font">Шрифт</param>
        /// <returns>Итоговую строку</returns>
        public static string Truncate(string text, float maxWidth, SKFont font)
        {
            if (font.MeasureText(text) <= maxWidth)
            {
                return text;
            }

            while (font.MeasureText(text + "...") > maxWidth && text.Length > 0)
            {
                text = text[..^1];
            }

            return text + "...";
        }
    }
}