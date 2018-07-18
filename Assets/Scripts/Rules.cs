using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessLib;

public class Rules : MonoBehaviour {

	DragAndDrop dad;
	Chess chess;
    const string letters = "abcdefgh";

	void Start () {
		dad = new DragAndDrop ();
		chess = new Chess ();

		ShowFigures ();
	}
	

	void Update ()
    {
		if(dad.Action())
        {

            string from = GetSquare(dad.pickPosition);
            string to = GetSquare(dad.dropPosition);
            string figure = chess.GetFigureAt(from);
            string move = figure + from + to;
            Debug.Log(figure);
            chess = chess.Move(move);
            
            ShowFigures();
        }
	}

    string GetSquare(Vector2 position)
    {
        int x = Convert.ToInt32(position.x);
        int y = Convert.ToInt32(position.y);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

	void ShowFigures()
	{
		int nr = 0;
		for (int y = 0; y < 8; y++)
			for (int x = 0; x < 8; x++) {
				string figure = chess.GetFigureAt (x, y).ToString ();
				if (figure == ".")
					continue;

				PlaceFigure ("box" + nr, figure, x, y);
				nr++;
			}
		for (; nr < 32; nr++)
			PlaceFigure ("box" + nr, "q", 9, 9);
	}

	void PlaceFigure(string box, string figure, int x, int y)
	{
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure);
        GameObject goSquare = GameObject.Find(letters[x] + "" + (y + 1));

        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;
        //spriteBox.name = spriteFigure.name;

        goBox.transform.position = goSquare.transform.position;
    }
}

class DragAndDrop{

	enum State
	{
		none,
		drag
	}

    public Vector2 pickPosition { get; private set; }
    public Vector2 dropPosition { get; private set; }

	State state;
	GameObject item;
	Vector2 offset;

	public DragAndDrop()
	{
		state = State.none;
		item = null;
	}

	public bool Action()
	{
		switch (state) 
		{
		case State.none:
			if (IsMouseButtonPressed ())
				PickUp ();
			break;
		case State.drag:
			if (IsMouseButtonPressed ())
				Drag ();
			else {
				Drop ();
				return true;
			}
			break;
		}
		return false;
	}

	bool IsMouseButtonPressed()
	{
		return Input.GetMouseButton (0);
	}

	void PickUp()
	{
		Vector2 clickPosition = GetClickPosition ();
		var clickedItem = GetItemAt (clickPosition);
		if (clickedItem == null)
			return;

        pickPosition = clickedItem.position;
		item = clickedItem.gameObject;
		state = State.drag;
		offset = pickPosition - clickPosition;
		//Debug.Log ("Picked up " + item.name);
	}

	Vector2 GetClickPosition()
	{
		return Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	Transform GetItemAt(Vector2 position)
	{
		RaycastHit2D[] figures = Physics2D.RaycastAll (position, position, 0.5f);
		if (figures.Length == 0)
			return null;
		return figures [0].transform;
	}

	void Drag()
	{
		item.transform.position = GetClickPosition () + offset;
	}

	void Drop()
	{
        dropPosition = item.transform.position;
		state = State.none;
		item = null;
    }

}
