using JsonUtilities;

/*
 * Андрусевич Роман Дмитриевич
 * БПИ244
 * Вариант 16
 */

namespace Module3Project.Models
{
    /// <summary>
    ///     Структура, представляющая последователя
    /// </summary>
    public struct Follower(
        string id = "0",
        string label = "",
        string description = "",
        string uniquenessGroup = "",
        string icon = "",
        int lifetime = 0,
        string decayTo = "",
        string comments = "",
        int animFrames = 0,
        Dictionary<string, int>? aspects = null,
        Dictionary<string, string>? xtriggers = null
    ) : IJsonObject
    {
        /// <summary>
        ///     Идентификатор последователя.
        /// </summary>
        public string Id { get; private set; } = id;

        /// <summary>
        ///     Подпись последователя.
        /// </summary>
        public string Label { get; private set; } = label;

        /// <summary>
        ///     Описание последователя.
        /// </summary>
        public string Description { get; private set; } = description;

        /// <summary>
        ///     Словарь аспектов последователя.
        /// </summary>
        public Dictionary<string, int> Aspects { get; private set; } = aspects ?? [];

        /// <summary>
        ///     Словарь триггеров продвижения.
        /// </summary>
        public Dictionary<string, string> XTriggers { get; private set; } =
            xtriggers ?? [];

        /// <summary>
        ///     Уникальная группа.
        /// </summary>
        public string UniquenessGroup { get; private set; } = uniquenessGroup;

        /// <summary>
        ///     Иконка
        /// </summary>
        public string Icon { get; private set; } = icon;

        /// <summary>
        ///     Время жизни
        /// </summary>
        public int Lifetime { get; private set; } = lifetime;

        /// <summary>
        ///     Поле decayTo
        /// </summary>
        public string DecayTo { get; private set; } = decayTo;

        /// <summary>
        ///     Комментарии
        /// </summary>
        public string Comments { get; private set; } = comments;

        /// <summary>
        ///     Количество кадров анимации
        /// </summary>
        public int AnimFrames { get; private set; } = animFrames;

        /// <summary>
        ///     Возвращает список всех доступных полей структуры.
        /// </summary>
        public readonly IEnumerable<string> GetAllFields()
        {
            return
            [
                "id", "label", "description", "uniquenessgroup", "icon", "lifetime", "decayTo", "comments",
                "animFrames", "aspects", "xtriggers"
            ];
        }

        /// <summary>
        ///     Получает значение указанного поля в формате JSON.
        /// </summary>
        /// <param name="fieldName">Название поля.</param>
        public readonly string GetField(string fieldName)
        {
            string fieldKey = fieldName;
            string? fieldValue = fieldKey switch
            {
                "id" => Id,
                "label" => Label,
                "description" => Description,
                "uniquenessgroup" => UniquenessGroup,
                "icon" => Icon,
                "lifetime" => Lifetime.ToString(),
                "decayTo" => DecayTo,
                "comments" => Comments,
                "animFrames" => AnimFrames.ToString(),
                "aspects" => JsonUtils.SerializeDictionary(Aspects),
                "xtriggers" => JsonUtils.SerializeDictionary(XTriggers),
                _ => null
            };

            return fieldKey is "aspects" or "xtriggers" or "lifetime" or "animFrames"
                ? $"\"{fieldKey}\": {fieldValue}"
                : $"\"{fieldKey}\": \"{fieldValue}\"";
        }

        /// <summary>
        ///     Устанавливает значение указанного поля.
        /// </summary>
        /// <param name="fieldName">Название поля.</param>
        /// <param name="value">Значение.</param>
        /// <exception cref="ArgumentException">Неизвестное поле</exception>
        public void SetField(string fieldName, string value)
        {
            switch (fieldName)
            {
                case "id":
                    Id = value;
                    break;
                case "label":
                    Label = value;
                    break;
                case "description":
                    Description = value;
                    break;
                case "uniquenessgroup":
                    UniquenessGroup = value;
                    break;
                case "icon":
                    Icon = value;
                    break;
                case "lifetime":
                    Lifetime = int.Parse(value);
                    break;
                case "decayTo":
                    DecayTo = value;
                    break;
                case "comments":
                    Comments = value;
                    break;
                case "animFrames":
                    AnimFrames = int.Parse(value);
                    break;
                case "aspects":
                    Aspects = JsonUtils.DeserializeDictionary<int>(value);
                    break;
                case "xtriggers":
                    XTriggers = JsonUtils.DeserializeDictionary<string>(value);
                    break;
                default:
                    throw new ArgumentException($"Поле {fieldName} не существует.");
            }
        }
    }
}