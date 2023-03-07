using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

namespace ETC
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private float delay;
        private Image image;
        private void Awake()
        {
            image = GetComponent<Image>();
        }
        private IEnumerator Start()
        {
            image.DOFade(0, delay);
            yield return new WaitForSeconds(delay - 1f);
            image.raycastTarget = false;
            yield return new WaitForSeconds(1f);
            image.gameObject.SetActive(false);
        }
    }
}
