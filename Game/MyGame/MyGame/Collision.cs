using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Entities;

namespace MyGame.MyGame;

public class Collision
{
	public readonly bool Result;
	public readonly Vector2 ContactPoint;
	public readonly Vector2 ContactNormal;
	public readonly float THitNear;

	private Collision(bool r, Vector2 cp, Vector2 cn, float thn)
	{
		Result = r;
		ContactPoint = cp;
		ContactNormal = cn;
		THitNear = thn;
	}

	private Collision(bool r)
	{
		Result = r;
	}

	private static Collision RayVsRect(Vector2 rayOrigin, Vector2 rayDir, Solid target)
	{
		//additional returns
		Vector2 contactNormal = new(-1, -1);
		Vector2 tNear = new(
			(target.x - rayOrigin.x) / rayDir.x,
			(target.y - rayOrigin.y) / rayDir.y);
		Vector2 tFar = new(
			(target.x + target.width - rayOrigin.x) / rayDir.x,
			(target.y + target.height - rayOrigin.y) / rayDir.y); //TODO: make sure these are correct, cause i think this code expects the origin to be at the top left and gxp does origins in the middle

		if (float.IsNaN(tFar.y) || float.IsNaN(tFar.x)) return new Collision(false);
		if (float.IsNaN(tNear.y) || float.IsNaN(tNear.x)) return new Collision(false);

		//maybe swap the whole vector and not only one coord? test that if this doesn't work!!
		if (tNear.x > tFar.x) (tNear.x, tFar.x) = (tFar.x, tNear.x);
		if (tNear.y > tFar.y) (tNear.y, tFar.y) = (tFar.y, tNear.y);

		if (tNear.x > tFar.y || tNear.y > tFar.x) return new Collision(false);

		float tHitNear = Mathf.Max(tNear.x, tNear.y);
		float tHitFar = Mathf.Min(tFar.x, tFar.y);

		if (tHitFar < 0.0f) return new Collision(false);

		Vector2 contactPoint = Vector2.Add(rayOrigin, Vector2.Mult(rayDir, tHitNear));

		if (tNear.x > tNear.y)
			contactNormal = rayDir.x < 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
		else if (tNear.x < tNear.y)
			contactNormal = rayDir.y < 0 ? new Vector2(0, 1) : new Vector2(0, -1);


		return new Collision(true, contactPoint, contactNormal, tHitNear);
	}

	public static Collision DynamicRectVsRect(Entity inp, Solid target)
	{
		//returns PVector contact_point, PVector contact_normal, float fTime
		bool ret = false;
		if (inp._vel.x == 0 && inp._vel.y == 0)
			return new Collision(false);

		Solid extendedTarget = new(Vector2.Sub(target.GetPos(), Vector2.Div(inp.GetSize(), 2)), Vector2.Add(target.GetSize(), inp.GetSize()), false);
		Collision c = RayVsRect(Vector2.Add(inp.GetPos(), Vector2.Div(inp.GetSize(), 2)), inp._vel, extendedTarget);
		if (c.Result)
		{
			if (c.THitNear <= 1.0f)
				ret = true;
			else
				return new Collision(false);
		}

		return new Collision(ret, c.ContactPoint, c.ContactNormal, c.THitNear);
	}
}
