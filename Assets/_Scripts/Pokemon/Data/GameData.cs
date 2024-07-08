using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.Pokemon {
    [Serializable, CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
    public class GameData : SerializedScriptableObject, ISaveable<GameData> {
        public const string DEFAULT_SAVE_NAME = "gameData";
        private      string saveName          = DEFAULT_SAVE_NAME;

        public string SaveName => saveName;

        public List<Pokemon> playerPokemon = new List<Pokemon>();
        public List<Pokemon> enemyPokemon  = new List<Pokemon>();

        public List<Pokemon> playerSelectedPokemon = new List<Pokemon>();
        public List<Pokemon> enemySelectedPokemon  = new List<Pokemon>();

        public async Awaitable UpdateName(string newName)
        {
            Assert.IsFalse(string.IsNullOrEmpty(newName), "Save name cannot be null or empty");
            Assert.AreEqual(saveName, newName);
            string oldName = saveName;
            saveName = newName;

            if (File.Exists(Application.persistentDataPath + $"/{oldName}.json"))
            {
                File.Move(Application.persistentDataPath + $"/{oldName}.json", Application.persistentDataPath + $"/{saveName}.json");
            }
            else
            {
                await Save();
            }
        }

        public async Awaitable Save()
        {
            byte[] bytes = SerializationUtility.SerializeValue(this, DataFormat.JSON);
            await File.WriteAllBytesAsync(Application.persistentDataPath + $"/{saveName}.json", bytes);
        }

        public static async Awaitable<GameData> Load(string name = DEFAULT_SAVE_NAME)
        {
            if (File.Exists(Application.persistentDataPath + $"/{name}.json"))
            {
                byte[] bytes = await File.ReadAllBytesAsync(Application.persistentDataPath + $"/{name}.json");
                return SerializationUtility.DeserializeValue<GameData>(bytes, DataFormat.JSON);
            }
            return CreateInstance<GameData>();
        }

        public static Awaitable Delete(string name = DEFAULT_SAVE_NAME)
        {
            if (File.Exists(Application.persistentDataPath + $"/{name}.json"))
            {
                File.Delete(Application.persistentDataPath + $"/{name}.json");
            }
            return default;
        }
    }
}