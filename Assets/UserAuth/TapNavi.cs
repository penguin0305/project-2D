using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapNavigation : MonoBehaviour
{
    // 이동하고 싶은 순서대로 넣어야 함
    public Selectable[] selectables;
    public GameObject LoadingPanel;
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {

        if (LoadingPanel != null && LoadingPanel.activeInHierarchy) return;

        //탭키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (eventSystem.currentSelectedGameObject == null) return;

            GameObject current = eventSystem.currentSelectedGameObject;
            for (int i = 0; i < selectables.Length; i++)
            {
                if (selectables[i].gameObject == current)
                {
                    // Shift Tab이면 이전으로, 그냥 Tab이면 다음으로 이동
                    int nextIndex;
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        nextIndex = (i - 1 + selectables.Length) % selectables.Length;
                    }
                    else
                    {
                        nextIndex = (i + 1) % selectables.Length;
                    }

                    // 다음 오브젝트 선택
                    selectables[nextIndex].Select();
                    break;
                }
            }
        }
    }
}

