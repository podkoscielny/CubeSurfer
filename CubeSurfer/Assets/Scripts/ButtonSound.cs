using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioClip hoverSound;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioSource clickAudio;

    private AudioSource _hoverAudio;

    void Start()
    {
        _hoverAudio = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData) => _hoverAudio.PlayOneShot(hoverSound);

    public void OnPointerClick(PointerEventData eventData) => clickAudio.PlayOneShot(clickSound);
}
