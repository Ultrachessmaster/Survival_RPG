using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

public static class Physics {
	static List<HitBox> hitboxes = new List<HitBox> ();
    static float dist = 15f;
	public static void Update () {
        for(int i = hitboxes.Count - 1; i >= 0; i--)
        {
            var hitbox = hitboxes[i];
            if (hitbox.trigger || hitbox.kinematic)
            {
                hitbox.pos += hitbox.vel;
                hitbox.UpdatePolygon();
                continue;
            }
            if (!hitbox.enabled || !hitbox.entity.enabled || hitbox.vel == Vector2.Zero)
                continue;
            hitbox.UpdatePolygon();
            HitBox largeCol;
            Vector2 size = (hitbox.size * 2) + new Vector2(Math.Abs(hitbox.vel.X), Math.Abs(hitbox.vel.Y));
            largeCol = new HitBox(hitbox.pos, size, null, true);
            largeCol.offset = hitbox.offset;
            largeCol.trigger = true;
            largeCol.UpdatePolygon();

            var localObjs = GetCollisions(largeCol);

            Vector2 nextvelocity = hitbox.vel;
            hitbox.offset += new Vector2(hitbox.vel.X, 0);
            hitbox.UpdatePolygon();
            bool xtouching = IsColliding(hitbox, localObjs);
            hitbox.offset -= new Vector2(hitbox.vel.X, 0);
            hitbox.UpdatePolygon();

            if (xtouching)
            {
                while (!IsColliding(hitbox, localObjs))
                {
                    hitbox.pos += new Vector2(Math.Sign(hitbox.vel.X), 0);
                    hitbox.UpdatePolygon();
                }
                hitbox.pos += new Vector2(-Math.Sign(hitbox.vel.X), 0);
                hitbox.UpdatePolygon();
                nextvelocity.X = 0;
            }
            hitbox.offset += new Vector2(0, hitbox.vel.Y);
            hitbox.UpdatePolygon();
            bool ytouching = IsColliding(hitbox, localObjs);
            hitbox.offset -= new Vector2(0, hitbox.vel.Y);
            hitbox.UpdatePolygon();
            if (ytouching)
            {
                while (!IsColliding(hitbox, localObjs))
                {
                    hitbox.pos += new Vector2(0, Math.Sign(hitbox.vel.Y));
                    hitbox.UpdatePolygon();
                }
                hitbox.pos += new Vector2(0, -Math.Sign(hitbox.vel.Y));
                hitbox.UpdatePolygon();
                nextvelocity.Y = 0;
            }
            hitbox.pos += nextvelocity;
            hitbox.vel = nextvelocity;
            hitbox.UpdatePolygon();
        }
    }
	public static void AddCollider(HitBox c) {
		hitboxes.Add(c);
	}
	public static void RemoveCollider(HitBox c) {
		hitboxes.Remove (c);
	}
	public static bool IsColliding (HitBox c) {
		if (!c.enabled || c.trigger)
			return false;
		foreach (HitBox collider in hitboxes) {
			if (!collider.enabled || !collider.entity.enabled || c == collider || collider.trigger)
				continue;
			bool touching = collider.IsOverlapping (c);
			if (touching)
				return true;
		}
		return false;
	}

	public static bool IsColliding (HitBox c, List<HitBox> colls) {
		if (!c.enabled || c.trigger)
			return false;
		foreach (HitBox collider in colls) {
			if (!collider.enabled || !collider.entity.enabled || c == collider || collider.trigger || Vector2.Distance(collider.pos, c.pos) > dist)
				continue;
            if (collider.IsOverlapping(c))
                return true;
		}
		return false;
	}

	public static HitBox GetCollision (HitBox c) {
		if (!c.enabled)
			return null;
		foreach (HitBox collider in hitboxes) {
			if (!collider.enabled || !collider.entity.enabled || c == collider || Vector2.Distance(collider.pos, c.pos) > dist)
				continue;
			if (collider.IsOverlapping (c)) {
				return collider;
			}
		}
		return null;
	}

    public static HitBox GetCollision<T>(HitBox c) {
        if (!c.enabled)
            return null;
        foreach (HitBox collider in hitboxes) {
            if (!collider.enabled || !collider.entity.enabled || c == collider || Vector2.Distance(collider.pos, c.pos) > dist)
                continue;
            if ((collider.entity is T) && collider.IsOverlapping(c)) {
                return collider;
            }
        }
        return null;
    }

    public static List<HitBox> GetCollisions (HitBox c) {
		if (!c.enabled)
			return null;
		List<HitBox> colList = new List<HitBox> ();
		for(int i = 0; i < hitboxes.Count; i++) {
            var collider = hitboxes[i];
			if (!collider.enabled || !collider.entity.enabled || c == collider || collider.trigger || Vector2.Distance(collider.pos, c.pos) > dist)
				continue;
			if (collider.IsOverlapping(c))
				colList.Add(collider);
		}
		return colList;
	}

    public static List<HitBox> GetCollisions<T>(HitBox c, bool triggers = false) {
        if (!c.enabled)
            return null;
        List<HitBox> colList = new List<HitBox>();
        foreach (HitBox collider in hitboxes) {
            if(triggers) {
                if (!collider.enabled || !collider.entity.enabled || c == collider || !collider.trigger || Vector2.Distance(collider.pos, c.pos) > dist)
                    continue;
            } else {
                if (!collider.enabled || !collider.entity.enabled || c == collider || collider.trigger || Vector2.Distance(collider.pos, c.pos) > dist)
                    continue;
            }
            
            if ((collider.entity is T) && collider.IsOverlapping(c))
                colList.Add(collider);
        }
        return colList;
    }

    public static List<HitBox> GetCollisions (HitBox c, string tag) {
		if (!c.enabled)
			return null;
		List<HitBox> colList = new List<HitBox> ();
		foreach (HitBox collider in hitboxes) {
			if (!collider.enabled || !collider.entity.enabled || c == collider || Vector2.Distance(collider.pos, c.pos) > dist)
				continue;
			if (collider.IsOverlapping (c)) {
				colList.Add(collider);
			}
		}
		return colList;
	}

	public static List<HitBox> GetCollisions (HitBox c, int layer) {
		if (!c.enabled)
			return null;
		List<HitBox> colList = new List<HitBox> ();
		foreach (HitBox collider in hitboxes) {
			if (collider.layer == layer && c != collider && collider.enabled && collider.IsOverlapping (c) && Vector2.Distance(collider.pos, c.pos) > dist && !collider.entity.enabled) {
				colList.Add(collider);
			}
		}
		return colList;
	}

	public static HitBox GetCollision (HitBox c, int layer) {
		if (!c.enabled)
			return null;
		foreach (HitBox collider in hitboxes) {
			if (collider.layer == layer && c != collider && collider.enabled && collider.IsOverlapping (c) && Vector2.Distance(collider.pos, c.pos) > dist && !collider.entity.enabled) {
				return collider;
			}
		}
		return null;
	}
    public static void ClearHitBoxes()
    {
        hitboxes = new List<HitBox>();
    }
}
