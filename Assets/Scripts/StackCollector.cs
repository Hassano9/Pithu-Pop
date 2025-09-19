using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackCollector : MonoBehaviour
{
    [Header("Stack Settings")]
    public Transform stackHolder;
    public float stackHeight = 0.5f;
    public float moveSpeed = 4f;
    public float rotateSpeed = 12f;

    [Header("Game Settings")]
    public int winItemCount = 5;            // how many items required to win
    public Transform dropZone;              // where items go when dropped

    private List<Transform> stack = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Collect(other.transform);
        }
        else if (other.CompareTag("DropZone"))
        {
            StartCoroutine(DropItems());
        }
    }

    void Collect(Transform item)
    {
        Collider col = item.GetComponent<Collider>();
        if (col) col.enabled = false;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        Vector3 worldPos = item.position;
        Quaternion worldRot = item.rotation;

        item.SetParent(stackHolder);
        item.position = worldPos;
        item.rotation = worldRot;

        Vector3 targetLocalPos = Vector3.up * stack.Count * stackHeight;

        StartCoroutine(MoveToStack(item, targetLocalPos));
        stack.Add(item);
    }

    IEnumerator MoveToStack(Transform item, Vector3 targetLocalPos)
    {
        Vector3 startPos = item.localPosition;
        Quaternion startRot = item.localRotation;
        Quaternion targetRot = Quaternion.identity;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            item.localPosition = Vector3.Lerp(startPos, targetLocalPos, t);
            item.localRotation = Quaternion.Slerp(startRot, targetRot, t * rotateSpeed);
            yield return null;
        }

        item.localPosition = targetLocalPos;
        item.localRotation = targetRot;
    }

    IEnumerator DropItems()
    {
        Debug.Log("📦 Dropping items...");

        for (int i = stack.Count - 1; i >= 0; i--)
        {
            Transform item = stack[i];
            stack.RemoveAt(i);

            // Re-parent to dropZone
            item.SetParent(dropZone);

            Vector3 targetPos = dropZone.position + Vector3.up * i * stackHeight;
            Quaternion targetRot = Quaternion.identity;

            Vector3 startPos = item.position;
            Quaternion startRot = item.rotation;

            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * (moveSpeed * 0.5f);
                item.position = Vector3.Lerp(startPos, targetPos, t);
                item.rotation = Quaternion.Slerp(startRot, targetRot, t);
                yield return null;
            }

            item.position = targetPos;
            item.rotation = targetRot;

            yield return new WaitForSeconds(0.1f); // delay for staggered effect
        }

        // ✅ Check Win
        if (dropZone.childCount >= winItemCount)
        {
            Debug.Log("🎉 YOU WIN!");
            // Call your win UI / scene transition here
        }
    }
}
