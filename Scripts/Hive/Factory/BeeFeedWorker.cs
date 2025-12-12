using System.Collections;
using UnityEngine;

public class BeeFeedWorker : MonoBehaviour
{
    [Header("References")]
    public Transform tankLocation;
    public Transform conveyorLocation;

    [Header("Worker Settings")]
    public float movingSpeed = 3f;
    public float collectingTimeNeeded = 2f;
    public float collectAmountPerCycle = 10f;

    private bool isWorking = false;

    private void Start()
    {
        StartWorkCycle();
    }

    public void StartWorkCycle()
    {
        if (isWorking) return;

        StartCoroutine(WorkerRoutine());
    }

    private IEnumerator WorkerRoutine()
    {
        isWorking = true;

        yield return StartCoroutine(MoveTo(tankLocation.position));
        Debug.Log("[Worker] Collecting nectar...");
        yield return new WaitForSeconds(collectingTimeNeeded);
        float amountExtracted = CollectFromTank();
        Debug.Log($"[Worker] Extracted {amountExtracted} nectar");
        yield return StartCoroutine(MoveTo(conveyorLocation.position));

        Debug.Log("[Worker] Reached conveyor. Waiting for next task...");
        StartWorkCycle();

        isWorking = false;
    }

    private IEnumerator MoveTo(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                movingSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    private float CollectFromTank()
    {
        if (BeeHiveTank.Instance == null)
        {
            Debug.LogError("[Worker] Tank not found!");
            return 0f;
        }

        float tankAmount = BeeHiveTank.Instance.nectarStored;
        if (tankAmount <= 0)
        {
            Debug.Log("[Worker] Tank empty!");
            return 0;
        }

        float extracted = Mathf.Min(collectAmountPerCycle, tankAmount);

        BeeHiveTank.Instance.nectarStored -= extracted;

        return extracted;
    }
}
