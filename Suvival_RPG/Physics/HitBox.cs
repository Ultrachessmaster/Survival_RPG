using System.Collections;
using System;
using Microsoft.Xna.Framework;
using Engine;

public class HitBox {
	public Vector2 size = Vector2.One;
	public Vector2 offset = Vector2.Zero;
    public Vector2 vel;
    public Vector2 pos;
    public float rotation;
    public int layer = 0;
	public bool trigger = false;
    public bool kinematic = false;
    const int highbound = 0;
	const int lowbound = 1;
	public Polygon polygon = new Polygon(4);
    public bool enabled = true;
    public Entity entity;

    public HitBox (Vector2 pos, Vector2 size, Entity e, bool markerhitbox = false)
    {
        this.pos = pos;
        this.size = size;
        entity = e;
        UpdatePolygon();
    }

    public void UpdatePolygon() {
        //right up, right down, left up, (left down is left as -ru)
		Vector2 ru = size / 2f;
		Vector2 rd = new Vector2 (size.X, -size.Y) / 2f;
		Vector2 lu = new Vector2 (-size.X, size.Y) / 2f;
        Vector2 ld = -ru;
		float[] angle = new float[4];
		angle[0] = Angle(offset + ru);
        angle[1] = Angle(offset + rd);
        angle[2] = Angle(offset + ld);
		angle[3] = Angle(offset + lu);
		float[] magnitudes = new float[4];

        magnitudes[0] = (offset + ru).Length();
		magnitudes[1] = (offset + rd).Length();
        magnitudes[2] = (offset + ld).Length();
        magnitudes[3] = (offset + lu).Length();
		Vector2[] angleoffsets = new Vector2[4];
        //offsets of individual points, then adjusting for pos. Angle should not be the same for all of them.
		for(int i = 0; i < angleoffsets.Length; i++)
			angleoffsets[i] = new Vector2 ((float)System.Math.Cos(angle[i]) * magnitudes[i], (float)System.Math.Sin(angle[i]) * magnitudes[i]) + pos;
        polygon.Points = angleoffsets;
		polygon.BuildEdges();
	}
	
	float Angle (Vector2 off) {
        //var offy = System.Math.Sign(off.Y);
        off.Normalize();
        //var angle = (offy == -1 ? -off.Angle(Vector2.UnitX)
        //    : off.Angle(Vector2.UnitX));
        //angle += rotation;
        float angle = off.Angle(Vector2.UnitX) + rotation;
        return angle;
	}

	// Calculate the projection of a polygon on an axis
	// and returns it as a [min, max] interval
	void ProjectPolygon(Vector2 axis, Polygon polygon, 
		ref float min, ref float max) {
		// To project a point on an axis use the dot product

		float dotProduct = Vector2.Dot (axis, polygon.Points [0]);
		min = dotProduct;
		max = dotProduct;
		for (int i = 0; i < polygon.Points.Length; i++) {
			dotProduct = Vector2.Dot (polygon.Points[i], axis);
			if (dotProduct < min) {
				min = dotProduct;
			} else {
				if (dotProduct > max) {
					max = dotProduct;
				}
			}
		}
	}

	float IntervalDistance(float minA, float maxA, float minB, float maxB) {
		if (minA < minB) {
			return minB - maxA;
		} else {
			return minA - maxB;
		}
	}

	public bool IsOverlapping(HitBox col) {
		Polygon polygonA = polygon;
		Polygon polygonB = col.polygon;
		bool overlap = true;

		int edgeCountA = polygonA.Edges.Length;
		int edgeCountB = polygonB.Edges.Length;
		Vector2 edge;

		// Loop through all the edges of both polygons
		for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++) {
			if (edgeIndex < edgeCountA) {
				edge = polygonA.Edges [edgeIndex];
			} else {
				edge = polygonB.Edges [edgeIndex - edgeCountA];
			}

			// ===== 1. Find if the polygons are currently intersecting =====

			// Find the axis perpendicular to the current edge
			Vector2 axis = new Vector2 (-edge.Y, edge.X);
			axis.Normalize ();

			// Find the projection of the polygon on the current axis
			float minA = 0;
			float minB = 0;
			float maxA = 0;
			float maxB = 0;
			ProjectPolygon (axis, polygonA, ref minA, ref maxA);
			ProjectPolygon (axis, polygonB, ref minB, ref maxB);

			// Check if the polygon projections are currentlty intersecting
			if (IntervalDistance (minA, maxA, minB, maxB) > 0)
				overlap = false;
		}
		return overlap;
	}

	public float closestDistance (Vector2 pos) {
		Vector2 closestPoint = new Vector2();
		for(int i = 0; i < polygon.Points.Length; i++) {
			var point = polygon.Points[i];
			if (Vector2.Distance (point, pos) < Vector2.Distance (closestPoint, pos)) {
				closestPoint = point;
			}
		}
		return Vector2.Distance (closestPoint, pos);
	}

	public void Destroy() {
		Physics.RemoveCollider (this);
	}
}
