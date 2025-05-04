using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerRunOut : MonoBehaviour
{
    [SerializeField] private LevelController levelController;
    private void OnTriggerEnter(Collider other)
    {
        levelController.OnEnterRunOutTrigger(other);
    }
}
