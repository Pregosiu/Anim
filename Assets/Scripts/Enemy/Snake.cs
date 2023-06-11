using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Animator _animator;
    private Transform _player;
    private float _distance;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tag.PLAYER).transform;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector2.Distance(_player.localPosition, transform.position);
        _animator.SetFloat(AnimatorValues.playerDistance, _distance);
    }


}
