namespace MrMunch
{
    /// <summary>
    /// Summary description for GhostControl.
    /// </summary>
    public class GhostControl
	{
		public static Sprite[] ghostSprite = new Sprite[Defs.NUM_GHOSTS];

		public GhostControl()
		{
			//
			// TODO: Add constructor logic here
			//
			for(int i = 0; i < Defs.NUM_GHOSTS; i++)
			{
				ghostSprite[i] = new Sprite();
			}
		}

		public static void SetGhostFrame(int i)
		{
			int xOff, yOff;

			xOff = 0;
			yOff = 4*Defs.SPRITE_HEIGHT + (i%4)*Defs.SPRITE_HEIGHT;
			switch(ghostSprite[i].s)
			{
				case 0:		// Sprite not active
					return;
					//break;

				case (int)StatType.ST_GOHOME:
					xOff = 8*Defs.SPRITE_WIDTH + Defs.SPRITE_WIDTH*ghostSprite[i].d;
					break;

				case (int)StatType.ST_FLEE:
					// In the last 2 secs of ghost fleeing, flash the ghost
					if(GameData.eatTime.counter < 2000)
					{
						xOff = ((GameData.eatTime.counter & 0x80) / 0x80)*4*Defs.SPRITE_WIDTH + Defs.SPRITE_WIDTH*ghostSprite[i].d;
					}
					else // Normal flee - not flashing
					{
						xOff = 4*Defs.SPRITE_WIDTH + Defs.SPRITE_WIDTH*ghostSprite[i].d;
					}
					break;

				case (int)StatType.ST_NORMAL:
					xOff = Defs.SPRITE_WIDTH*ghostSprite[i].d;
					break;

				case (int)StatType.ST_FROZEN:					// Frozen sprite.
					yOff = Defs.SPRITE_HEIGHT * 17;
					xOff = Defs.SPRITE_WIDTH * 0;
					break;

				case (int)StatType.ST_FROZEN2:				// Frozen and transparent.
					yOff = Defs.SPRITE_HEIGHT * 17;
					xOff = Defs.SPRITE_WIDTH * 3;
					break;
			}
			ghostSprite[i].SetFrame(xOff, yOff);
		}

