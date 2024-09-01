using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float _hp;
    [SerializeField] float _attackInterbal;
    float _attackTime;
     void FixedUpdate()
    {
        _attackTime += Time.deltaTime;
        if(_attackInterbal < _attackTime)
        {
            _attackTime = 0;
        } 
    }
}
