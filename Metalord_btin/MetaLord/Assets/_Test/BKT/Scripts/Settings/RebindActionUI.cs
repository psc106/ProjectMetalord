using System; // 시스템 네임스페이스를 사용합니다.
using System.Collections.Generic; // 제네릭 컬렉션을 사용합니다.
using UnityEngine.Events; // 유니티 이벤트 시스템을 사용합니다.
using UnityEngine.UI; // 유니티 UI 요소를 사용합니다.
using TMPro;

////TODO: localization support - 다국어 지원 추가 필요

////TODO: deal with composites that have parts bound in different control schemes - 다른 컨트롤 스키마에서 바인딩된 부분을 가진 복합체 처리 필요

namespace UnityEngine.InputSystem.Samples.RebindUI // InputSystem 예제 네임스페이스
{
    /// <summary>
    /// 단일 액션을 다시 바인딩하는 데 사용되는 자체 UI를 갖춘 재사용 가능한 컴포넌트입니다.
    /// </summary>
    public class RebindActionUI : MonoBehaviour // 액션 재바인딩을 위한 UI 컴포넌트 클래스
    {
        /// <summary>
        /// 다시 바인딩할 액션에 대한 참조입니다.
        /// </summary>
        public InputActionReference actionReference // 바인딩할 입력 액션에 대한 참조
        {
            get => m_Action; // 액션을 가져옵니다.
            set // 액션을 설정합니다.
            {
                m_Action = value;
                UpdateActionLabel(); // 액션 라벨을 업데이트합니다.
                UpdateBindingDisplay(); // 바인딩 표시를 업데이트합니다.
            }
        }

        /// <summary>
        /// 액션에 다시 바인딩될 바인딩의 ID(문자열 형식)입니다.
        /// </summary>
        /// <seealso cref="InputBinding.id"/>
        public string bindingId // 바인딩할 입력 액션의 ID
        {
            get => m_BindingId; // 바인딩 ID를 가져옵니다.
            set // 바인딩 ID를 설정합니다.
            {
                m_BindingId = value;
                UpdateBindingDisplay(); // 바인딩 표시를 업데이트합니다.
            }
        }

        /// <summary>
        /// 바인딩 표시에 대한 문자열 옵션입니다.
        /// </summary>
        public InputBinding.DisplayStringOptions displayStringOptions // 바인딩 표시에 대한 문자열 옵션
        {
            get => m_DisplayStringOptions; // 문자열 옵션을 가져옵니다.
            set // 문자열 옵션을 설정합니다.
            {
                m_DisplayStringOptions = value;
                UpdateBindingDisplay(); // 바인딩 표시를 업데이트합니다.
            }
        }

        /// <summary>
        /// 액션의 이름을 받는 텍스트 컴포넌트입니다. 선택 사항입니다.
        /// </summary>
        public TMP_Text actionLabel // 액션의 이름을 받는 텍스트 컴포넌트
        {
            get => m_ActionLabel; // 액션 라벨을 가져옵니다.
            set // 액션 라벨을 설정합니다.
            {
                m_ActionLabel = value;
                UpdateActionLabel(); // 액션 라벨을 업데이트합니다.
            }
        }

        /// <summary>
        /// 바인딩의 디스플레이 문자열을 받는 텍스트 컴포넌트입니다. <c>null</c>일 수 있으며,
        /// 이 경우 컴포넌트가 완전히 <see cref="updateBindingUIEvent"/>에 의존합니다.
        /// </summary>
        public TMP_Text bindingText // 바인딩 디스플레이 문자열을 받는 텍스트 컴포넌트
        {
            get => m_BindingText; // 바인딩 텍스트를 가져옵니다.
            set // 바인딩 텍스트를 설정합니다.
            {
                m_BindingText = value;
                UpdateBindingDisplay(); // 바인딩 표시를 업데이트합니다.
            }
        }

        /// <summary>
        /// 컨트롤이 활성화될 때 텍스트 프롬프트를 받는 선택적 텍스트 컴포넌트입니다.
        /// </summary>
        /// <seealso cref="startRebindEvent"/>
        /// <seealso cref="rebindOverlay"/>
        public TMP_Text rebindPrompt // 활성화될 때 텍스트 프롬프트를 받는 선택적 텍스트 컴포넌트
        {
            get => m_RebindText; // 다시 바인딩할 때 텍스트를 가져옵니다.
            set => m_RebindText = value; // 텍스트를 설정합니다.
        }

