using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using LastWeek.Model;
using LastWeek.Model.Enums;

namespace DataManager.Helpers
{
    public class EntryConverter : JsonConverter<Record>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(Record).IsAssignableFrom(typeToConvert);

        public override Record Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "EntryType")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            EntryType typeDiscriminator = Enum.Parse<EntryType>(reader.GetString());
            Record data = typeDiscriminator switch
            {
                EntryType.ChoiceEntry => new ChoiceRecord(),
                EntryType.RangeEntry => new RangeRecord(),
                EntryType.SimpleEntry => new SimpleRecord(),
                EntryType.TextEntry => new TextRecord(),
                EntryType.Entry => new Record(),
                _ => throw new JsonException()
            };

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return data;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    var value = reader.GetString();
                    // Each object will have to handle its way to read data from json (to avoid a huge switch for all properties here)

                    var entryType = data.GetType();
                    var pi = entryType.GetProperty(propertyName);
                    var des = JsonSerializer.Deserialize(value, pi.PropertyType);
                    if (pi.PropertyType == typeof(Range))
                    {
                        des = DeserializeRange(value);
                    }

                    pi.SetValue(data, des);
                }
            }

            throw new JsonException();
        }

        private Range DeserializeRange(string value)
        {
            var jObj = (JsonElement)JsonSerializer.Deserialize<object>(value);
            var start = jObj.GetProperty("Start");
            var end = jObj.GetProperty("End");
            var startIndex = new Index(start.GetProperty("Value").GetInt32(), start.GetProperty("IsFromEnd").GetBoolean());
            var endIndex = new Index(end.GetProperty("Value").GetInt32(), end.GetProperty("IsFromEnd").GetBoolean());
            return new Range(startIndex, endIndex);
        }

        public override void Write(Utf8JsonWriter writer, Record data, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            // Required to be first property written as it's used to determine instantiation type at deserialization
            writer.WriteString("EntryType", data.GetType().Name);
            var properties = data.GetType().GetProperties();
            var text = properties.Select(pi => pi.Name).ToArray();
            foreach (var pi in properties)
            {
                var serialized = JsonSerializer.Serialize(pi.GetValue(data));
                writer.WriteString(pi.Name, serialized);
            }

            writer.WriteEndObject();
        }
    }
}