using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AStarPathfinding;

public class GhostController : MonoBehaviour 
{
	public Vector2 ReturnLocation = new Vector2(9, -10);

	private Animator _animator;
    public Transform PacMan;
	public Vector2 moveToLocation;
	public float speed;


    public bool isDead = false;

	void Start()
	{
		_animator = GetComponent<Animator>();
		GameDirector.Instance.GameStateChanged.AddListener(GameStateChanged);

		Move();
	}

	public void Move()
	{
		List<Vector3> _path = new List<Vector3>();
		PathFinding.Instance.generatePath(transform.position, moveToLocation, _path);
		if (_path.Count >= 2)
		{
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(_path[1].x, _path[1].y * -1, 0),
													"speed", speed,
													"easetype", "linear",
													"oncomplete", "moveComplete"));
		}
		else
		{
			StartCoroutine(WaitToMove());
		}
	}

	public IEnumerator WaitToMove()
	{
		yield return new WaitForSeconds(1);
		Move();
	}

	public void moveComplete()
	{
		if (GameDirector.Instance.gameOver == false)
		{
			Move();
		}
	}

	public void Kill()
	{
		_animator.SetBool("IsDead", true);
        isDead = true;
	}

    public void Revive()
    {
        _animator.SetBool("IsDead", false);
        isDead = false;
    }

	public void GameStateChanged(GameDirector.States _state)
	{
		switch (_state)
		{
			case GameDirector.States.enState_Normal:
				_animator.SetBool("IsGhost", false);
				break;

			case GameDirector.States.enState_PacmanInvincible:
				_animator.SetBool("IsGhost", true);
				break;

			case GameDirector.States.enState_GameOver:
				break;
		}
	}
}
