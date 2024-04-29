using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using C_Script.Base.Abstract;
using C_Script.Data;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class NewHPBar : MonoBehaviour
{
    private Transform _target;
    private EntityDataSo _data;
    public Image hpImg;
    public Image hpEffectImg;
    private const float BuffTime = 0.5f;
    private Coroutine updateCoroutime;

    private void Awake()
    {
        _data = GetComponentInParent<Entity>().EntityDataSo;
    }

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("MainCamera").transform;
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        hpImg.fillAmount = 1.0f * ((float)_data.CurrentHealth / _data.MaxHealth);
    }
    private IEnumerator UpdateHpEffect()
    {
        float effectLength = hpEffectImg.fillAmount - hpImg.fillAmount;
        float elapsedTime = 0f;
        while (elapsedTime < BuffTime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            hpEffectImg.fillAmount = Mathf.Lerp(hpImg.fillAmount + effectLength, hpImg.fillAmount, elapsedTime / BuffTime);
            yield return null;
        }
        hpEffectImg.fillAmount = hpImg.fillAmount;
    }
    // Update is called once per frame
    void Update()
    {
        transform.forward = new Vector3(transform.position.x - _target.position.x, 0, transform.position.z - _target.position.z);
    }
}