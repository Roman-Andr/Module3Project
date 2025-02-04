using Module3Project.Utilities;
using SkiaSharp;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Models
{
    /// <summary>
    ///     Класс визуализатора дерева
    /// </summary>
    public class TreeDrawer
    {
        // Константы для отрисовки
        private const int NodeWidth = 500;
        private const int NodeHeight = 140;
        private const int NodeVIndent = NodeHeight / 2;
        private const int NodeHIndent = NodeWidth / 2;
        private const int LevelHorizontal = 300;
        private const int LevelVertical = 80;
        private const int ConnectorSpacing = 20;
        private const int TextPadding = 10;

        private readonly Dictionary<TreeNode, SKPoint> _positions = [];
        private readonly List<string> _imageDirs;
        private readonly TreeNode _root;
        
        private readonly SKPaint _borderPaint = new()
        {
            Style = SKPaintStyle.Stroke, Color = SKColors.Black, StrokeWidth = 4
        };

        private readonly SKFont _descFont =
            new(SKTypeface.FromFile(FileUtils.GetFont(Fonts.Slab))) { Size = 24 };

        private readonly SKFont _textFont =
            new(SKTypeface.FromFile(FileUtils.GetFont(Fonts.Sans))) { Size = 28 };

        private readonly SKPaint _linePaint = new()
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Gray,
            StrokeWidth = 4,
            PathEffect = SKPathEffect.CreateDash([6f, 6f], 0)
        };

        private readonly SKPaint _nodePaint = new()
        {
            Style = SKPaintStyle.Fill, Color = SKColors.White, StrokeWidth = 4
        };

        private readonly SKPoint _startPos = new(NodeHIndent, 50 + NodeVIndent);
        private readonly SKPaint _textPaint = new() { Color = SKColors.Black, IsAntialias = true };
        private readonly SKSize _size;

        /// <summary>
        ///     Конструктор визуализатора дерева
        /// </summary>
        /// <param name="follower">Начальный последователь</param>
        /// <param name="dict">Словарь всех последователей</param>
        /// <param name="dirs">Папки с иконками</param>
        public TreeDrawer(Follower follower, Dictionary<string, Follower> dict, List<string> dirs)
        {
            _root = BuildTree(follower, dict);
            _imageDirs = dirs;

            Stack<TreeNode> stack = new();
            stack.Push(_root);
            _positions.Clear();
            _size = new SKSize(0, 0);
            float currentY = _startPos.Y;
            while (stack.Count > 0)
            {
                TreeNode node = stack.Pop();
                float x = _startPos.X + (node.Level * LevelHorizontal);
                _positions[node] = new SKPoint(x, currentY);
                currentY += NodeHeight + LevelVertical;
                node.Children.AsEnumerable().Reverse().ToList().ForEach(child => stack.Push(child.Node));
                _size.Width = Math.Max(_size.Width, x + NodeHIndent);
                _size.Height = Math.Max(_size.Height, currentY);
            }
        }

        /// <summary>
        ///     Формирование дерева
        /// </summary>
        /// <param name="follower">Текущий последователь</param>
        /// <param name="dict">Данные об последователях</param>
        /// <param name="level">Текущий уровень вложенности</param>
        /// <returns></returns>
        private static TreeNode BuildTree(Follower follower, Dictionary<string, Follower> dict, int level = 0)
        {
            TreeNode node = new(
                follower.Id,
                string.IsNullOrEmpty(follower.Label) ? follower.Id : $"{follower.Label} ({follower.Id})",
                follower.Icon,
                level
            );

            node.Children.AddRange(follower.XTriggers.Select(trigger =>
            {
                (string key, string value) = trigger;
                return (key, dict.TryGetValue(value, out Follower childFollower)
                    ? BuildTree(childFollower, dict, level + 1)
                    : new TreeNode(value, value, "", level + 1));
            }).OrderByDescending(pair => pair.Item2.Children.Count));

            return node;
        }

        /// <summary>
        ///     Отрисовка дерева последователей начиная с корня
        /// </summary>
        public void Draw()
        {
            using SKSurface surface = SKSurface.Create(new SKImageInfo(
                (int)_size.Width + NodeWidth,
                (int)_size.Height + NodeHeight));
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.White);
            Draw(canvas, _root);
            FileUtils.SaveImage(surface, _root.Id);
        }

        /// <summary>
        ///     Отрисовка дерева последователей начиная с node
        /// </summary>
        /// <param name="canvas">Холст изображения</param>
        /// <param name="node">Узел отрисовки</param>
        private void Draw(SKCanvas canvas, TreeNode node)
        {
            SKPoint pos = _positions[node];
            SKRect rect = new(pos.X, pos.Y, pos.X + NodeWidth, pos.Y + NodeHeight);

            using SKBitmap? image = FileUtils.LoadImage(_imageDirs, node.Id) ??
                                    FileUtils.LoadImage(_imageDirs, node.Icon);

            const float imageSize = 128;
            float textOffset = image != null ? imageSize : 0;

            canvas.DrawRect(rect, _nodePaint);
            canvas.DrawRect(rect, _borderPaint);
            canvas.DrawText(
                StringUtils.Truncate(node.Label, NodeWidth - (TextPadding * 2) - textOffset, _textFont),
                pos.X + NodeHIndent + (textOffset / 2),
                pos.Y + NodeVIndent + 7, SKTextAlign.Center,
                _textFont, _textPaint
            );
            if (image != null)
            {
                canvas.DrawBitmap(image, new SKRect(
                    pos.X + 5,
                    pos.Y + ((NodeHeight - imageSize) / 2),
                    pos.X + 5 + imageSize,
                    pos.Y + ((NodeHeight - imageSize) / 2) + imageSize
                ));
            }

            foreach ((string trigger, TreeNode child) in node.Children)
            {
                SKPoint childPos = _positions[child];
                canvas.DrawLine(
                    pos.X + NodeHIndent, pos.Y + NodeHeight,
                    pos.X + NodeHIndent, childPos.Y - ConnectorSpacing,
                    _linePaint);
                canvas.DrawLine(
                    pos.X + NodeHIndent, childPos.Y - ConnectorSpacing,
                    childPos.X + NodeHIndent, childPos.Y - ConnectorSpacing,
                    _linePaint);
                canvas.DrawLine(
                    childPos.X + NodeHIndent, childPos.Y - ConnectorSpacing,
                    childPos.X + NodeHIndent, childPos.Y,
                    _linePaint);
                canvas.DrawText(
                    trigger,
                    (pos.X + childPos.X + NodeWidth) / 2, childPos.Y - ConnectorSpacing - 5,
                    SKTextAlign.Center, _descFont, _textPaint);
                Draw(canvas, child);
            }
        }
    }
}