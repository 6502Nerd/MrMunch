using System.Drawing;

namespace MrMunch
{
    /// <summary>
    /// Summary description for Fruit.
    /// </summary>
    public class Fruit
	{
		static PcTableEntry[] fruitPcTable = new PcTableEntry[]
		{
			new PcTableEntry((int)FruitType.FT_CHERRY,	200),
			new PcTableEntry((int)FruitType.FT_GRAPE,	150),
			new PcTableEntry((int)FruitType.FT_MELON,	150),
			new PcTableEntry((int)FruitType.FT_BELL,	150),
			new PcTableEntry((int)FruitType.FT_BAR1,	150),
			new PcTableEntry((int)FruitType.FT_BAR2,	100),
			new PcTableEntry((int)FruitType.FT_BAR3,	100),
			new PcTableEntry(-1,						1000)
		};

		public static Sprite fruitSprite = new Sprite();

		public Fruit()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void DrawFruitBonus(DDSurface ddsDest, int fruit, int got)
		{
			Rectangle srcRect = new Rectangle(fruit*5*Defs.SPRITE_WIDTH, (18+got)*Defs.SPRITE_HEIGHT,
				5*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			Rectangle destRect = new Rectangle(Defs.BONUS_WND_X, fruit*Defs.SPRITE_HEIGHT + Defs.BONUS_WND_Y,
				5*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			ddsDest.g.DrawImage(DDGraphics.surfStaticSprite.b, destRect, srcRect, GraphicsUnit.Pixel);
        }


        public static void DrawAllFruitBonus(DDSurface ddsDest, int[] f)
		{
			int y;

			for(y = 0; y < (int)FruitType.FT_NUMTYPES; y++)
			{
				DrawFruitBonus(ddsDest, y, f[y]);
			}
		}


		public static void ProcessFruitBonus()
		{
			// Only continue with this if there is no fruit on screen and
			// there is not a time block in force.
			if ((GameData.currFruit < 0) && GameData.noFruitTime.expired)
			{
				// Only a 10% chance of even trying to choose a fruit.
				if(GameControl.munchRandom.PcCheck(100))
				{
					// Choose nothing, or a fruit which is not already in bonus mode.
					do
					{
						GameData.currFruit = GameControl.munchRandom.PcTable(fruitPcTable);
					}while ((GameData.currFruit != -1) && (GameData.fruitGot[GameData.currFruit] == 3));

					// If actually chose a fruit, then action it.
					if (GameData.currFruit != -1)
					{
						fruitSprite.Init(Defs.FRUIT_HOMEX*Defs.SPRITE_WIDTH + Defs.GAME_OFFX,
							Defs.FRUIT_HOMEY*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
							(int)StatType.ST_NORMAL, 100, DDGraphics.surfSprite, Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
						fruitSprite.SetFrame(GameData.currFruit*Defs.SPRITE_WIDTH, 15*Defs.SPRITE_HEIGHT);
						fruitSprite.AddAtBottom();

						// Calculate how long the fruit stays on the screen.
						GameData.currFruitTime.counter = System.Math.Max(5000, 15000-(1000*GameData.level));
					}
				}
			}
			else	// There must be a time block or a fruit on screen
			{
				// If the fruit on screen time has reached zero then...
				if (GameData.currFruitTime.expired && (GameData.currFruit != -1))
				{
					// Take the fruit off screen and set the on screen time to -1
					GameData.currFruit = -1;
					fruitSprite.EraseNow(DDGraphics.surfScreen, DDGraphics.surfShadow, DDGraphics.surfStatic);
					fruitSprite.Remove();
				}
			}
		}


		public static void EnableFasterBoots()
		{
			GameData.munchSpeed = GameData.munchLevelSpeed / 2;
			GameData.munchCounter.counter = 0;
		}


		public static void DisableFasterBoots()
		{
			GameData.munchCounter.counter = GameData.munchSpeed = GameData.munchLevelSpeed;
		}


		public static void EnableFrozenGhosts()
		{
			int g;

			for(g = 0; g < Defs.NUM_GHOSTS; g++)
			{
				if(GhostControl.ghostSprite[g].s == (int)StatType.ST_NORMAL)
				{
					GhostControl.ghostSprite[g].s = (int)StatType.ST_FROZEN;
					GhostControl.SetGhostFrame(g);
				}
			}
		}


		public static void DisableFrozenGhosts()
		{
			int g;

			for(g = 0; g < Defs.NUM_GHOSTS; g++)
			{
				if(GhostControl.ghostSprite[g].s == (int)StatType.ST_FROZEN)
				{
					GhostControl.ghostSprite[g].s = (int)StatType.ST_NORMAL;
					GhostControl.SetGhostFrame(g);
				}
			}
		}


		public static void RefreshPowerPills()
		{
			int x, y;
			Rectangle rect;

			for(y = 0; y < Defs.MAZE_HEIGHT; y++)
			{
				for(x = 0; x < Defs.MAZE_WIDTH; x++)
				{
					// Find where the power pills are missing from the game map
					// compared to the permanent map and fill them in.
					if((GameData.permMap[x, y] == (int)BlockType.BT_PILL) && (GameData.map[x, y] != (int)BlockType.BT_PILL))
					{
						GameData.dots++;
						GameData.map[x, y] = (int)BlockType.BT_PILL;
						Maze.DrawMazeTile(DDGraphics.surfStatic, x, y, (int)BlockType.BT_PILL);
						rect = new Rectangle(x*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, y*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
							Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
						DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, rect);
						DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, rect);
					}
				}
			}
		}


		public static void EnableTranslucentGhosts()
		{
			int g;

			for(g = 0; g < Defs.NUM_GHOSTS; g++)
			{
				if((GhostControl.ghostSprite[g].s == (int)StatType.ST_NORMAL) || (GhostControl.ghostSprite[g].s == (int)StatType.ST_FROZEN))
				{
					GhostControl.ghostSprite[g].s = (int)StatType.ST_FROZEN2;
					GhostControl.SetGhostFrame(g);
				}
			}
		}


		public static void DisableTranslucentGhosts()
		{
			int g;

			for(g = 0; g < Defs.NUM_GHOSTS; g++)
			{
				if(GhostControl.ghostSprite[g].s == (int)StatType.ST_FROZEN2)
				{
					GhostControl.ghostSprite[g].s = (int)StatType.ST_NORMAL;
					GhostControl.SetGhostFrame(g);
				}
			}
		}


		public static void EatAllGhosts()
		{
			int g;

			for (g = 0; g < Defs.NUM_GHOSTS; g++)
			{
				GhostControl.DoEatGhost(g, 0);
			}
		}


		public static void GoToNextLevel()
		{
			int x, y, i;
			Rectangle rect;

			i = 0;		// Spinning rotation anim counter for munch man.

			DDSound.StopAllLoopSounds();
			GameData.eatTime.counter = 0;

			for(y = 0; y < Defs.MAZE_HEIGHT; y++)
			{
				for(x = 0; x < Defs.MAZE_WIDTH; x++)
				{
					// Find where the power pills are missing from the game map
					// compared to the permanent map and fill them in.
					if((GameData.map[x, y] == (int)BlockType.BT_PILL) || (GameData.map[x, y] == (int)BlockType.BT_DOT))
					{
						Sprite.BeginUpdate(DDGraphics.surfShadow, DDGraphics.surfStatic);

						MunchControl.munchSprite.SetFrame(Defs.SPRITE_WIDTH, (i++%4)*Defs.SPRITE_HEIGHT);

						Maze.DrawMazeTile(DDGraphics.surfStatic, x, y, (int)BlockType.BT_VOID);
						rect = new Rectangle(x*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, y*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
							Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
						DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, rect);
						DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, rect);

						// If it is a dot then add to the score
						if(GameData.map[x, y] == (int)BlockType.BT_DOT)
						{
							MunchControl.DoEatDot();
						}

						// Remove pill or dot from map and decrement number of dots or pills left to eat
						GameData.map[x, y] = (int)BlockType.BT_VOID;
						GameData.dots--;

						Sprite.EndUpdate(DDGraphics.surfScreen, DDGraphics.surfShadow);

						// Flash the bonus!!
						if((i & 0x04) != 0)
						{
							DrawFruitBonus(DDGraphics.surfShadow, (int)FruitType.FT_BAR2, 0);
						}
						else
						{
							DrawFruitBonus(DDGraphics.surfShadow, (int)FruitType.FT_BAR2, 3);
						}

						rect = new Rectangle(Defs.BONUS_WND_X, Defs.BONUS_WND_Y + ((int)FruitType.FT_BAR2*Defs.SPRITE_HEIGHT),
							5*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
						DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, rect);

						System.Threading.Thread.Sleep(50);
					}
				}
			}

			// Level must be complete...
			GameControl.DoLevelComplete();

			System.Threading.Thread.Sleep(50);
		}


		public static void ExtraLife()
		{
			GameData.lives++;
			GamePanel.ShowLives(GameData.lives);
		}


		public static void EnableFruit(int fruit)
		{
			switch(fruit)
			{
				case (int)FruitType.FT_CHERRY:	// Freeze ghosts
					EnableFrozenGhosts();
					break;

				case (int)FruitType.FT_GRAPE:	// Go faster boots
					EnableFasterBoots();
					break;

				case (int)FruitType.FT_MELON:	// Refresh Power Pills
					RefreshPowerPills();
					break;

				case (int)FruitType.FT_BELL:	// Translucent ghosts.
					EnableTranslucentGhosts();
					break;

				case (int)FruitType.FT_BAR1:	// Eat all the ghosts up
					EatAllGhosts();
					break;

				case (int)FruitType.FT_BAR2:	// Go to next level
					GoToNextLevel();
					break;

				case (int)FruitType.FT_BAR3:	// Extra life
					ExtraLife();
					break;

				default:
					break;
			}
		}


		public static void DisableFruit(int fruit)
		{
			switch(fruit)
			{
				case (int)FruitType.FT_CHERRY:	// Freeze ghosts - Remove.
					DisableFrozenGhosts();
					break;

				case (int)FruitType.FT_GRAPE:	// Go faster boots - Remove.
					DisableFasterBoots();
					break;

				case (int)FruitType.FT_MELON:	// Restore power pills - Nothing to do as it is not a time based bonus.
					break;

				case (int)FruitType.FT_BELL:	// Translucent ghosts - Remove.
					DisableTranslucentGhosts();
					break;

				case (int)FruitType.FT_BAR1:	// Eat all ghosts - Nothing to do as it is not a time based bonus.
					break;

				case (int)FruitType.FT_BAR2:	// This should never be called
					break;

				case (int)FruitType.FT_BAR3:	// This should never be called
					break;

				default:
					break;
			}
		}


		public static void ActionFruitGot(int fruit)
		{
			Rectangle destRect;

			fruitSprite.s = (int)StatType.ST_DEAD;
			fruitSprite.Remove();
			fruitSprite.EraseNow(DDGraphics.surfScreen, DDGraphics.surfShadow, DDGraphics.surfStatic);
			GameData.fruitGot[fruit]++;
			GameData.noFruitTime.counter = 500;		// min 0.5 sec before next fruit.

			DrawFruitBonus(DDGraphics.surfShadow, fruit, GameData.fruitGot[fruit]);
			destRect = new Rectangle(Defs.BONUS_WND_X, Defs.BONUS_WND_Y + fruit * Defs.SPRITE_HEIGHT,
				5 * Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);

			if(GameData.fruitGot[fruit] == 3)
			{
				DDSound.PlayOnce(DDSound.dsbGot3Fruit);
				EnableFruit(fruit);
				if((fruit == (int)FruitType.FT_MELON) || (fruit == (int)FruitType.FT_BAR1) ||
					(fruit == (int)FruitType.FT_BAR2) || (fruit == (int)FruitType.FT_BAR3))
				{
					// These fruit bonuses are not time-limited, so give the play 1 sec to
					// see the bonus appear in the panel.
					GameData.fruitTime[fruit].counter = 1000;
				}
				else
				{
					GameData.fruitTime[fruit].counter = GameData.levelFruitTime;
				}
			}
			else
			{
				DDSound.PlayOnce(DDSound.dsbEatFruit);
			}
		}


		public static void DoFruitAction()
		{
			int i;
			Rectangle rect;

			for(i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				if(GameData.fruitGot[i] == 3)
				{
					if(GameData.fruitTime[i].counter <= 0)
					{
						DisableFruit(i);
						DrawFruitBonus(DDGraphics.surfShadow, i, 0);
						GameData.fruitGot[i] = 0;
					}
					else if(GameData.fruitTime[i].counter < 2000)
					{
						if((GameData.fruitTime[i].counter & 0x80) != 0)
						{
							DrawFruitBonus(DDGraphics.surfShadow, i, 0);
						}
						else
						{
							DrawFruitBonus(DDGraphics.surfShadow, i, 3);
						}
					}
				}

				rect = new Rectangle(Defs.BONUS_WND_X, Defs.BONUS_WND_Y + (i*Defs.SPRITE_HEIGHT),
					5*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
				DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, rect);
			}
		}

	}
}
