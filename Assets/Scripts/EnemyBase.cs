using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider _slider;
    [SerializeField] float _offset;
    [SerializeField] float _hp;
    [SerializeField] string[] _animname;
    [SerializeField]AudioClip _clip;
    bool ss = true;
    UnityEngine.UI.Slider _hpslider;
    Animator _animator;
    
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _hpslider = Instantiate(_slider);
        _hpslider.maxValue = _hp;
        _hpslider.value = _hp;
        _hpslider.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
    private void OnDisable()
    {
        Destroy(_hpslider.gameObject);
    }
    public void Damage(float damage)
    {
        if(_hp > 0)
        {
            DOTween.To(() => _hp, x => _hp = x, _hp - damage, 0.5f).OnUpdate(() => _hpslider.value = _hp); // HPバーの更新を追加
            Debug.Log("hit!!!!!!!!!!!!!");
        }
        if (_hp <= 0)
        {
            if (ss)
            {
                GetComponent<AudioSource>().PlayOneShot(_clip);
                ss = false;
            }
            _animator.Play("Diee");
        }
    }

    private void LateUpdate()
    {
        // HPスライダーの位置更新
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, _offset, 0));
        _hpslider.GetComponent<RectTransform>().position = screenPos;
    
    }

    public void RandomAnim()
    {
        _animator.Play(_animname[Random.Range(0, _animname.Length)]);
    }
    public void ZeroHp()
    {
       
       gameObject.active = false;
        _animator.StopPlayback();
    }
}