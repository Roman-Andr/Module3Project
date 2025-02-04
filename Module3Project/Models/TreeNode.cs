/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Models
{
    /// <summary>
    ///     Узел дерева для визуализации связей между последователями.
    /// </summary>
    public class TreeNode(string id, string label, string icon, int level)
    {
        /// <summary>
        ///     Идентификатор узла.
        /// </summary>
        public string Id { get; } = id;

        /// <summary>
        ///     Надпись для отображения.
        /// </summary>
        public string Label { get; } = label;


        /// <summary>
        ///     Уровень вложенности узла в дереве.
        /// </summary>
        public int Level { get; } = level;

        /// <summary>
        ///     Иконка для отображения.
        /// </summary>
        public string Icon { get; } = icon;

        /// <summary>
        ///     Дочерние узлы.
        /// </summary>
        public List<(string Trigger, TreeNode Node)> Children { get; } = [];
    }
}