		public static void DoBestGhostMove(int i, int dx, int dy, int prob)
		{
			int x, y;
			int adx, ady;
			DirType dir1, dir2, dir3, dir4, d, t;
			DirType towardsTargetX, retreatTargetX;
			DirType towardsTargetY, retreatTargetY;

			dir1 = dir2 = dir3 = dir4 = DirType.DIR_NUMTYPES;

			x = ghostSprite[i].x;
			y = ghostSprite[i].y;
			if((((x-Defs.GAME_OFFX) % Defs.SPRITE_WIDTH) != 0) ||
				(((y-Defs.GAME_OFFY) % Defs.SPRITE_HEIGHT)!= 0))
				return;
			d = (DirType)ghostSprite[i].d;
			adx = dx;
			ady = dy;
			if(adx < 0)
				adx = -adx;
			if(ady < 0)
				ady = -ady;

			if(dx >= 0)
			{
				towardsTargetX = DirType.DIR_LEFT;
				retreatTargetX = DirType.DIR_RIGHT;
			}
			else
			{
				towardsTargetX = DirType.DIR_RIGHT;
				retreatTargetX = DirType.DIR_LEFT;
			}

			if(dy >= 0)
			{
				towardsTargetY = DirType.DIR_UP;
				retreatTargetY = DirType.DIR_DOWN;
			}
			else
			{
				towardsTargetY = DirType.DIR_DOWN;
				retreatTargetY = DirType.DIR_UP;
			}

			if(d == DirType.DIR_UP)
			{
				if((dy >= 0) && (ady > adx))
				{
					dir1 = DirType.DIR_UP;
					dir2 = towardsTargetX;
					dir3 = retreatTargetX;
					dir4 = DirType.DIR_DOWN;
				}
				else
				{
					dir1 = towardsTargetX;
					if((dy >=0) || (ady <= adx))
					{
						dir2 = DirType.DIR_UP;
						dir3 = retreatTargetX;
					}
					else
					{
						dir2 = retreatTargetX;
						dir3 = DirType.DIR_UP;
					}
					dir4 = DirType.DIR_DOWN;
				}
			}
			else if(d == DirType.DIR_DOWN)
			{
				if((dy <= 0) && (ady > adx))
				{
					dir1 = DirType.DIR_DOWN;
					dir2 = towardsTargetX;
					dir3 = retreatTargetX;
					dir4 = DirType.DIR_UP;
				}
				else
				{
					dir1 = towardsTargetX;
					if((dy <=0) || (ady <= adx))
					{
						dir2 = DirType.DIR_DOWN;
						dir3 = retreatTargetX;
					}
					else
					{
						dir2 = retreatTargetX;
						dir3 = DirType.DIR_DOWN;
					}
					dir4 = DirType.DIR_UP;
				}
			}
			else if(d == DirType.DIR_LEFT)
			{
				if((dx >= 0) && (adx >= ady))
				{
					dir1 = DirType.DIR_LEFT;
					dir2 = towardsTargetY;
					dir3 = retreatTargetY;
					dir4 = DirType.DIR_RIGHT;
				}
				else
				{
					dir1 = towardsTargetY;
					if((dx >= 0) || (adx <= ady))
					{
						dir2 = DirType.DIR_LEFT;
						dir3 = retreatTargetY;
					}
					else
					{
						dir2 = retreatTargetY;
						dir3 = DirType.DIR_LEFT;
					}
					dir4 = DirType.DIR_RIGHT;
				}
			}
			else if(d == DirType.DIR_RIGHT)
			{
				if((dx <= 0) && (adx > ady))
				{
					dir1 = DirType.DIR_RIGHT;
					dir2 = towardsTargetY;
					dir3 = retreatTargetY;
					dir4 = DirType.DIR_LEFT;
				}
				else
				{
					dir1 = towardsTargetY;
					if ((dx <= 0) || (adx <= ady))
					{
						dir2 = DirType.DIR_RIGHT;
						dir3 = retreatTargetY;
					}
					else
					{
						dir2 = retreatTargetY;
						dir3 = DirType.DIR_RIGHT;
					}
					dir4 = DirType.DIR_LEFT;
				}
			}

			// On prob rotate best move
			if(GameControl.munchRandom.PcCheck(prob))
			{
				t = dir1;
				dir1 = dir2;
				dir2 = t;
			}
			// On prob rotate 2nd best move
			if(GameControl.munchRandom.PcCheck(prob))
			{
				t = dir2;
				dir2 = dir3;
				dir3 = t;
			}
			if(Maze.MoveAllowed(x, y, (int)dir1))
				d = dir1;
			else if(Maze.MoveAllowed(x, y, (int)dir2))
				d = dir2;
			else if(Maze.MoveAllowed(x, y, (int)dir3))
				d = dir3;
			else if(Maze.MoveAllowed(x, y, (int)dir4))
				d = dir4;
			ghostSprite[i].d = (int)d;
		}


