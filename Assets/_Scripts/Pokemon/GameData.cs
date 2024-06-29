using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _Scripts.Pokemon {
    [Serializable, CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
    public class GameData : SerializedScriptableObject, ISaveable<GameData>
    {
        public List<Pokemon> playerPokemon = new List<Pokemon>();
        public List<Pokemon> enemyPokemon  = new List<Pokemon>();

        public List<Pokemon> playerSelectedPokemon = new List<Pokemon>();
        public List<Pokemon> enemySelectedPokemon  = new List<Pokemon>();

        public void Save()
        {
            byte[] bytes = SerializationUtility.SerializeValue(this, DataFormat.JSON);
            File.WriteAllBytes(Application.persistentDataPath + "/gameData.json", bytes);
        }

        public static GameData Load()
        {
            if (File.Exists(Application.persistentDataPath + "/gameData.json"))
            {
                byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/gameData.json");
                return SerializationUtility.DeserializeValue<GameData>(bytes, DataFormat.JSON);
            }
            return new GameData();
        }
    }
}