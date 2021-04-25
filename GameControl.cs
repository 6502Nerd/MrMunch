using System.Timers;

namespace MrMunch
{
    /// <summary>
    /// Enum describing functions that a loop can do
    /// </summary>
    public enum GameFunctions
	{
		GAME_NOTHING,
		GAME_INITGAME,
		GAME_STARTPLAY,
		GAME_PLAY
	}

	/// <summary>
	/// Summary description for GameControl.
	/// </summary>
	public class GameControl
	{
		public static int interval = 1000/100;		// The interval between game loop processing in msecs
		public static GameFunctions gameFunc = GameFunctions.GAME_NOTHING;
		public static bool gamePaused = false;
		public static MunchRandom munchRandom = new MunchRandom();
		public Timer gameTimer = new Timer();
		private static bool intervalExpired = false;

		public GameControl()
		{
			//
			// TODO: Add constructor logic here
			//
			gameTimer.Elapsed += new System.Timers.ElapsedEventHandler(gameTimer_Elapsed);
			gameTimer.Enabled = true;
			gameTimer.AutoReset = true;
			gameTimer.Interval = interval;
		}

		protected static void gameTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if(!GameControl.gamePaused)
			{
				intervalExpired = true;
				ProcessTimings();
			}
		}

		public static void PlayDead()
		{
			int i;

			DDSound.StopAllSounds();
			DDSound.PlayOnce(DDSound.dsbMunchDead);

			
			Sprite.EndUpdate(DDGraphics.surfScreen, DDGraphics.surfShadow);
			for(i = 0; i < 50; i++)
			{
				Sprite.BeginUpdate(DDGraphics.surfShadow, DDGraphics.surfStatic);
				MunchControl.munchSprite.SetFrame(Defs.SPRITE_WIDTH, (i%4)*Defs.SPRITE_HEIGHT);
				Sprite.EndUpdate(DDGraphics.surfScreen, DDGraphics.surfShadow);
				System.Threading.Thread.Sleep(30);
			}

			GameData.lives--;
			GamePanel.ShowLives(GameData.lives);

			System.Threading.Thread.Sleep(100);

			if(GameData.lives != 0)
			{
				gameFunc = GameFunctions.GAME_STARTPLAY;
			}
			else
			{
				if(GameData.score > GameData.hiScore)
				{
					GameData.hiScore = GameData.score;
					GamePanel.ShowHiScore(GameData.hiScore);
				}
				gameFunc = GameFunctions.GAME_NOTHING;
			}
		}

		// Perform actions when the level has been completed
		// and prepare for the next level of the game
		public static void DoLevelComplete()
		{
			int i;
			string mapFName;

			DDSound.StopAllSounds();
			DDSound.PlayOnce(DDSound.dsbLevelComplete);
			System.Threading.Thread.Sleep(200);

			GameData.level++;
            GameData.mapLevel++;

			// Try to fine a maze with name 'mapX.txt' where X is the
			// level (e.g. level 1 is map1.txt etc.)
			mapFName = "map"+((GameData.mapLevel % 3)+1);

			Maze.LoadMap(mapFName, GameData.permMap);
			Maze.InitMap();

			gameFunc = GameFunctions.GAME_STARTPLAY;

			// Clear all fruits for next level..
			for(i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				GameData.fruitGot[i] = 0;
				GameData.fruitTime[i].counter = 0;
			}
		}

		// Initialise all game timers to their standard values for the level
		// that the game is currently on - used at start of level or resuming after
		// life lost
		public static void InitTimers()
		{
			GameData.munchCounter.counter = GameData.munchSpeed = GameData.munchLevelSpeed = Defs.NORMAL_MUNCH_SPEED;
			// Ghosts chase faster every 2nd level
			GameData.ghostChaseCounter.counter = GameData.ghostChaseSpeed = Defs.NORMAL_GHOST_SPEED - ((GameData.level-1)/2)*4;
			// Ghosts flee faster every 4th level
			GameData.ghostFleeCounter.counter = GameData.ghostFleeSpeed = Defs.GHOST_FLEE_SPEED - ((GameData.level-1)/4)*6;
			GameData.ghostEatenCounter.counter = GameData.ghostEatenSpeed = Defs.GHOST_GOHOME_SPEED;
			GameData.scoreSpriteCounter.counter = Defs.SCORE_SPRITE_SPEED;

			GameData.noFruitTime.counter = 1000;			// min. 1 sec delay before fruit start appearing.

			GameData.levelFruitTime = 8000;					// Currently fixed 8 secs that fruit time lasts for
	
			GameData.eatTime.counter = 0;

			for(int i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				GameData.fruitTime[i].counter = 0;
				// Only clear fruit bonuses that are currently in force...
				if(GameData.fruitGot[i] >= 3)
				{
					GameData.fruitGot[i] = 0;
				}
			}
		}

