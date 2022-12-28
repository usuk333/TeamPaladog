using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private IEnumerator Start()
    {
        image.DOFade(0, 2f);
        yield return new WaitForSeconds(1f);
        image.raycastTarget = false;
        yield return new WaitForSeconds(1f);
        image.gameObject.SetActive(false);
    }
}
