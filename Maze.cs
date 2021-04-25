using System.IO;
using System.Drawing;

namespace MrMunch
{
    /// <summary>
    /// Summary description for Maze.
    /// </summary>
    public class Maze
	{
		static int[] wall_pat = new int[] 
		{
			3,2,3,
			1,1,1,
			3,2,3, (int)BlockType.BT_WALLHORZ,

			3,1,3,
			2,1,2,
			3,1,3, (int)BlockType.BT_WALLVERT,

			2,2,2,
			2,1,1,
			2,1,2, (int)BlockType.BT_WALLTOPLEFT,

			2,2,2,
			1,1,2,
			2,1,2, (int)BlockType.BT_WALLTOPRIGHT,

			2,1,2,
			2,1,1,
			2,2,2, (int)BlockType.BT_WALLBOTLEFT,

			2,1,2,
			1,1,2,
			2,2,2, (int)BlockType.BT_WALLBOTRIGHT,

			2,1,2,
			1,1,2,
			2,1,2, (int)BlockType.BT_WALLTLEFT,

			2,1,2,
			2,1,1,
			2,1,2, (int)BlockType.BT_WALLTRIGHT,

			2,1,2,
			1,1,1,
			2,2,2, (int)BlockType.BT_WALLTUP,

			2,2,2,
			1,1,1,
			2,1,2, (int)BlockType.BT_WALLTDOWN,

			2,2,3,
			2,1,1,
			2,2,3, (int)BlockType.BT_WALLSTOPLEFT,

			3,2,2,
			1,1,2,
			3,2,2, (int)BlockType.BT_WALLSTOPRIGHT,

			2,2,2,
			2,1,2,
			3,1,3, (int)BlockType.BT_WALLSTOPUP,

			3,1,3,
			2,1,2,
			2,2,2, (int)BlockType.BT_WALLSTOPDOWN,

			2,2,2,
			2,1,2,
			2,2,2, (int)BlockType.BT_WALLALONE,

			-1
		};
	
