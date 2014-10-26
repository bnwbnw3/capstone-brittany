using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    private Animator _animator;
    private CanvasGroup _canvasGroup;

    public bool IsOpen
    {
        get { return _animator.GetBool("IsOpen"); }
        set { _animator.SetBool("IsOpen", value);}
    }

    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasGroup = GetComponent<CanvasGroup>();

        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);

    }

    public void Update()
    {
        _canvasGroup.blocksRaycasts = _canvasGroup.interactable = (_animator.GetCurrentAnimatorStateInfo(0).IsName("Open"));
    }
}
