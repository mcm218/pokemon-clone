using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Pokemon
{
    public class GameManager : PersistentSingleton<GameManager>
    {
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

        public async Awaitable Load()
        {
            gameData = GameData.Load();
            await BuildScene();
        }

        public void Save()
        {
            gameData.Save();
        }

        protected override void Awake()
        {
            base.Awake();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Load();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public void StartLoading()
        {
            isLoading = isLoadingComplete = false;
            loadingProgress = 0;
        }

        public void FinishLoading()
        {
            isLoadingComplete = true;
        }

        public void StopLoading()
        {
            isLoading = false;
        }

        [Button]
        private void ClearScene()
        {
#if UNITY_EDITOR
            if (playerPokemonParent != null)
            {
                DestroyImmediate(playerPokemonParent);
                playerPokemonParent = null;
            }
            if (enemyPokemonParent != null)
            {
                DestroyImmediate(enemyPokemonParent);
                enemyPokemonParent = null;
            }
#endif
#if !UNITY_EDITOR
            if (playerPokemonParent != null)
            {
                Destroy(playerPokemonParent);
                playerPokemonParent = null;
            }
            if (enemyPokemonParent != null)
            {
                Destroy(enemyPokemonParent);
                enemyPokemonParent = null;
            }
#endif
        }

        [Button]
        private async Awaitable BuildScene()
        {
            StartLoading();

            ClearScene();

            if (playerPokemonParent == null)
            {
                playerPokemonParent = new GameObject();
                playerPokemonParent.name = "Player Pokemon";
            }
            if (enemyPokemonParent == null)
            {
                enemyPokemonParent = new GameObject();
                enemyPokemonParent.name = "Enemy Pokemon";
            }

            Instantiate(playerPokemonParent);
            Instantiate(enemyPokemonParent);

            if (gameData.playerPokemon.Count == 0)
            {
                gameData.playerPokemon.Add(
                    GeneratePokemon(
                        ScriptableObject.CreateInstance<PokemonData>(),
                        playerPokemonParent
                    )
                );
            }
            else
            {
                gameData
                    .playerPokemon.ConvertAll(InstantiatePokemon)
                    .ForEach(controller =>
                    {
                        playerPokemonParent.transform.SetParent(controller.transform);
                        Instantiate(controller);
                    });
            }

            if (gameData.enemyPokemon.Count == 0)
            {
                gameData.enemyPokemon.Add(
                    GeneratePokemon(
                        ScriptableObject.CreateInstance<PokemonData>(),
                        enemyPokemonParent
                    )
                );
            }
            else
            {
                gameData
                    .enemyPokemon.ConvertAll(InstantiatePokemon)
                    .ForEach(controller =>
                    {
                        enemyPokemonParent.transform.SetParent(controller.transform);
                        Instantiate(controller);
                    });
            }

            await Awaitable.WaitForSecondsAsync(5);

            FinishLoading();
        }

        public PokemonController InstantiatePokemon(Pokemon pokemon)
        {
            GameObject pokemonObject = new GameObject();
            pokemonObject.transform.SetParent(enemyPokemonParent.transform);
            PokemonController controller = pokemonObject.AddComponent<PokemonController>();
            controller.pokemon = pokemon;
            pokemonObject.name = controller.pokemon.data.nickname ?? controller.pokemon.data.name;
            return controller;
        }

        public Pokemon GeneratePokemon(PokemonData data, GameObject parent)
        {
            string[] pokemonNames = { "Bulbasaur", "Charmander", "Squirtle", "Pikachu" };
            data.name = pokemonNames[(int)Mathf.Floor(UnityEngine.Random.Range(0f, 4f))];
            GameObject pokemonObject = new GameObject();
            pokemonObject.transform.SetParent(parent.transform);

            Pokemon pokemon = new Pokemon
            {
                data = data,
                level = 5,
                hp = 20,
                IVs = Stats.RandomIVs(),
                EVs = new Stats(),
                attackBuff = 0,
                defenseBuff = 0,
                speedBuff = 0,
                specialBuff = 0,
                moves = new List<IMove>(),
                statusCondition = StatusCondition.None
            };
            pokemonObject.AddComponent<PokemonController>().pokemon = pokemon;
            return pokemon;
        }

        private void Update()
        {
            if (isLoading)
            {
                if (!isLoadingComplete && loadingProgress <= loadingPausePercentage)
                {
                    loadingProgress +=
                        Time.deltaTime
                        * (isLoadingComplete ? loadingCompletePercentPerS : loadingPercentPerS);
                }

                if (loadingProgress >= 1f)
                {
                    StopLoading();
                }
            }
        }
    }
}
