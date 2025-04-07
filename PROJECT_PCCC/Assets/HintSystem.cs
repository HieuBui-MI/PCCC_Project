using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HintSystem : MonoBehaviour
{
    [Header("References")]
    public InteractionSystem interactionSystem;
    public GameObject containerHints;

    [Header("Hint Prefabs")]
    public GameObject genericHintPrefab;

    private List<GameObject> currentHints = new List<GameObject>();
    private GameObject lastTarget;

    private void Update()
    {
        GameObject currentTarget = interactionSystem.TargetObject;

        if (currentTarget == null)
        {
            ClearHints();
            lastTarget = null;
            return;
        }

        if (currentTarget != lastTarget)
        {
            lastTarget = currentTarget;
            Interactable interactable = currentTarget.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                ShowHints(interactable);
            }
            else
            {
                ClearHints();
            }
        }
    }

    private void ShowHints(Interactable interactable)
    {
        ClearHints();

        // Lấy danh sách các hint cần hiển thị
        List<string> hintTexts = GetHintTexts(interactable.type, interactable.gameObject);

        foreach (string hintText in hintTexts)
        {
            if (!string.IsNullOrEmpty(hintText))
            {
                SpawnHint(hintText);
            }
        }
    }

    private void SpawnHint(string hintText)
    {
        if (genericHintPrefab != null && containerHints != null)
        {
            GameObject newHint = Instantiate(genericHintPrefab, containerHints.transform);
            TMP_Text textComponent = newHint.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = hintText;
            }
            currentHints.Add(newHint);
        }
    }

    private List<string> GetHintTexts(Interactable.InteractableType type, GameObject target)
    {
        List<string> hintTexts = new List<string>();

        switch (type)
        {
            case Interactable.InteractableType.Drivable:
                CarController car = target.GetComponentInParent<CarController>();
                if (car != null)
                {
                    if (car.driver != null)
                    {
                        hintTexts.Add("Press Q to leave");
                        hintTexts.Add("Hold W to go forward");
                        hintTexts.Add("Hold S to reverse");
                    }
                    else
                    {
                        hintTexts.Add("Press \"F\" to drive");
                    }
                }
                break;

            case Interactable.InteractableType.Carriable:
                hintTexts.Add("Press \"F\" to carry");
                break;
            case Interactable.InteractableType.Putable:
                hintTexts.Add("Press \"F\" to put");
                break;
            case Interactable.InteractableType.Climbable:
                hintTexts.Add("Press \"F\" to climb");
                break;
            case Interactable.InteractableType.Take:
                hintTexts.Add("Press \"F\" to take");
                break;
            case Interactable.InteractableType.Connectable:
                hintTexts.Add("Press \"F\" to connect");
                break;
            default:
                break;
        }

        return hintTexts;
    }

    public void ForceUpdateHint()
    {
        if (lastTarget != null)
        {
            Interactable interactable = lastTarget.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                ShowHints(interactable);
            }
        }
    }

    private void ClearHints()
    {
        foreach (GameObject hint in currentHints)
        {
            Destroy(hint);
        }
        currentHints.Clear();
    }
}