		public Maze()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public static void DrawMazeTile(DDSurface ddsDest, int x, int y, int tileType)
		{
			int xOff = 0, yOff = 0;
			Rectangle destRect, srcRect;

			if(tileType >= (int)BlockType.BT_WALL)
			{
				tileType -= (int)BlockType.BT_WALL;
				yOff = 8 + (tileType / 8);
				xOff = tileType % 8;
			}
			else if(tileType == (int)BlockType.BT_DOT)
			{
				xOff = 0;
				yOff = 10;
			}
			else if(tileType == (int)BlockType.BT_PILL)
			{
				xOff = 1;
				yOff = 10;
			}
			else
			{
				tileType = -1;
			}

			destRect = new Rectangle(x*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, y*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
												Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			srcRect = new Rectangle(2*Defs.SPRITE_WIDTH, 10*Defs.SPRITE_HEIGHT, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);

			ddsDest.g.DrawImage(DDGraphics.surfSprite.b, destRect, srcRect, GraphicsUnit.Pixel);

            if (tileType != -1)
			{
				yOff *= Defs.SPRITE_HEIGHT;
				xOff *= Defs.SPRITE_WIDTH;
				destRect = new Rectangle(x*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, y*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
													Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
				srcRect = new Rectangle(xOff, yOff, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);

                ddsDest.g.DrawImage(DDGraphics.surfSprite.b, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

		public static void DrawMaze(DDSurface ddsDest, int[,] mp)
		{
			int x, y;

			for(y = 0; y < Defs.MAZE_HEIGHT; y++)
			{
				for(x = 0; x < Defs.MAZE_WIDTH; x++)
				{
					DrawMazeTile(ddsDest, x, y, mp[x, y]);
				}
			}
		}

		public static void RefreshMaze(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Rectangle rect = new Rectangle(Defs.GAME_OFFX, Defs.GAME_OFFY, Defs.GAME_WIDTH, Defs.GAME_HEIGHT);
			DDGraphics.UpdateSurface(ddsDest, ddsSrc, rect);
		}

		public static void InitMap()
		{
			int x, y;

			GameData.dots = 0;
			for(y = 0; y < Defs.MAZE_HEIGHT; y++)
			{
				for(x = 0; x < Defs.MAZE_WIDTH; x++)
				{
					GameData.map[x, y] = GameData.permMap[x, y];
					if((GameData.map[x, y] == (int)BlockType.BT_DOT) || (GameData.map[x, y] == (int)BlockType.BT_PILL))
						GameData.dots++;
				}
			}
		}

		public static void LoadMap(string fname, int[,] mp)
		{
			int x, y, i;
			char m;
			int[,] tm = new int[Defs.MAZE_WIDTH+2, Defs.MAZE_HEIGHT+2];

            string s = (string)MrMunch.Properties.Resources.ResourceManager.GetObject(fname);
            StringReader sr = new StringReader(s);

			for(y = 0; y < (Defs.MAZE_HEIGHT+2); y++)
			{
				tm[0, y] = tm[Defs.MAZE_WIDTH+1, y] = 2;
			}

			for(x = 0; x < (Defs.MAZE_WIDTH+2); x++)
			{
				tm[x, 0] = tm[x, Defs.MAZE_HEIGHT+1] = 2;
			}

			for(y = 0; y < Defs.MAZE_HEIGHT; y++)
			{
				s = sr.ReadLine();
				for(x = 0; x < Defs.MAZE_WIDTH; x++)
				{
					m = s[x];
					if(m == '1')				// Wall
					{
						m = (char)1;
					}
					else
					{
						if(m == '.')			// Normal dot
							m = (char)(2+4);
						else if(m == 'o')		// Power dot
							m = (char)(2+8);
						else
							m = (char)2;
					}
					tm[x+1, y+1] = (int)m;
				}
			}
			sr.Close();

			for(y = 1; y < (Defs.MAZE_HEIGHT+1); y++)
			{
				for(x = 1; x < (Defs.MAZE_WIDTH+1); x++)
				{
					if(tm[x, y] == 1)			// If wall
					{
						for(i = 0; wall_pat[i] != -1; i += 10)
						{
							if(
							((wall_pat[i]   & tm[x-1, y-1]) != 0) &&
							((wall_pat[i+1] & tm[x  , y-1]) != 0) &&
							((wall_pat[i+2] & tm[x+1, y-1]) != 0) &&
							((wall_pat[i+3] & tm[x-1, y  ]) != 0) &&
							((wall_pat[i+4] & tm[x  , y  ]) != 0) &&
							((wall_pat[i+5] & tm[x+1, y  ]) != 0) &&
							((wall_pat[i+6] & tm[x-1, y+1]) != 0) &&
							((wall_pat[i+7] & tm[x  , y+1]) != 0) &&
							((wall_pat[i+8] & tm[x+1, y+1]) != 0))
							{
								mp[x-1, y-1] = wall_pat[i+9];
								break;
							}
						}
						if(wall_pat[i] == -1)
						{
							mp[x-1, y-1] = (int)BlockType.BT_VOID;
						}
						if((x == (Defs.GHOST_HOMEX+1)) && (y == Defs.GHOST_HOMEY))
						{
							mp[x-1, y-1] = (int)BlockType.BT_WALLDOOR;
						}
					}
					else
					{
						if((tm[x, y] & 4) != 0)		// If dot
						{
							mp[x-1, y-1] = (int)BlockType.BT_DOT;
						}
						else if((tm[x, y] & 8) != 0)	// If pill
						{
							mp[x-1, y-1] = (int)BlockType.BT_PILL;
						}
						else					// Otherwise blank
							mp[x-1, y-1] = (int)BlockType.BT_VOID;
						
						// Now work out any hyper-jump positions
						if(x == 1)
							GameData.left_hyper = y-1;
						else if(x == Defs.MAZE_WIDTH)
							GameData.right_hyper = y-1;
						else if(y == 1)
							GameData.top_hyper = x-1;
						else if(y == Defs.MAZE_HEIGHT)
							GameData.bottom_hyper = x-1;
					}
				}
			}
		}

		/// <summary>
		// Name			:	moveAllowed											//
		// Parameters	:	x and y coordinates, direction to move in.			//
		// Return value	:	True if move is allowed, False otherwise.			//
		// Description	:														//
		//	Determines whether a move in the input direction is allowed based	//
		//	on whether the move would result in being obstructed or not.  Also	//
		//	a change in direction is only allowed when the coordinates are		//
		//	exactly aligned with the game block size.							//
		/// </summary>
		public static bool MoveAllowed(int x, int y, int dir)
		{
			int m;

			x -= Defs.GAME_OFFX;
			y -= Defs.GAME_OFFY;
			if(dir == (int)DirType.DIR_LEFT)
			{
				x -= Defs.STEP_SIZE;
				if((y % Defs.SPRITE_HEIGHT) != 0)
					return(false);
			}
			else if(dir == (int)DirType.DIR_RIGHT)
			{
				x += Defs.SPRITE_WIDTH + Defs.STEP_SIZE - 1;
				if((y % Defs.SPRITE_HEIGHT) != 0)
					return(false);
			}
			else if(dir == (int)DirType.DIR_UP)
			{
				y -= Defs.STEP_SIZE;
				if((x % Defs.SPRITE_WIDTH) != 0)
					return(false);
			}
			else if(dir == (int)DirType.DIR_DOWN)
			{
				y += Defs.SPRITE_HEIGHT + Defs.STEP_SIZE - 1;
				if((x % Defs.SPRITE_WIDTH) != 0)
					return(false);
			}
			m = GameData.map[x/Defs.SPRITE_WIDTH, y/Defs.SPRITE_HEIGHT];
			if(m < (int)BlockType.BT_WALL)
				return(true);
			else
				return(false);
		}

	}
}
