using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject advanceIcon;
    [SerializeField] private GameObject finishedIcon;

    // 상태 아이콘 변경
    public void SetState(NPCState newState, bool npcPoint)
    {
        canStartIcon.SetActive(false);
        advanceIcon.SetActive(false);

        switch (newState)
        {
            case NPCState.REQUIREMENTS_NOT_MET:
                if (npcPoint) { requirementsNotToStartIcon.SetActive(true); }
                break;
            case NPCState.CAN_START:
                if (npcPoint) { canStartIcon.SetActive(true); }
                break;
            case NPCState.IN_PROGRESS:
                if (npcPoint) { advanceIcon.SetActive(true); }
                break;
            case NPCState.FINISHED:
                if (npcPoint) { finishedIcon.SetActive(true); }
                break;
        }
    }
}
