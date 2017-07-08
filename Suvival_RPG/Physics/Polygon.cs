using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

public struct Polygon
{
	public Vector2[] Points;
	public Vector2[] Edges;

	public Polygon (int numPoints) {
		Points = new Vector2[numPoints];
		Edges = new Vector2[numPoints];
	}

	public void BuildEdges() {
		Vector2 p1;
		Vector2 p2;
		for (int i = 0; i < Points.Length; i++) {
			p1 = Points[i];
			if (i + 1 >= Points.Length) {
				p2 = Points[0];
			} else {
				p2 = Points[i + 1];
			}
			Edges[i] = (p2 - p1);
		}
	}
}

