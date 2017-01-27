using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventGameObject : UnityEvent<GameObject> { }

public class Switcher : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();

    [SerializeField]
    private int currentTarget = 0;

    [SerializeField]
    private UnityEventGameObject onSwitch;

    public GameObject activeTarget
    {
        get { return targets[currentTarget]; }
    }

    private void Start()
    {
        // deactivate all gameobjects in the list
        for(int i = 0; i < targets.Count; ++i)
        {
            targets[i].SetActive(false);
        }

        // reactive the designated item
        activeTarget.SetActive(true);

        onSwitch.Invoke(activeTarget);
    }

    public GameObject SwitchNext()
    {
        activeTarget.SetActive(false);
        currentTarget = (currentTarget + 1) % targets.Count;

        activeTarget.SetActive(true);

        onSwitch.Invoke(activeTarget);

        return activeTarget;
    }
}
