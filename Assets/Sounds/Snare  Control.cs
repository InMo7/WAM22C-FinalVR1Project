using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnareDrumHit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;  // Snare crack

    [Header("Hit Settings")]
    public float minHitVelocity = 1f;

    private XRGrabInteractable grab;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return;

        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < minHitVelocity) return;

        // Snare: Brighter pitch
        audioSource.pitch = 1.0f + (velocity * 0.1f);
        audioSource.volume = Mathf.Clamp01(velocity * 0.15f);

        audioSource.PlayOneShot(hitSound);
    }
}