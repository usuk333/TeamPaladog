using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class MushroomTest : MonoBehaviour
{
    public Image warningImg;
    public Text warningText;
    public GameObject timer;
    public SkeletonAnimation skeleton;

    public Image[] clearImage;
    public Text clearText;

    public Image result;

    public Transform test;

    private bool warning;
    private bool clear;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Co_WarnigAnim());
        StartCoroutine(Co_Clear());
    }
    private IEnumerator Co_WarnigAnim()
    {
        while (true)
        {
            if (warning)
            {          
                warningImg.gameObject.SetActive(true);
                warningImg.DOColor(Color.white, 2f);
                warningText.DOColor(Color.white, 2f);
                yield return new WaitForSeconds(2f);
                warningImg.DOColor(Color.clear, 2f);
                warningText.DOColor(Color.clear, 2f);
                yield return new WaitForSeconds(2f);
                warningImg.gameObject.SetActive(false);
                timer.SetActive(true);
                warning = false;
            }
            yield return null;
        }  
    }
    private IEnumerator Co_Clear()
    {
        while (true)
        {
            yield return null;
            if (clear)
            {
                foreach (var item in clearImage)
                {
                    item.DOFade(1, 1f);
                }
                clearText.DOFade(1, 1f);
                yield return new WaitForSeconds(1f);
                clearImage[0].transform.DOMoveY(910, 0.5f);
                yield return new WaitForSeconds(0.5f);
                result.gameObject.SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            warning = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            warningImg.gameObject.SetActive(true);
            warningImg.DOFade(1, 2f);
            warningText.DOFade(1, 2f);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            skeleton.Skeleton.A = 0;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            clear = true;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            test.DOScaleX(1, 2f);
        }
    }
}