		public static void DoBestGhostMoveForced(int i, int dx, int dy)
		{
			DirType dir1, dir2, dir3, dir4, d;
			int x, y;
			int adx, ady;

			x = ghostSprite[i].x;
			y = ghostSprite[i].y;
			d = (DirType)ghostSprite[i].d;
			adx = dx;
			ady = dy;
			if(adx < 0)
				adx = -adx;
			if(ady < 0)
				ady = -ady;

			if(ady > adx)
			{
				if(dy >= 0)
				{
					dir1 = DirType.DIR_UP;
					dir4 = DirType.DIR_DOWN;
				}
				else
				{
					dir1 = DirType.DIR_DOWN;
					dir4 = DirType.DIR_UP;
				}
				if(dx >= 0)
				{
					dir2 = DirType.DIR_LEFT;
					dir3 = DirType.DIR_RIGHT;
				}
				else
				{
					dir2 = DirType.DIR_RIGHT;
					dir3 = DirType.DIR_LEFT;
				}
			}
			else
			{
				if(dx >= 0)
				{
					dir1 = DirType.DIR_LEFT;
					dir4 = DirType.DIR_RIGHT;
				}
				else
				{
					dir1 = DirType.DIR_RIGHT;
					dir4 = DirType.DIR_LEFT;
				}
				if(dy >= 0)
				{
					dir2 = DirType.DIR_UP;
					dir3 = DirType.DIR_DOWN;
				}
				else
				{
					dir2 = DirType.DIR_DOWN;
					dir3 = DirType.DIR_UP;
				}
			}
			if(Maze.MoveAllowed(x, y, (int)dir1))
				d = dir1;
			else if(Maze.MoveAllowed(x, y, (int)dir2))
				d = dir2;
			else if(Maze.MoveAllowed(x, y, (int)dir3))
				d = dir3;
			else if(Maze.MoveAllowed(x, y, (int)dir4))
				d = dir4;
			ghostSprite[i].d = (int)d;
		}


		void doGhostGoHome2(int i)
		{
			DirType d, d1, d2, d3, t;
			int dx, dy;
			int x, y;

			x = ghostSprite[i].x;
			y = ghostSprite[i].y;
			if(((x-Defs.GAME_OFFX) == (Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH)) &&
				((y-Defs.GAME_OFFY) >= (Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT-Defs.SPRITE_HEIGHT*2)) &&
				((y-Defs.GAME_OFFY) < (Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT)))
			{
				ghostSprite[i].d = (int)DirType.DIR_DOWN;
				return;
			}
			if((((x-Defs.GAME_OFFX)%Defs.SPRITE_WIDTH) != 0) ||
				(((y-Defs.GAME_OFFY)%Defs.SPRITE_HEIGHT) != 0))
				return;
			d = (DirType)ghostSprite[i].d;
			dx = x - Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH;
			dy = y - Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT;
			if((d == DirType.DIR_LEFT) || (d == DirType.DIR_RIGHT))
			{
				if(dy >= 0)
				{
					d1 = DirType.DIR_UP;
					d2 = DirType.DIR_DOWN;
				}
				else
				{
					d1 = DirType.DIR_DOWN;
					d2 = DirType.DIR_UP;
				}
				if(d == DirType.DIR_LEFT)
				{
					d3 = DirType.DIR_RIGHT;
				}
				else
				{
					d3 = DirType.DIR_LEFT;
				}
				if(GameControl.munchRandom.PcCheck(500))
				{
					t = d;
					d = d1;
					d1 = t;
				}
			}
			else
			{
				if(dx >= 0)
				{
					d1 = DirType.DIR_LEFT;
					d2 = DirType.DIR_RIGHT;
				}
				else
				{
					d1 = DirType.DIR_RIGHT;
					d2 = DirType.DIR_LEFT;
				}
				if(d == DirType.DIR_UP)
				{
					d3 = DirType.DIR_DOWN;
				}
				else
				{
					d3 = DirType.DIR_UP;
				}
				if(GameControl.munchRandom.PcCheck(500))
				{
					t = d;
					d = d1;
					d1 = t;
				}
			}
			if(Maze.MoveAllowed(x, y, (int)d1))
				ghostSprite[i].d = (int)d1;
			else if(Maze.MoveAllowed(x, y, (int)d))
				ghostSprite[i].d = (int)d;
			else if(Maze.MoveAllowed(x, y, (int)d2))
				ghostSprite[i].d = (int)d2;
			else if(Maze.MoveAllowed(x, y, (int)d3))
				ghostSprite[i].d = (int)d3;
		}

