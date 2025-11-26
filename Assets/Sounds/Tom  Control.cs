using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TomDrumHit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;  // Tom thump

    [Header("Hit Settings")]
    public float minHitVelocity = 0.8f;
    public float tomRadius = 0.3f;  // Adjust to your tom size

    private XRGrabInteractable grab;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return;

        Vector3 hitPoint = collision.contacts[0].point - transform.position;
        float distFromCenter = hitPoint.magnitude / tomRadius;
        float velocity = collision.relativeVelocity.magnitude;

        if (velocity < minHitVelocity) return;

        // Tom: Pitch higher near edge
        float pitch = 0.7f + (distFromCenter * 0.4f) + (velocity * 0.08f);
        audioSource.pitch = pitch;
        audioSource.volume = Mathf.Clamp01(velocity * 0.18f);

        audioSource.PlayOneShot(hitSound);
    }
}