		// Used to initialise level specific properties
		public static void InitLevel()
		{
			GameData.totEaten = 0;
			GameData.currFruit = -1;
		}

		public static void StartPlay()
		{
			int i;

			InitLevel();			// Initialise level dependant stuff.

			Sprite.ClearList();

			DDSound.Stop(DDSound.dsbMainTune);

			MunchControl.munchSprite.Init(Defs.SPRITE_WIDTH*Defs.MUNCH_HOMEX + Defs.GAME_OFFX,
				Defs.SPRITE_HEIGHT*Defs.MUNCH_HOMEY + Defs.GAME_OFFY,
				(int)StatType.ST_NORMAL, (int)DirType.DIR_RIGHT,
				DDGraphics.surfSprite, Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			MunchControl.SetMunchFrame();
			MunchControl.munchSprite.AddAtTop();
			MunchControl.nextDir = (int)DirType.DIR_RIGHT;

			for(i = 0; i < Defs.NUM_GHOSTS; i++)
			{
				GhostControl.ghostSprite[i].Init((Defs.GHOST_HOMEX+(i%4)-2)*Defs.SPRITE_WIDTH + Defs.GAME_OFFX, 
					Defs.GHOST_HOMEY*Defs.SPRITE_HEIGHT + Defs.GAME_OFFY,
					(int)StatType.ST_NORMAL, (int)(((i&1)!=0)? DirType.DIR_RIGHT:DirType.DIR_RIGHT),
					DDGraphics.surfSprite, Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
				GhostControl.SetGhostFrame(i);
				GhostControl.ghostSprite[i].AddAtTop();
			}

			// Clear all score sprites.
			for(i = 0; i < Defs.NUM_SCORESPRITES; i++)
			{
				ScoreSprite.scoreSprite[i].s = (int)StatType.ST_DEAD;
			}

			Fruit.fruitSprite.s = (int)StatType.ST_DEAD;

			Maze.DrawMaze(DDGraphics.surfStatic, GameData.map);
			Fruit.DrawAllFruitBonus(DDGraphics.surfStatic, GameData.fruitGot);

			DDGraphics.RefreshScreen(DDGraphics.surfShadow, DDGraphics.surfStatic);
			DDGraphics.RefreshScreen(DDGraphics.surfScreen, DDGraphics.surfShadow);
	
			GamePanel.ShowHiScore(GameData.hiScore);
			GamePanel.ShowScore(GameData.score);
			GamePanel.ShowLives(GameData.lives);
			GamePanel.ShowLevel(GameData.level);
			Sprite.BeginUpdate(DDGraphics.surfShadow, DDGraphics.surfStatic);
			Sprite.EndUpdate(DDGraphics.surfScreen, DDGraphics.surfShadow);
			DDGraphics.ShowMsgNow(9*Defs.SPRITE_WIDTH+Defs.GAME_OFFX, 11*Defs.SPRITE_HEIGHT+Defs.GAME_OFFY, "READY", 1000);
			InitTimers();
    		DDSound.PlayLoop(DDSound.dsbMainTune);
			gameFunc = GameFunctions.GAME_PLAY;
		}

		public static void InitGame()
		{
			int i;

			GameData.score = 0;
			GameData.lives = 3;
			GameData.level = 1;
			GameData.mapLevel = 1;
			for(i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				GameData.fruitGot[i] = 0;
			}
			GameData.noFruitTime.counter = 1000;				// 1 sec delay before fruit start appearing
			Maze.LoadMap("map1", GameData.permMap);
			Maze.InitMap();
			gameFunc = GameFunctions.GAME_STARTPLAY;
		}

		public static void ProcessGamePlayLoop()
		{
			if(intervalExpired)
			{
				intervalExpired = false;

				Sprite.BeginUpdate(DDGraphics.surfShadow, DDGraphics.surfStatic);

				if(GameData.ghostChaseCounter.expired)
				{
					GameData.ghostChaseCounter.counter = GameData.ghostChaseSpeed;
					GhostControl.ProcessGhosts((int)StatType.ST_NORMAL);
				}
				if(GameData.ghostFleeCounter.expired)
				{
					GameData.ghostFleeCounter.counter = GameData.ghostFleeSpeed;
					GhostControl.ProcessGhosts((int)StatType.ST_FLEE);
				}
				if(GameData.ghostEatenCounter.expired)
				{
					GameData.ghostEatenCounter.counter = GameData.ghostEatenSpeed;
					GhostControl.ProcessGhosts((int)StatType.ST_GOHOME);
				}
				if(GameData.scoreSpriteCounter.expired)
				{
					GameData.scoreSpriteCounter.counter = Defs.SCORE_SPRITE_SPEED;
					ScoreSprite.ProcessScoreSprite();
					Fruit.ProcessFruitBonus();
				}
				if(GameData.munchCounter.expired)
				{
					GameData.munchCounter.counter = GameData.munchSpeed;
					MunchControl.ProcessMunch();
					Fruit.DoFruitAction();
				}

				Sprite.EndUpdate(DDGraphics.surfScreen, DDGraphics.surfShadow);

			}
		}

		public static void ProcessGameNothingLoop()
		{
		}

		public static void ProcessGameStartPlayLoop()
		{
			StartPlay();
		}

		public static void ProcessGameInitGameLoop()
		{
			InitGame();
		}

		public static void ProcessLoop()
		{
			switch((int)gameFunc)
			{
				case (int)GameFunctions.GAME_NOTHING:
					ProcessGameNothingLoop();
					break;
				case (int)GameFunctions.GAME_INITGAME:
					ProcessGameInitGameLoop();
					break;
				case (int)GameFunctions.GAME_PLAY:
					ProcessGamePlayLoop();
					break;
				case (int)GameFunctions.GAME_STARTPLAY:
					ProcessGameStartPlayLoop();
					break;
			}
		}

		// This method decrements game timers by the elapsed interval
		// as long as the timer has not expired
		public static void ProcessTimings()
		{
			int i;

			if(!GameData.currFruitTime.expired)
			{
				GameData.currFruitTime.counter -= interval;
			}
			if(!GameData.munchCounter.expired)
			{
				GameData.munchCounter.counter -= interval;
			}
			if(!GameData.ghostChaseCounter.expired)
			{
				GameData.ghostChaseCounter.counter -= interval;
			}
			if(!GameData.ghostFleeCounter.expired)
			{
				GameData.ghostFleeCounter.counter -= interval;
			}
			if(!GameData.ghostEatenCounter.expired)
			{
				GameData.ghostEatenCounter.counter -= interval;
			}
			if(!GameData.eatTime.expired)
			{
				GameData.eatTime.counter -= interval;
			}
			if(!GameData.scoreSpriteCounter.expired)
			{
				GameData.scoreSpriteCounter.counter -= interval;
			}
			if(!GameData.noFruitTime.expired)
			{
				GameData.noFruitTime.counter -= interval;
			}
			for(i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				if(!GameData.fruitTime[i].expired)
				{
					GameData.fruitTime[i].counter -= interval;
				}
			}
		}

		/// <summary>
		/// Updates x and y based on direction and step size
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="dir"></param>
		/// <param name="step"></param>
		public static void UpdateMove(ref int x, ref int y, int dir, int step)
		{
			if(dir == (int)DirType.DIR_LEFT)
			{
				x -= step;
				if (x <= Defs.GAME_OFFX)
				{
					x = Defs.GAME_OFFX + Defs.GAME_WIDTH - Defs.SPRITE_WIDTH;
					y = Defs.GAME_OFFY + GameData.right_hyper * Defs.SPRITE_HEIGHT;
				}
			}
			else if(dir == (int)DirType.DIR_RIGHT)
			{
				x += step;
				if (x >= (Defs.GAME_OFFX + Defs.GAME_WIDTH - Defs.SPRITE_WIDTH))
				{
					x = Defs.GAME_OFFX;
					y = Defs.GAME_OFFY + GameData.left_hyper * Defs.SPRITE_HEIGHT;
				}
			}
			else if(dir == (int)DirType.DIR_UP)
			{
				y -= step;
				if (y <= Defs.GAME_OFFY)
				{
					y = Defs.GAME_OFFY + Defs.GAME_HEIGHT - Defs.SPRITE_HEIGHT;
					x = Defs.GAME_OFFX + GameData.bottom_hyper * Defs.SPRITE_WIDTH;
				}
			}
			else if(dir == (int)DirType.DIR_DOWN)
			{
				y += step;
				if (y >= (Defs.GAME_OFFY + Defs.GAME_HEIGHT - Defs.SPRITE_HEIGHT))
				{
					y = Defs.GAME_OFFY;
					x = Defs.GAME_OFFX + GameData.top_hyper * Defs.SPRITE_WIDTH;
				}
			}
		}

	}
}
