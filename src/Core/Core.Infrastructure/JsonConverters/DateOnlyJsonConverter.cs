﻿using Newtonsoft.Json;
using System.Globalization;

namespace Core.Infrastructure.JsonConverters
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "dd/MM/yyyy";

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
            => writer.WriteValue(value.ToString(Format, CultureInfo.InvariantCulture));

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
            => DateOnly.ParseExact(reader.Value as string ?? string.Empty, Format, CultureInfo.InvariantCulture);
    }
}
