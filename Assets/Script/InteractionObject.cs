/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using System.Collections;

namespace Oculus.Interaction
{
    /// <summary>
    /// Visually displays the current state of an interactable.
    /// </summary>
    public class InteractionObject : MonoBehaviour
    {
        [Tooltip("The interactable to monitor for state changes.")]
        [SerializeField, Interface(typeof(IInteractableView))]
        private UnityEngine.Object _interactableView;

        [Tooltip("The mesh that will change color based on the current state.")]
        [SerializeField]
        private Renderer _renderer;

        [Tooltip("Displayed when the state is normal.")]
        [SerializeField]
        private Color _normalColor = Color.red;

        [Tooltip("Displayed when the state is hover.")]
        [SerializeField]
        private Color _hoverColor = Color.blue;

        [Tooltip("Displayed when the state is selected.")]
        [SerializeField]
        private Color _selectColor = Color.green;

        [Tooltip("Displayed when the state is disabled.")]
        [SerializeField]
        private Color _disabledColor = Color.black;

        [Header("Notification UI")]
        [SerializeField]
        private GameObject notificationPanel;

        [SerializeField]
        private TextMeshProUGUI notificationText;

        [Header("Item Text")]
        [SerializeField]
        private TextMeshProUGUI itemText;

        private IInteractableView InteractableView;
        private Material _material;
        private Coroutine notificationCoroutine;
        private ItemManager itemManager;

        public Color NormalColor
        {
            get => _normalColor;
            set => _normalColor = value;
        }

        public Color HoverColor
        {
            get => _hoverColor;
            set => _hoverColor = value;
        }

        public Color SelectColor
        {
            get => _selectColor;
            set => _selectColor = value;
        }

        public Color DisabledColor
        {
            get => _disabledColor;
            set => _disabledColor = value;
        }

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractableView = _interactableView as IInteractableView;
            itemManager = FindObjectOfType<ItemManager>();
            Assert.IsNotNull(itemManager, "ItemManager is required.");
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));
            this.AssertField(_renderer, nameof(_renderer));
            _material = _renderer.material;

            Assert.IsNotNull(notificationPanel, "Notification panel is required.");
            Assert.IsNotNull(notificationText, "Notification Text component is required.");
            Assert.IsNotNull(itemText, "Item Text component is required.");

            notificationPanel.SetActive(false); // Ensure the notification panel is initially hidden

            UpdateVisual();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged += UpdateVisualState;
                UpdateVisual();
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractableView.WhenStateChanged -= UpdateVisualState;
            }
        }

        private void OnDestroy()
        {
            Destroy(_material);
        }

        public void SetNormalColor(Color color)
        {
            _normalColor = color;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            switch (InteractableView.State)
            {
                case InteractableState.Normal:
                    _material.color = _normalColor;
                    break;
                case InteractableState.Hover:
                    _material.color = _hoverColor;
                    break;
                case InteractableState.Select:
                    _material.color = _selectColor;
                    ShowNotification();
                    itemManager.ItemFound(itemText); // Notify the ItemManager that an item was found
                    break;
                case InteractableState.Disabled:
                    _material.color = _disabledColor;
                    break;
            }
        }

        private void UpdateVisualState(InteractableStateChangeArgs args) => UpdateVisual();

        private void ShowNotification()
        {
            notificationPanel.SetActive(true);

            notificationPanel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
            notificationPanel.transform.rotation = Camera.main.transform.rotation;

            if (notificationCoroutine != null)
                StopCoroutine(notificationCoroutine);
            notificationCoroutine = StartCoroutine(HideNotificationCoroutine(2f));
        }

        private IEnumerator HideNotificationCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            notificationPanel.SetActive(false);
        }

        #region Inject

        public void InjectAllInteractableDebugVisual(IInteractableView interactableView, Renderer renderer)
        {
            InjectInteractableView(interactableView);
            InjectRenderer(renderer);
        }

        public void InjectInteractableView(IInteractableView interactableView)
        {
            _interactableView = interactableView as UnityEngine.Object;
            InteractableView = interactableView;
        }

        public void InjectRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        #endregion
    }
}