		public static void DoGhostGoHome(int i)
		{
			int dx, dy;
			int x, y;

			x = ghostSprite[i].x;
			y = ghostSprite[i].y;

			if(((x-Defs.GAME_OFFX) == (Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH)) &&
				((y-Defs.GAME_OFFY) >= (Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT-Defs.SPRITE_HEIGHT*2)) &&
				((y-Defs.GAME_OFFY) < (Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT)))
			{
				ghostSprite[i].d = (int)DirType.DIR_DOWN;
				return;
			}

			if((((x-Defs.GAME_OFFX)%Defs.SPRITE_WIDTH) != 0) ||
				(((y-Defs.GAME_OFFY)%Defs.SPRITE_HEIGHT) != 0))
				return;

			dx = x - Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH;
			dy = y - Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT;

			DoBestGhostMove(i, dx, dy, 0);
		}

		public static void DoGhostFlee(int i)
		{
			int dx, dy;

			dx = ghostSprite[i].x - MunchControl.munchSprite.x;
			dy = ghostSprite[i].y - MunchControl.munchSprite.y;
			DoBestGhostMove(i, -dx, -dy, i*100 + 50);
		}


		public static void DoGhostChase(int i)
		{
			int dx, dy;

			if(((ghostSprite[i].x-Defs.GAME_OFFX)==(Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH)) &&
				((ghostSprite[i].y-Defs.GAME_OFFY) >(Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT-Defs.SPRITE_HEIGHT*2)) &&
				((ghostSprite[i].y-Defs.GAME_OFFY)<=(Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT)))
			{
				ghostSprite[i].d = (int)DirType.DIR_UP;
				return;
			}
			dx = ghostSprite[i].x - MunchControl.munchSprite.x;
			dy = ghostSprite[i].y - MunchControl.munchSprite.y;
			DoBestGhostMove(i, dx, dy, i*100 + 50);
		}


		public static void DoGhostMove(int i)
		{
			switch(ghostSprite[i].s)
			{
				case (int)StatType.ST_NORMAL:
					DoGhostChase(i);
					break;

				case (int)StatType.ST_FLEE:
					DoGhostFlee(i);
					if(GameData.eatTime.counter <= 0)
					{
						ghostSprite[i].s = (int)StatType.ST_NORMAL;
					}
					break;
	
				case (int)StatType.ST_GOHOME:
					DoGhostGoHome(i);
					if(((ghostSprite[i].x-Defs.GAME_OFFX)==(Defs.GHOST_HOMEX*Defs.SPRITE_WIDTH)) &&
						((ghostSprite[i].y-Defs.GAME_OFFY)==(Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT)))
					{
						ghostSprite[i].s = (int)StatType.ST_NORMAL;
						ghostSprite[i].d = (int)(GameControl.munchRandom.PcCheck(500) ? DirType.DIR_LEFT : DirType.DIR_RIGHT);
					}
					break;

				default:
					break;
			}
		}


		public static void DoEatGhost(int c, int s)
		{
			int extra;

			ghostSprite[c].s = (int)StatType.ST_GOHOME;
			DoGhostMove(c);
			SetGhostFrame(c);
			if(s != 0)
			{
				DDSound.PlayOnce(DDSound.dsbEatGhost);
			}
			GameData.totEaten++;
			if(GameData.totEaten > Defs.NUM_GHOSTS)
			{
				GameData.totEaten = Defs.NUM_GHOSTS;
			}
			extra = (GameData.totEaten + (GameData.level - 1));
			if(extra > 10)
				extra = 10;
			GameData.score += extra*100;
			GamePanel.ShowScore(GameData.score);
			ScoreSprite.StartScoreSprite(ghostSprite[c].x, ghostSprite[c].y, extra-1);
		}


		public static void ProcessGhosts(int s)
		{
			int i;

			for(i = 0; i < Defs.NUM_GHOSTS; i++)
			{
				if(ghostSprite[i].s == s)
				{
					DoGhostMove(i);
					SetGhostFrame(i);
					GameControl.UpdateMove(ref ghostSprite[i].x, ref ghostSprite[i].y, ghostSprite[i].d, Defs.STEP_SIZE);
				}
			}
		}

	}
}
