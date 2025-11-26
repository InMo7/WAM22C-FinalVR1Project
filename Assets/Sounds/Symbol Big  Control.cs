using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cymbal1Hit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;  // Crash cymbal

    [Header("Hit Settings")]
    public float minHitVelocity = 1.2f;  // Needs solid swing

    private XRGrabInteractable grab;

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;  // No loop for crash
        grab = GetComponent<XRGrabInteractable>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (grab != null && grab.isSelected) return;

        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < minHitVelocity) return;

        // Cymbal: High pitch, volume scales hard
        audioSource.pitch = 1.2f + (velocity * 0.15f);
        audioSource.volume = Mathf.Clamp01(velocity * 0.25f);

        audioSource.PlayOneShot(hitSound);
    }
}