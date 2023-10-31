using System;
using System.Collections.Generic;
using Core.Scenes;
using Core.User;
using Core.Utility.UI.Navigation;
using UI.InGame.Screen.GameOver;
using UI.InGame.Screen.InGame;
using UI.Title.Option;
using UnityEngine;

namespace UI.InGame
{
    public class InGameUIManager: MonoBehaviour
    {
        [SerializeField] private InGameManager inGameManager;
        [SerializeField] private GameOverManager gameOverManager;
        [SerializeField] private OptionManager optionManager;
        private Navigation<InGameNav> navigation;

        private SceneChanger sceneChanger;

        public event Action OnGameInactive;
        public event Action OnGameActive;

        private void Awake()
        {
            navigation = new Navigation<InGameNav>(
                new Dictionary<InGameNav, UIManager>
                {
                    { InGameNav.InGame, inGameManager },
                    { InGameNav.GameOver, gameOverManager },
                    { InGameNav.Option, optionManager }
                }
            );
        }

        private void Start()
        {
            gameOverManager.OnSelect += () =>
            {
                navigation.SetActive(false);
                sceneChanger.LoadTitle(TitleNavigation.StageSelect);
            };

            gameOverManager.OnRetry += () =>
            {
                navigation.SetActive(false);
                sceneChanger.LoadInGame(sceneChanger.GetInGame());
            };

            optionManager.OnBack += () =>
            {
                navigation.SetActive(true);
                navigation.SetScreen(InGameNav.InGame);
                OnGameActive?.Invoke();
            };
            
            navigation.SetActive(true);
        }

        public void StartGame(UserPreference preference)
        {
            optionManager.SetPreference(preference);
            navigation.SetActive(true);
            navigation.SetScreen(InGameNav.InGame);
        }

        public void StartOption()
        {
            navigation.SetScreen(InGameNav.Option, isReset: true);
            navigation.SetActive(false);
            OnGameInactive?.Invoke();
        }

        public void SetGameOver()
        {
            navigation.SetScreen(InGameNav.GameOver, isReset: true);
            OnGameInactive?.Invoke();
        }

        public void SetSceneChanger(SceneChanger scene)
        {
            sceneChanger = scene;
        }

        public void UpdateStageProgress(int value)
        {
            inGameManager.SetStageProgress((uint)value);
        }
        
        public void SetHp(short current, short? max = null)
        {
            if (max.HasValue)
            {
                inGameManager.SetHp((uint)current, (uint)max.Value);
            }
            else
            {
                inGameManager.SetHp((uint)current);   
            }
        }
        
        public void SetWorkerCount(uint value, uint? max = null)
        {
            inGameManager.SetWorkerCount(value, max);
        }
        
        public void SetResourceCount(uint current, uint? max = null)
        {
            inGameManager.SetResource(current, max);
        }
    }
    
    public enum InGameNav {
        /**
         Tutorial*** ...
         */
        
        GameOver,
        Option,
        InGame,
    }
}