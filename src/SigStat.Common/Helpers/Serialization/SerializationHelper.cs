﻿using Newtonsoft.Json;
using SigStat.Common.Helpers.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SigStat.Common.Helpers
{
    public class SerializationHelper
    {
        public static T Deserialize<T>(string s) where T : new()
        {
            T desirializedObject = JsonConvert.DeserializeObject<T>(s, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new VerifierResolver()
            });

            return desirializedObject;
        }
        public static T DeserializeFromFile<T>(string path) where T:new()
        {
            T desirializedObject = JsonConvert.DeserializeObject<T>(File.ReadAllText(path), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver =  new VerifierResolver()
            });

            return desirializedObject;
        }

        public static void JsonSerializeToFile<T>(T o,string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new VerifierResolver()
            }));
        }

        public static string JsonSerialize<T>(T o)
        {
           return JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
               ContractResolver = new VerifierResolver()
           });
        }
    }
}