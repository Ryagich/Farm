using System.Collections;
using System.Collections.Generic;
using Code.Game;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

namespace Code.UI.Storage
{
    public class StorageShower : MonoBehaviour
    {
        public UnityEvent StorageShowed;
        public UnityEvent StorageHided;

        [SerializeField] private UIDocument _screen;
        [Space]
        [SerializeField] private float _startMargin = 150f;
        [SerializeField] private float _endMargin = 50f;
        [Space]
        [SerializeField] private float _delay = .2f;
        [SerializeField] private float _translation = .2f;
        [SerializeField] private EasingMode _easing;
        [SerializeField] private VisualTreeAsset _storageTree;

        private VisualElement container;
        private WaitForSeconds wait;
        private Coroutine coroutine;
        private GameStateController gameStateController;

        [Inject]
        public void Constructor(GameStateController gameStateController)
        {
            this.gameStateController = gameStateController;
        }
        
        private void Start()
        {
            wait = new WaitForSeconds(_delay);
            container = _screen.rootVisualElement.Q<VisualElement>("Bottom").Q<VisualElement>("Container");
            gameStateController.GameState.Subscribe(ShowStorage);
        }

        private void ShowStorage(GameStates state)
        {
           coroutine = StartCoroutine(ShowStorageCor(state));
        }
        
        private IEnumerator ShowStorageCor(GameStates state)
        {
            if (state == GameStates.Redactor)
            {
                var storage = _storageTree.CloneTree();
                storage.style.transitionDuration = new List<TimeValue> { new(_translation, TimeUnit.Second) };
                storage.style.transitionTimingFunction = new StyleList<EasingFunction>(new List<EasingFunction> { new (_easing) });
                container.Add(storage);
                
                storage.style.marginBottom = _startMargin;
                yield return null;
                storage.style.marginBottom = _endMargin;

                yield return wait;
                StorageShowed?.Invoke();
            }
            if (state == GameStates.Game)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = null;
                container.Clear();
                StorageHided?.Invoke();
            }
        }
    }
}