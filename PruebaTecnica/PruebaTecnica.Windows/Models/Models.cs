using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace PruebaTecnica.Models
{
    public enum Gender { Female, Male };

    public enum Title { Madame, Mademoiselle, Miss, Monsieur, Mr, Mrs, Ms };

    public partial struct Postcode
    {
        public long? Integer;
        public string String;

        public static implicit operator Postcode(long Integer) => new Postcode { Integer = Integer };

        public static implicit operator Postcode(string String) => new Postcode { String = String };
    }

    public static class Serialize
    {
        public static string ToJson(this Root self) => JsonConvert.SerializeObject(self, Models.Converter.Settings);
    }

    public partial class Coordinates
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }

    public partial class Dob
    {
        [JsonProperty("age")]
        public long Age { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
    }

    public partial class Id
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("results")]
        public long Results { get; set; }

        [JsonProperty("seed")]
        public string Seed { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("postcode")]
        public Postcode Postcode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("timezone")]
        public Timezone Timezone { get; set; }
    }

    public partial class Login
    {
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("uuid")]
        public Guid Uuid { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }
    }

    public partial class Picture
    {
        [JsonProperty("large")]
        public Uri Large { get; set; }

        [JsonProperty("medium")]
        public Uri Medium { get; set; }

        [JsonProperty("thumbnail")]
        public Uri Thumbnail { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("cell")]
        public string Cell { get; set; }

        [JsonProperty("dob")]
        public Dob Dob { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("id")]
        public Id Id { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("login")]
        public Login Login { get; set; }

        [JsonProperty("name")]
        public Name Name { get; set; }

        [JsonProperty("nat")]
        public string Nat { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("picture")]
        public Picture Picture { get; set; }

        [JsonProperty("registered")]
        public Dob Registered { get; set; }
    }

    public partial class Root
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public partial class Root
    {
        public static Root FromJson(string json) => JsonConvert.DeserializeObject<Root>(json, Models.Converter.Settings);
    }

    public class RootDTO
    {
        public string Direccion { get; set; }

        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int ID { get; set; }

        public string Imagen { get; set; }
        public string Name { get; set; }
    }

    public partial class Timezone
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("offset")]
        public string Offset { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                GenderConverter.Singleton,
                PostcodeConverter.Singleton,
                TitleConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class GenderConverter : JsonConverter
    {
        public static readonly GenderConverter Singleton = new GenderConverter();

        public override bool CanConvert(Type t) => t == typeof(Gender) || t == typeof(Gender?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "female":
                    return Gender.Female;

                case "male":
                    return Gender.Male;
            }
            throw new Exception("Cannot unmarshal type Gender");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Gender)untypedValue;
            switch (value)
            {
                case Gender.Female:
                    serializer.Serialize(writer, "female");
                    return;

                case Gender.Male:
                    serializer.Serialize(writer, "male");
                    return;
            }
            throw new Exception("Cannot marshal type Gender");
        }
    }

    internal class PostcodeConverter : JsonConverter
    {
        public static readonly PostcodeConverter Singleton = new PostcodeConverter();

        public override bool CanConvert(Type t) => t == typeof(Postcode) || t == typeof(Postcode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new Postcode { Integer = integerValue };

                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Postcode { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Postcode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Postcode)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Postcode");
        }
    }

    internal class TitleConverter : JsonConverter
    {
        public static readonly TitleConverter Singleton = new TitleConverter();

        public override bool CanConvert(Type t) => t == typeof(Title) || t == typeof(Title?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "madame":
                    return Title.Madame;

                case "mademoiselle":
                    return Title.Mademoiselle;

                case "miss":
                    return Title.Miss;

                case "monsieur":
                    return Title.Monsieur;

                case "mr":
                    return Title.Mr;

                case "mrs":
                    return Title.Mrs;

                case "ms":
                    return Title.Ms;
            }
            throw new Exception("Cannot unmarshal type Title");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Title)untypedValue;
            switch (value)
            {
                case Title.Madame:
                    serializer.Serialize(writer, "madame");
                    return;

                case Title.Mademoiselle:
                    serializer.Serialize(writer, "mademoiselle");
                    return;

                case Title.Miss:
                    serializer.Serialize(writer, "miss");
                    return;

                case Title.Monsieur:
                    serializer.Serialize(writer, "monsieur");
                    return;

                case Title.Mr:
                    serializer.Serialize(writer, "mr");
                    return;

                case Title.Mrs:
                    serializer.Serialize(writer, "mrs");
                    return;

                case Title.Ms:
                    serializer.Serialize(writer, "ms");
                    return;
            }
            throw new Exception("Cannot marshal type Title");
        }
    }
}