        /// <summary>
        /// 대화형 다시 바인딩이 시작되면 활성화되고, 다시 바인딩이 완료되면 비활성화되는 선택적 UI입니다. 일반적으로 시스템이 컨트롤이 활성화될 때까지 대기하는 동안 현재 UI 위에 오버레이를 표시하는 데 사용됩니다.
        /// </summary>
        /// <remarks>
        /// <see cref="rebindPrompt"/>나 <c>rebindOverlay</c> 둘 다 설정되지 않은 경우, 컴포넌트는 <see cref="bindingText"/>가 <c>null</c>이 아닌 경우 임시로 <c>"Waiting..."</c>으로 대체됩니다.
        /// </remarks>
        /// <seealso cref="startRebindEvent"/>
        /// <seealso cref="rebindPrompt"/>
        public GameObject rebindOverlay // 대화형 다시 바인딩 UI
        {
            get => m_RebindOverlay; // 대화형 다시 바인딩 UI 오브젝트를 가져옵니다.
            set => m_RebindOverlay = value; // 대화형 다시 바인딩 UI 오브젝트를 설정합니다.
        }

        /// <summary>
        /// UI가 현재 바인딩을 반영하기 위해 업데이트될 때마다 트리거되는 이벤트입니다. 사용자 지정 시각화를 바인딩에 연결하는 데 사용될 수 있습니다.
        /// </summary>
        public UpdateBindingUIEvent updateBindingUIEvent // 바인딩 UI 업데이트를 위한 이벤트
        {
            get
            {
                if (m_UpdateBindingUIEvent == null)
                    m_UpdateBindingUIEvent = new UpdateBindingUIEvent();
                return m_UpdateBindingUIEvent; // 바인딩 UI 업데이트 이벤트를 반환합니다.
            }
        }

        /// <summary>
        /// 액션에서 대화형 다시 바인딩이 시작될 때 트리거되는 이벤트입니다.
        /// </summary>
        public InteractiveRebindEvent startRebindEvent // 대화형 다시 바인딩 시작 시 이벤트
        {
            get
            {
                if (m_RebindStartEvent == null)
                    m_RebindStartEvent = new InteractiveRebindEvent();
                return m_RebindStartEvent; // 대화형 다시 바인딩 시작 이벤트를 반환합니다.
            }
        }

        /// <summary>
        /// 대화형 다시 바인딩이 완료되거나 취소될 때 트리거되는 이벤트입니다.
        /// </summary>
        public InteractiveRebindEvent stopRebindEvent // 대화형 다시 바인딩 종료 이벤트
        {
            get
            {
                if (m_RebindStopEvent == null) // 종료 이벤트가 없는 경우
                    m_RebindStopEvent = new InteractiveRebindEvent(); // 새로운 이벤트 생성
                return m_RebindStopEvent; // 대화형 다시 바인딩 종료 이벤트 반환
            }
        }

        /// <summary>
        /// 대화형 다시 바인딩이 진행 중일 때, 이것은 바인딩 작업 컨트롤러입니다. 그렇지 않으면 <c>null</c>입니다.
        /// </summary>
        public InputActionRebindingExtensions.RebindingOperation ongoingRebind // 대화형 다시 바인딩 진행 중인 경우 바인딩 작업 컨트롤러
        {
            get => m_RebindOperation; // 바인딩 작업 컨트롤러 반환
        }

        /// <summary>
        /// 컴포넌트가 대상으로 하는 바인딩에 대한 액션 및 바인딩 인덱스를 반환합니다.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="bindingIndex"></param>
        /// <returns></returns>
        public bool ResolveActionAndBinding(out InputAction action, out int bindingIndex) // 액션 및 바인딩 인덱스를 확인하는 메서드
        {
            bindingIndex = -1; // 초기화

            action = m_Action?.action; // 액션 설정
            if (action == null) // 액션이 없는 경우
                return false; // 실패 반환

            if (string.IsNullOrEmpty(m_BindingId)) // 바인딩 ID가 없는 경우
                return false; // 실패 반환

            // 바인딩 인덱스 조회.
            var bindingId = new Guid(m_BindingId); // 바인딩 ID 가져오기
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId); // 바인딩 인덱스 찾기
            if (bindingIndex == -1) // 인덱스를 찾지 못한 경우
            {
                Debug.LogError($"'{action}'에서 ID '{bindingId}'를 가진 바인딩을 찾을 수 없습니다.", this); // 에러 메시지 출력
                return false; // 실패 반환
            }

