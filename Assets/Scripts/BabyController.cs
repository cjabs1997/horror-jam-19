using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour
{
    [Header("Traits")]
    [Tooltip("How long the baby can be left on the ground before 'expiring.'")]
    [SerializeField] private float expirationTime;

    [Header("Events")]
    [Tooltip("Raised when the baby is picked up.")]
    [SerializeField] GameEvent babyPickedUpEvent;
    [Tooltip("Raised when the baby is dropped.")]
    [SerializeField] GameEvent babyDroppedEvent;
    [Tooltip("Raised when the baby has been left on the ground too long and expires.")]
    [SerializeField] GameEvent babyExpiredEvent;

    Coroutine DroppedTimerRoutine;

    public void BabyPickedUp()
    {
        if (babyPickedUpEvent != null)
            babyPickedUpEvent.Raise();
        else
            Debug.LogWarning("Ayo we didn't set a pickup event, silly.");

        if(DroppedTimerRoutine != null)
            StopCoroutine(DroppedTimerRoutine);
    }

    public void BabyDropped(bool startTimer = true)
    {
        if (babyDroppedEvent != null)
            babyDroppedEvent.Raise();
        else
            Debug.LogWarning("Ayo we didn't set a drop event, silly.");

        if(startTimer)
            DroppedTimerRoutine = StartCoroutine(DroppedTimer(expirationTime));
    }

    public void BabyExpired()
    {
        if (babyExpiredEvent != null)
            babyExpiredEvent.Raise();
        else
            Debug.LogWarning("Ayo we didn't set an expired event, silly.");


        Debug.LogError("<color=#FF0000>YOU LOSE OR SOMETHING, the baby died</color>");
        // Do other stuff maybe...
    }

    IEnumerator DroppedTimer(float timeRemaining)
    {
        yield return new WaitForSeconds(timeRemaining);

        BabyExpired();
    }
}
