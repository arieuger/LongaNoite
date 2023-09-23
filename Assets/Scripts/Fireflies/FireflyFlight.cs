using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyFlight : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float randomMovementRange = 0.5f;
    [SerializeField] private List<Transform> positions;
    [SerializeField] private LayerMask platformLayer;

    private Rigidbody2D _rb;
    private Vector3 _newPosition;
    private int _positionIndex = 0;
    private bool _shouldMove = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _newPosition = positions[_positionIndex].position;
        StartCoroutine(RandomMove());
    }

    void Update()     {
        if (Vector2.Distance(transform.position, _newPosition) < .5f) {
            PositionChange();
        }
        // transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * speed);
        if (_shouldMove) transform.position = Vector3.MoveTowards(transform.position, _newPosition, Time.deltaTime * speed);

    }

    public void ActivateMovement()
    {
        _shouldMove = true;
    }

    private void PositionChange()
    {
        if (_positionIndex.Equals(positions.Count - 1))
        {
            _positionIndex = 0;
            Invoke(nameof(DestroyGO), 1.5f);
        }
        else _positionIndex++;
        
        _newPosition = positions[_positionIndex].position;
        if (_positionIndex > 1 && _positionIndex < positions.Count -1) _shouldMove = false;
        
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }

    private IEnumerator RandomMove()
    {
        while (true)
        {
            var position = transform.position;
            position = new Vector3(position.x + (Random.Range(-randomMovementRange, randomMovementRange)),
                position.y + (Random.Range(-randomMovementRange, randomMovementRange)));
            RaycastHit2D raycastHit2D = Physics2D.Linecast(transform.position, position, platformLayer);

            if (raycastHit2D)
            {
                position = raycastHit2D.point - Vector2.ClampMagnitude(raycastHit2D.point, raycastHit2D.distance);
            }
                
            transform.position = position;
            
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
        }
    }
}
