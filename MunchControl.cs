using System.Drawing;

namespace MrMunch
{
    /// <summary>
    /// Summary description for MunchControl.
    /// </summary>
    public class MunchControl
	{
		public static int nextDir = -1;
		public static Sprite munchSprite = new Sprite();

		public MunchControl()
		{
			//
			// TODO: Add constructor logic here
			//

		}

		public static int CheckCollision()
		{
			int i, i1, i2, j1, j2, x1, x2, y1, y2;

			i1 = munchSprite.x - Defs.HIT_SIZE;
			i2 = munchSprite.x + Defs.SPRITE_WIDTH + Defs.HIT_SIZE;
			j1 = munchSprite.y - Defs.HIT_SIZE;
			j2 = munchSprite.y + Defs.SPRITE_HEIGHT + Defs.HIT_SIZE;

			for(i = 0; i < Defs.NUM_GHOSTS; i++)
			{
				if((GhostControl.ghostSprite[i].s == (int)StatType.ST_NORMAL) ||
					(GhostControl.ghostSprite[i].s == (int)StatType.ST_FLEE))
				{
					x1 = GhostControl.ghostSprite[i].x - Defs.SPRITE_WIDTH;
					x2 = GhostControl.ghostSprite[i].x + Defs.SPRITE_WIDTH*2;
					y1 = GhostControl.ghostSprite[i].y - Defs.SPRITE_HEIGHT;
					y2 = GhostControl.ghostSprite[i].y + Defs.SPRITE_HEIGHT*2;
					if((i1 >= x1) && (i2 <= x2) && (j1 >= y1) && (j2 <= y2))
						return(i);
				}
			}
			return(-1);
		}

		public static void DoEatDot()
		{
			DDSound.Stop(DDSound.dsbEatDot);
			DDSound.PlayOnce(DDSound.dsbEatDot);

			GameData.score += Defs.DOT_SCORE;
			GamePanel.ShowScore(GameData.score);
		}

		public static void DoEatPill()
		{
			int i;
			int extra;

			DDSound.StopAllFxSounds();
			DDSound.PlayOnce(DDSound.dsbEatPowerPill);

			for(i = 0; i < Defs.NUM_GHOSTS; i++)
			{
				if(GhostControl.ghostSprite[i].s == (int)StatType.ST_NORMAL)
				{
					GhostControl.ghostSprite[i].s = (int)StatType.ST_FLEE;
					GhostControl.DoBestGhostMoveForced(i,
						munchSprite.x - GhostControl.ghostSprite[i].x,
						munchSprite.y - GhostControl.ghostSprite[i].y );
					GhostControl.SetGhostFrame(i);
				}
			}
			extra = 10000-(GameData.level-1)*1000;
			if(extra < 1000)
				extra = 1000;
			GameData.eatTime.counter += extra;
			GameData.score += 10;
			GamePanel.ShowScore(GameData.score);
		}

		public static void CheckMunch()
		{
			int x, y, d, m, c, olds;
			Rectangle srcRect, destRect;

			olds = GameData.score;
			x = munchSprite.x + (Defs.SPRITE_WIDTH/2) - Defs.GAME_OFFX;
			y = munchSprite.y + (Defs.SPRITE_HEIGHT/2) - Defs.GAME_OFFY;
			d = munchSprite.d;
			x /= Defs.SPRITE_WIDTH;
			y /= Defs.SPRITE_HEIGHT;
			m = GameData.map[x, y];
			if((m == (int)BlockType.BT_DOT) || (m == (int)BlockType.BT_PILL))
			{
				// Erase dot or pill from screen
				srcRect = new Rectangle(2*Defs.SPRITE_WIDTH, 10*Defs.SPRITE_HEIGHT, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);

				destRect = new Rectangle(x*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, y*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
											Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
				DDGraphics.surfStatic.g.DrawImage(DDGraphics.surfSprite.b, destRect, srcRect, GraphicsUnit.Pixel);
				DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, destRect);
				DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);

				// Do pill or dot processing as appropriate..
				if(m == (int)BlockType.BT_PILL)
				{
					DoEatPill();
				}
				else
				{
					DoEatDot();
				}

				// Remove pill or dot from map and decrement number of dots or pills left to eat
				GameData.map[x, y] = (int)BlockType.BT_VOID;
				GameData.dots--;

				// Check if level is complete..
				if(GameData.dots == 0)
				{
					GameControl.DoLevelComplete();
					return;
				}
			}

			// Check if eaten a fruit..
			if((x == Defs.FRUIT_HOMEX) && (y == Defs.FRUIT_HOMEY) && (GameData.currFruit >= 0))
			{
				if(GameData.fruitGot[GameData.currFruit] < 3)
				{
					Fruit.ActionFruitGot(GameData.currFruit);
				}
				GameData.currFruit = -1;
			}


			if(GameData.eatTime.counter != 0)
			{
				if(GameData.eatTime.counter < 4000)
				{
					DDSound.PlayOnce(DDSound.dsbPowerMunch);
				}
			}
			else
			{
				GameData.totEaten = 0;
				DDSound.Stop(DDSound.dsbPowerMunch);
			}

			// Check collisions with ghosts..
			c = CheckCollision();
			if(c != -1)
			{
				if(GhostControl.ghostSprite[c].s == (int)StatType.ST_FLEE)
				{
					GhostControl.DoEatGhost(c, 1);
				}
				else
				{
					if(GhostControl.ghostSprite[c].s != (int)StatType.ST_FROZEN2) // If ghost is not frozen + transparent.
					{
						GameControl.PlayDead();
					}
					return;
				}
			}
		}

		public static void ProcessMunch()
		{
			int x, y;

			x = munchSprite.x;
			y = munchSprite.y;

			if((nextDir != -1) && Maze.MoveAllowed(x, y, nextDir))
				munchSprite.d = nextDir;

			if(Maze.MoveAllowed(x, y, munchSprite.d))
				GameControl.UpdateMove(ref x, ref y, munchSprite.d, Defs.STEP_SIZE);

			munchSprite.SetPos(x, y);
			SetMunchFrame();

			CheckMunch();
		}

		public static void SetMunchFrame()
		{
			int yOff, xOff, x, y, d;

			yOff = xOff = 0;
			d = munchSprite.d;
			x = munchSprite.x - Defs.GAME_OFFX;
			y = munchSprite.y - Defs.GAME_OFFY;

			yOff = d;
			if(d == (int)DirType.DIR_RIGHT)
			{
				xOff = (x / 2) % 8;
				if(xOff > 3)
					xOff = 7 - xOff;
			}
			else if(d == (int)DirType.DIR_LEFT)
			{
				xOff = (x / 2) % 8;
				xOff = 7 - xOff;
				if(xOff > 3)
					xOff = 7 - xOff;
			}
			else if(d == (int)DirType.DIR_DOWN)
			{
				xOff = (y / 2) % 8;
				if(xOff > 3)
					xOff = 7 - xOff;
			}
			else if(d == (int)DirType.DIR_UP)
			{
				xOff = (y / 2) % 8;
				xOff = 7 - xOff;
				if(xOff > 3)
					xOff = 7 - xOff;
				xOff = 3 - xOff;
			}
			yOff *= Defs.SPRITE_HEIGHT;
			xOff *= Defs.SPRITE_WIDTH;
			munchSprite.SetFrame(xOff, yOff);
		}

	}
}
