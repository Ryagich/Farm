using System.Collections;
using System.Collections.Generic;
using Code.Digging;
using Code.Digging.Garden;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Code.UI.Storage
{
    public class StorageButtons : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset _buttonTree;
        [Space]
        [SerializeField] private float _startMargin = 150f;
        [SerializeField] private float _endMargin = 50f;
        [SerializeField] private float _delay = .2f;
        [SerializeField] private float _translation = 1f;
        [SerializeField] private EasingMode _easing;
        
        private UIDocument screen;
        private GardensInfo gardensInfo;
        
        private VisualElement container;
        private WaitForSeconds wait;
        private Coroutine coroutine;
        private DiggingController diggingC;
        
        [Inject]
        private void Constructor(UIDocument screen,
                                 DiggingController diggingC,
                                 GardensInfo gardensInfo)
        {
            this.screen = screen;
            this.gardensInfo = gardensInfo;
            this.diggingC = diggingC;
        }
        
        private void Start()
        {
            wait = new WaitForSeconds(_delay);
        }

        public void SpawnButtons()
        {
            wait = new WaitForSeconds(_delay);
            container = screen.rootVisualElement.Q<VisualElement>("Storage_Container");
            coroutine = StartCoroutine(SpawnButtonsCor());
        }

        public void StopSpawnButtons()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private IEnumerator SpawnButtonsCor()
        {
            var buttonsSize = .0f;
            foreach (var info in gardensInfo.Info)
            {
                // Хардкожу button width поскольку не могу получить значение (button width = 64)
                //TODO: Если кнопок больше чем может вместиться - показывать стрелочки прокрутки кнопок
                if (buttonsSize + _endMargin + 64 <= container.worldBound.width)
                {
                    buttonsSize += _endMargin + 64;
                    var button = _buttonTree.CloneTree();
                    button.Q<Label>("Text").text = info.Cost.ToString();
                    button.RegisterCallback<ClickEvent>(_ => OnClicked(info));
                    
                    button.style.transitionDuration = new List<TimeValue> { new(_translation, TimeUnit.Second) };
                    button.style.transitionTimingFunction =
                        new StyleList<EasingFunction>(new List<EasingFunction> { new(_easing) });
                    container.Add(button);

                    button.style.marginLeft = _startMargin;
                    yield return null;
                    button.style.marginLeft = _endMargin;

                    yield return wait;
                }
            }
        }

        private void OnClicked(GardenInfo info)
        {
            diggingC.ShowGarden(info);
        }
    }
}