            return true; // 성공 반환
        }

        /// <summary>
        /// 현재 표시되는 바인딩을 새로고침합니다.
        /// </summary>
        public void UpdateBindingDisplay() // 바인딩 표시 업데이트 메서드
        {
            var displayString = string.Empty; // 표시할 문자열 초기화
            var deviceLayoutName = default(string); // 장치 레이아웃 이름 초기화
            var controlPath = default(string); // 컨트롤 경로 초기화

            // 액션에서 디스플레이 문자열 가져오기.
            var action = m_Action?.action; // 액션 가져오기
            if (action != null) // 액션이 있는 경우
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId); // 바인딩 인덱스 찾기
                if (bindingIndex != -1) // 인덱스를 찾은 경우
                    displayString = action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, displayStringOptions); // 디스플레이 문자열 가져오기
            }

            // 라벨에 설정합니다(있는 경우).
            if (m_BindingText != null) // 바인딩 텍스트가 있는 경우
                m_BindingText.text = displayString; // 텍스트 설정

            // 응답에 맞게 UI를 구성하는 리스너들에게 기회 부여합니다.
            m_UpdateBindingUIEvent?.Invoke(this, displayString, deviceLayoutName, controlPath); // UI 업데이트 이벤트 호출
        }

        /// <summary>
        /// 현재 적용된 바인딩 오버라이드를 제거합니다.
        /// </summary>
        public void ResetToDefault() // 현재 적용된 바인딩 오버라이드를 초기화하는 메서드입니다.
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex)) // 액션과 바인딩 인덱스를 확인합니다.
                return; // 확인에 실패하면 메서드를 종료합니다.

            if (action.bindings[bindingIndex].isComposite) // 바인딩이 복합적인 경우
            {
                // 복합 바인딩의 부분 바인딩에서 오버라이드를 제거합니다.
                for (var i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; ++i)
                    action.RemoveBindingOverride(i);
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex); // 단일 바인딩의 오버라이드를 제거합니다.
            }
            UpdateBindingDisplay(); // 바인딩 디스플레이를 업데이트합니다.
        }

        /// <summary>
        /// 플레이어가 제어를 활성화하여 액션에 대한 새로운 바인딩을 선택할 수 있는 대화형 다시 바인딩을 시작합니다.
        /// </summary>
        public void StartInteractiveRebind() // 대화형 다시 바인딩을 시작하는 메서드입니다.
        {
            if (!ResolveActionAndBinding(out var action, out var bindingIndex)) // 액션과 바인딩 인덱스를 확인합니다.
                return; // 확인에 실패하면 메서드를 종료합니다.

            // 바인딩이 복합적인 경우
            if (action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1; // 첫 번째 부분 바인딩의 인덱스
                if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                    PerformInteractiveRebind(action, firstPartIndex, allCompositeParts: true); // 대화형 다시 바인딩을 실행합니다.
            }
            else
            {
                PerformInteractiveRebind(action, bindingIndex); // 대화형 다시 바인딩을 실행합니다.
            }
        }

        private void PerformInteractiveRebind(InputAction action, int bindingIndex, bool allCompositeParts = false)
        {
            m_RebindOperation?.Cancel(); // m_RebindOperation을 null로 설정합니다.

            void CleanUp()
            {
                m_RebindOperation?.Dispose();
                m_RebindOperation = null;
            }

            //활성화된 인풋시스템 비활성화
            action.Disable();

            // 다시 바인딩 구성.
            m_RebindOperation = action.PerformInteractiveRebinding(bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnCancel(
                    operation =>
                    {
                        action.Enable();
                        m_RebindStopEvent?.Invoke(this, operation);
                        m_RebindOverlay?.SetActive(false);
                        UpdateBindingDisplay();
                        CleanUp();
                    })
                .OnComplete(
                    operation =>
                    {
                        action.Enable();
                        m_RebindOverlay?.SetActive(false);
                        m_RebindStopEvent?.Invoke(this, operation);
                        UpdateBindingDisplay();
                        CleanUp();

                        // 추가로 바인딩할 복합 부분이 있으면 다음 부분을 위해 다시 바인딩을 시작합니다.
                        if (allCompositeParts)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < action.bindings.Count && action.bindings[nextBindingIndex].isPartOfComposite)
                                PerformInteractiveRebind(action, nextBindingIndex, true);
                        }
                    });

            // 만약 바인딩이 부분적인 경우, UI에 부분의 이름을 표시합니다.
            var partName = default(string);
            if (action.bindings[bindingIndex].isPartOfComposite)
                partName = $"바인딩 '{action.bindings[bindingIndex].name}'. ";

            // 다시 바인딩 오버레이를 표시합니다(있는 경우).
            m_RebindOverlay?.SetActive(true);
            if (m_RebindText != null)
            {
                var text = !string.IsNullOrEmpty(m_RebindOperation.expectedControlType)
                    ? $"{partName}{m_RebindOperation.expectedControlType} 입력을 기다리는 중..."
                    : $"{partName}입력을 기다리는 중...";
                m_RebindText.text = text;
            }

            // 만약 다시 바인딩 오버레이와 콜백이 없지만 바인딩 텍스트 라벨이 있는 경우, 일시적으로 바인딩 텍스트 라벨을 "<Waiting>"으로 설정합니다.
            if (m_RebindOverlay == null && m_RebindText == null && m_RebindStartEvent == null && m_BindingText != null)
                m_BindingText.text = "<Waiting...>";

            // 다시 바인딩 시작 시 듣기자들에게 기회를 부여합니다.
            m_RebindStartEvent?.Invoke(this, m_RebindOperation);

            m_RebindOperation.Start();
        }

        protected void OnEnable() // 오브젝트가 활성화되면 호출되는 메서드입니다.
        {
            if (s_RebindActionUIs == null) // RebindActionUI 리스트가 비어있는 경우
                s_RebindActionUIs = new List<RebindActionUI>(); // 새로운 리스트를 생성합니다.
            s_RebindActionUIs.Add(this); // 현재 오브젝트를 리스트에 추가합니다.
            if (s_RebindActionUIs.Count == 1) // 리스트에 첫 번째 항목으로 추가되었다면
                InputSystem.onActionChange += OnActionChange; // InputSystem의 액션 변경 이벤트에 대한 리스너를 등록합니다.
        }

        protected void OnDisable() // 오브젝트가 비활성화되면 호출되는 메서드입니다.
        {
            m_RebindOperation?.Dispose(); // Rebind 작업이 존재하면 해제합니다.
            m_RebindOperation = null; // Rebind 작업을 null로 초기화합니다.

            s_RebindActionUIs.Remove(this); // 현재 오브젝트를 RebindActionUI 리스트에서 제거합니다.
            if (s_RebindActionUIs.Count == 0) // 리스트에 항목이 없다면
            {
                s_RebindActionUIs = null; // 리스트를 null로 설정합니다.
                InputSystem.onActionChange -= OnActionChange; // InputSystem의 액션 변경 이벤트 리스너를 해제합니다.
            }
        }

        // 액션 시스템이 바인딩을 다시 해석할 때 UI를 업데이트하려 합니다. 이로 인해 우리가 직접 만든 변화뿐만 아니라
        // 다른 곳에서 발생한 변화에도 반응합니다.
        private static void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.BoundControlsChanged) // 변경이 바운드된 컨트롤에 발생하지 않으면
                return; // 반환하고 함수 종료

            var action = obj as InputAction; // 입력 액션을 가져옵니다.
            var actionMap = action?.actionMap ?? obj as InputActionMap; // 액션 맵을 가져옵니다.
            var actionAsset = actionMap?.asset ?? obj as InputActionAsset; // 액션 어셋을 가져옵니다.

            for (var i = 0; i < s_RebindActionUIs.Count; ++i) // 반복문을 통해 모든 RebindActionUI를 확인합니다.
            {
                var component = s_RebindActionUIs[i]; // 현재 RebindActionUI를 가져옵니다.
                var referencedAction = component.actionReference?.action; // 참조된 액션을 가져옵니다.
                if (referencedAction == null) // 액션이 없으면
                    continue; // 다음으로 넘어갑니다.

                // 참조된 액션이나 액션 맵, 액션 어셋 중 하나라도 현재 액션과 일치하면
                if (referencedAction == action ||
                    referencedAction.actionMap == actionMap ||
                    referencedAction.actionMap?.asset == actionAsset)
                    component.UpdateBindingDisplay(); // 바인딩을 업데이트합니다.
            }
        }

        [Tooltip("UI에서 다시 바인딩될 액션에 대한 참조입니다.")]
        [SerializeField] // 시리얼라이즈된 필드로 Inspector 창에서 수정 가능합니다.
        private InputActionReference m_Action; // UI에서 다시 바인딩될 액션에 대한 참조

        [SerializeField]
        private string m_BindingId;

        [SerializeField]
        private InputBinding.DisplayStringOptions m_DisplayStringOptions;

        [Tooltip("액션 이름을 받을 텍스트 레이블입니다. 선택 사항입니다. 액션에 대한 레이블을 표시하지 않으려면 None으로 설정하세요.")]
        [SerializeField]
        private TMP_Text m_ActionLabel; // 액션 이름을 받는 텍스트 레이블. None으로 설정하면 레이블을 표시하지 않음

        [Tooltip("현재 포맷된 바인딩 문자열을 받을 텍스트 레이블입니다.")]
        [SerializeField]
        private TMP_Text m_BindingText; // 현재 포맷된 바인딩 문자열을 받는 텍스트 레이블

        [Tooltip("다시 바인딩 중일 때 표시되는 선택적 UI입니다.")]
        [SerializeField]
        private GameObject m_RebindOverlay; // 다시 바인딩 중일 때 표시되는 선택적 UI

        [Tooltip("사용자 입력을 위한 프롬프트로 업데이트되는 선택적 텍스트 레이블입니다.")]
        [SerializeField]
        private TMP_Text m_RebindText; // 사용자 입력을 위한 프롬프트로 업데이트되는 선택적 텍스트 레이블

        [Tooltip("바인딩이 표시되는 방법을 업데이트해야 할 때 트리거되는 이벤트입니다. 이를 통해 텍스트 대신 이미지 등을 사용하여 바인딩을 표시할 수 있습니다.")]
        [SerializeField]
        private UpdateBindingUIEvent m_UpdateBindingUIEvent; // 바인딩 표시 방법을 업데이트하는 이벤트

        [Tooltip("대화식 다시 바인딩이 시작될 때 트리거되는 이벤트입니다. 예를 들어 바인딩이 진행 중일 때 사용자 지정 UI 동작을 구현하는 데 사용할 수 있습니다. 또한 다시 바인딩을 추가로 사용자 지정하는 데 사용할 수 있습니다.")]
        [SerializeField]
        private InteractiveRebindEvent m_RebindStartEvent; // 대화식 다시 바인딩 시작 시 트리거되는 이벤트

        [Tooltip("대화식 다시 바인딩이 완료되거나 중단될 때 트리거되는 이벤트입니다.")]
        [SerializeField]
        private InteractiveRebindEvent m_RebindStopEvent; // 대화식 다시 바인딩 완료 또는 중단 시 트리거되는 이벤트

        private InputActionRebindingExtensions.RebindingOperation m_RebindOperation; // 다시 바인딩 작업을 담당하는 변수

        private static List<RebindActionUI> s_RebindActionUIs; // RebindActionUI 오브젝트 리스트

#if UNITY_EDITOR
        protected void OnValidate()
        {
            UpdateActionLabel(); // 액션 레이블을 업데이트하는 메서드 호출
            UpdateBindingDisplay(); // 바인딩 디스플레이를 업데이트하는 메서드 호출
        }
#endif

        private void UpdateActionLabel() // 액션 레이블을 업데이트하는 메서드
        {
            if (m_ActionLabel != null) // 액션 레이블이 null이 아닌 경우
            {
                var action = m_Action?.action; // 액션 참조를 가져옵니다.
                m_ActionLabel.text = action != null ? action.name : string.Empty; // 액션 이름을 텍스트로 설정합니다.
            }
        }

        [Serializable]
        public class UpdateBindingUIEvent : UnityEvent<RebindActionUI, string, string, string>
        {
            // 바인딩 UI를 업데이트하는 이벤트 클래스
        }

        [Serializable]
        public class InteractiveRebindEvent : UnityEvent<RebindActionUI, InputActionRebindingExtensions.RebindingOperation>
        {
            // 대화식 다시 바인딩 이벤트 클래스
        }


    }
}
