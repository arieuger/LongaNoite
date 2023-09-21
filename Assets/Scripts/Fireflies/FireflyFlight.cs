using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyFlight : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float randomMovementRange = 0.5f;
    [SerializeField] private List<Transform> positions; 

    private Rigidbody2D _rb;
    private Vector3 _newPosition;
    private int _positionIndex = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _newPosition = positions[_positionIndex].position;
        StartCoroutine(RandomMove());
    }

    void PositionChange()
    {
        if (_positionIndex.Equals(positions.Count - 1)) _positionIndex = 0;
        else _positionIndex++;
        
        _newPosition = positions[_positionIndex].position;
    }

    private IEnumerator RandomMove()
    {
        while (true)
        {
            var position = transform.position;
            position = new Vector3(position.x + (Random.Range(-randomMovementRange, randomMovementRange)),
                position.y + (Random.Range(-randomMovementRange, randomMovementRange)));
            transform.position = position;
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
        }
    }

    void Update()     {
        if (Vector2.Distance(transform.position, _newPosition) < .5f)    {
            PositionChange();
        }
        // transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * speed);
        transform.position = Vector3.MoveTowards(transform.position, _newPosition, Time.deltaTime * speed);

    }
}
