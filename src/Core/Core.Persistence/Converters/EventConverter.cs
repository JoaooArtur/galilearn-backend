﻿using Core.Domain.Primitives;
using Core.Infrastructure.JsonConverters;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace Core.Persistence.Converters
{
    public class EventConverter() : ValueConverter<IDomainEvent?, string>(
        @event => JsonConvert.SerializeObject(@event, typeof(IDomainEvent), SerializerSettings()),
        jsonString => JsonConvert.DeserializeObject<IDomainEvent>(jsonString, DeserializerSettings()))
    {
        private static JsonSerializerSettings SerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings = new()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            jsonSerializerSettings.Converters.Add(new DateOnlyJsonConverter());

            return jsonSerializerSettings;
        }

        private static JsonSerializerSettings DeserializerSettings()
        {
            JsonSerializerSettings jsonDeserializerSettings = new()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new PrivateSetterContractResolver()
            };

            jsonDeserializerSettings.Converters.Add(new DateOnlyJsonConverter());

            return jsonDeserializerSettings;
        }
    }
}
