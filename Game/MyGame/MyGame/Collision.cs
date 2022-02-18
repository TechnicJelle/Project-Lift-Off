using GXPEngine;
using GXPEngine.Core;
using MyGame.MyGame.Entities;

namespace MyGame.MyGame;

public static class Collision
{
	private static readonly (bool, Vector2, Vector2, float) NoCollision = (false, null, null, 0);

	private static (bool isColliding, Vector2 contactPoint, Vector2 contactNormal, float tHitNear) RayVsRect(Vector2 rayOrigin, Vector2 rayDir, Solid target)
	{
		//additional returns
		Vector2 contactNormal = new(-1, -1);
		Vector2 tNear = new(
			(target.x - rayOrigin.x) / rayDir.x,
			(target.y - rayOrigin.y) / rayDir.y);
		Vector2 tFar = new(
			(target.x + target.width - rayOrigin.x) / rayDir.x,
			(target.y + target.height - rayOrigin.y) / rayDir.y);

		if (float.IsNaN(tFar.y) || float.IsNaN(tFar.x)) return NoCollision;
		if (float.IsNaN(tNear.y) || float.IsNaN(tNear.x)) return NoCollision;

		//maybe swap the whole vector and not only one coord? test that if this doesn't work!!
		if (tNear.x > tFar.x) (tNear.x, tFar.x) = (tFar.x, tNear.x);
		if (tNear.y > tFar.y) (tNear.y, tFar.y) = (tFar.y, tNear.y);

		if (tNear.x > tFar.y || tNear.y > tFar.x) return NoCollision;

		float tHitNear = Mathf.Max(tNear.x, tNear.y);
		float tHitFar = Mathf.Min(tFar.x, tFar.y);

		if (tHitFar < 0.0f) return NoCollision;

		Vector2 contactPoint = Vector2.Add(rayOrigin, Vector2.Mult(rayDir, tHitNear));

		if (tNear.x > tNear.y)
			contactNormal = rayDir.x < 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
		else if (tNear.x < tNear.y)
			contactNormal = rayDir.y < 0 ? new Vector2(0, 1) : new Vector2(0, -1);


		return (true, contactPoint, contactNormal, tHitNear);
	}

	public static (bool isColliding, Vector2 contactPoint, Vector2 contactNormal, float tHitNear) DynamicRectVsRect(Entity inp, Solid target)
	{
		//returns PVector contact_point, PVector contact_normal, float fTime
		if (inp._vel.x == 0 && inp._vel.y == 0)
			return NoCollision;

		Solid extendedTarget = new(Vector2.Sub(target.GetPos(), Vector2.Div(inp.GetSize(), 2)), Vector2.Add(target.GetSize(), inp.GetSize()), false);
		(bool isColliding, Vector2 contactPoint, Vector2 contactNormal, float tHitNear) = RayVsRect(Vector2.Add(inp.GetPos(), Vector2.Div(inp.GetSize(), 2)), inp._vel, extendedTarget);
		if (!isColliding || tHitNear > 1.0f) return NoCollision;

		return (true, contactPoint, contactNormal, tHitNear);
	}
}
