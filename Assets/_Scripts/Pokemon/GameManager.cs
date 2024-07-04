using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Scripts.Pokemon {
    public class GameManager : PersistentSingleton<GameManager> {
        public GameData gameData;

        [ReadOnly, SerializeField, Range(0f, 1f)]
        private float loadingProgress = 0;

        [ReadOnly, SerializeField, Range(0f, 1f)]
        private float loadingPercentPerS = 0.1f;

        [ReadOnly, SerializeField, Range(0f, 1f)]
        private float loadingCompletePercentPerS = 0.9f;

        [ReadOnly, SerializeField, Range(0f, 1f)]
        private float loadingPausePercentage = 0.9f;

        [ReadOnly, SerializeField]
        private bool isLoading = false;

        [ReadOnly, SerializeField]
        private bool isLoadingComplete = false;

        [ReadOnly, SerializeField]
        private GameObject playerPokemonParent;

        [ReadOnly, SerializeField]
        private GameObject enemyPokemonParent;

        public async Awaitable Load() {
            gameData = await GameData.Load();
            await BuildScene();
        }

        public async Awaitable Save() {
            await gameData.Save();
        }

        protected override void Awake() {
            base.Awake();
            Task.Run(Init);
        }

        private async Awaitable Init() {
            await Load();
            await Save();
        }

        public void StartLoading() {
            isLoading       = isLoadingComplete = false;
            loadingProgress = 0;
        }

        public void FinishLoading() {
            isLoadingComplete = true;
        }

        public void StopLoading() {
            isLoading = false;
        }

        [Button]
        private void ClearScene() {
        #if UNITY_EDITOR
            if (playerPokemonParent != null) {
                playerPokemonParent.DestroyChildrenImmediate();
                DestroyImmediate(playerPokemonParent);
                playerPokemonParent = null;
            }
            if (enemyPokemonParent != null) {
                enemyPokemonParent.DestroyChildrenImmediate();
                DestroyImmediate(enemyPokemonParent);
                enemyPokemonParent = null;
            }
        #endif
        #if !UNITY_EDITOR
            if (playerPokemonParent != null)
            {
                playerPokemonParent.DestroyChildren();
                Destroy(playerPokemonParent);
                playerPokemonParent = null;
            }
            if (enemyPokemonParent != null)
            {
                enemyPokemonParent.DestroyChildren();
                Destroy(enemyPokemonParent);
                enemyPokemonParent = null;
            }
        #endif
            Assert.IsNull(playerPokemonParent);
            Assert.IsNull(enemyPokemonParent);
        }

        [Button]
        private async Awaitable BuildScene() {
            StartLoading();

            ClearScene();

            if (playerPokemonParent == null) {
                playerPokemonParent = new GameObject {
                    name = "Player Pokemon"
                };
            }
            if (enemyPokemonParent == null) {
                enemyPokemonParent = new GameObject {
                    name = "Enemy Pokemon"
                };
            }

            if (gameData.playerPokemon.Count == 0) {
                gameData.playerPokemon.Add(
                    GeneratePokemon(
                        ScriptableObject.CreateInstance<PokemonData2>()
                    )
                );
            }
            gameData
                .playerPokemon.ForEach((pokemon) => InstantiatePokemon(pokemon, playerPokemonParent, true));

            if (gameData.enemyPokemon.Count == 0) {
                gameData.enemyPokemon.Add(
                    GeneratePokemon(
                        ScriptableObject.CreateInstance<PokemonData2>()
                    )
                );
            }
            gameData
                .enemyPokemon.ForEach((pokemon) => InstantiatePokemon(pokemon, enemyPokemonParent, false));
            await Awaitable.WaitForSecondsAsync(5);

            FinishLoading();
        }

        public PokemonController InstantiatePokemon(Pokemon pokemon, GameObject parent, bool isPlayer = false){
            GameObject        pokemonObject = parent.CreateChild(pokemon.data2.nickname ?? pokemon.data2.name);
            PokemonController controller    = pokemonObject.GetOrAdd<PokemonController>();
            controller.pokemon = pokemon;
            pokemonObject.AddComponent<SpriteRenderer>().sprite = pokemon.data2.sprite;
            if (isPlayer) {
                pokemonObject.transform.position = new Vector3(-5, -1, 0);
                pokemonObject.transform.localScale = new Vector3(-1, 1, 1);
            } else {
                pokemonObject.transform.position = new Vector3(5, 3.5f, 0);
                
            }
            return controller;
        }

        public Pokemon GeneratePokemon(PokemonData2 data2) {
            string[] pokemonNames = { "Pikachu" };
            data2.name     = pokemonNames[(int)Mathf.Floor(UnityEngine.Random.Range(0f, 4f))];
            data2.nickname = data2.name;
            List<BaseMove> moves = new List<BaseMove>();
            for (int i = 0; i < 4; i++) {
                if (data2.learnableMoves.Count > i) {
                    moves.Add(data2.learnableMoves[i]);
                }
            }

            Pokemon pokemon = new Pokemon {
                data2            = data2,
                level           = 5,
                IVs             = Stats.RandomIVs(),
                EVs             = new Stats(),
                attackBuff      = 0,
                defenseBuff     = 0,
                speedBuff       = 0,
                specialBuff     = 0,
                moves           = moves,
                statusCondition = StatusCondition.None
            };
            
            
            return pokemon;
        }

        private void Update() {
            if (isLoading) {
                if (!isLoadingComplete && loadingProgress <= loadingPausePercentage) {
                    loadingProgress +=
                        Time.deltaTime
                        * (isLoadingComplete ? loadingCompletePercentPerS : loadingPercentPerS);
                }

                if (loadingProgress >= 1f) {
                    StopLoading();
                }
            }
        }
    }
}