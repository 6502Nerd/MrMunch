namespace MrMunch
{
    public enum BlockType				// Valid block types in maze
	{
		BT_VOID,						// Nothing there
		BT_DOT,							// Normal dot
		BT_PILL,						// Power pill
		BT_BONUS,						// Bonus symbol
		BT_WALL,						// Impassable blocks from here
		BT_WALLHORZ = BT_WALL,			// A horizontal wall
		BT_WALLVERT,					// A vertical wall
		BT_WALLTOPLEFT,					// A top-left corner
		BT_WALLTOPRIGHT,				// A top-right corner
		BT_WALLBOTLEFT,					// A bottom-left corner
		BT_WALLBOTRIGHT,				// A bottom-right corner
		BT_WALLTLEFT,					// A t-shape facing left
		BT_WALLTRIGHT,					// A t-shape facing right
		BT_WALLTUP,						// A t-shape facing up
		BT_WALLTDOWN,					// A t-shape facing down
		BT_WALLSTOPLEFT,				// An end facing left
		BT_WALLSTOPRIGHT,				// An end facing right
		BT_WALLSTOPUP,					// An end facing up
		BT_WALLSTOPDOWN,				// An end facing down
		BT_WALLALONE,					// A wall on its own
		BT_WALLDOOR,					// The ghosts door
		BT_NUMTYPES						// End of enumerations
	};

	public enum StatType				// Allowed statuses of dynamic objects
	{
		ST_DEAD,						// Dead, not in game at present
		ST_NORMAL,						// The normal status for this object
		ST_FLEE,						// When ghost is fleeing from munch
		ST_GOHOME,						// When ghost has been eaten (going back to den)
		//	ST_ATHOME,						// When ghost is at home
		ST_FROZEN,						// When ghost is frozen, but still deadly.
		ST_FROZEN2,						// When ghost is frozen and not deadly.
		ST_NUMTYPES						// End of enumerations
	};

	public enum DirType					// Directions that objects can travel in
	{
		DIR_LEFT,
		DIR_RIGHT,
		DIR_DOWN,
		DIR_UP,
		DIR_NUMTYPES
	};
	
	public enum FruitType
	{
		FT_CHERRY,
		FT_GRAPE,
		FT_MELON,
		FT_BELL,
		FT_BAR1,
		FT_BAR2,
		FT_BAR3,
		FT_NUMTYPES
	};

	// This is a simple timer which keeps a count of the time
	// remaining and whether or not the timer has expired.
	// Needs updating by an update routine - this class doesn't
	// have the ability to update the counter by itself
	public class GameTimer
	{
		int c;

		public int counter
		{
			get
			{
				return c;
			}
			set
			{
				c = value;
				if(c <= 0)
				{
					expired = true;
				}
				else
				{
					expired = false;
				}
			}
		}

		public bool expired;
	}

	public class GameData
	{
		static public int lives;													// How many lives are left in the game
		static public int dots;														// How many dots are left to be eaten
		static public int score;													// Current score
		static public int hiScore;													// The high score in the game
		static public int level;													// Current game level
		static public int mapLevel;													// Current level for game map
		static public int munchLevelSpeed;											// Normal munch speed for this level
		static public int munchSpeed;												// Current munch speed (may be different becase of bonuses)
		static public int ghostChaseSpeed;											// Ghost chase speed
		static public int ghostFleeSpeed;											// Ghost flee speed
		static public int ghostEatenSpeed;											// Ghost going back to den speed
		static public GameTimer munchCounter = new GameTimer();
		static public GameTimer ghostChaseCounter = new GameTimer();
		static public GameTimer ghostFleeCounter = new GameTimer();
		static public GameTimer ghostEatenCounter = new GameTimer();
		static public GameTimer scoreSpriteCounter = new GameTimer();
		static public GameTimer eatTime = new GameTimer();							// Time remaining in eat mode
		static public GameTimer noFruitTime = new GameTimer();						// Time remaining before fruit are allowed to be shown.
		static public int totEaten;													// How many ghosts eaten successively
		static public int []fruitGot = new int[(int)FruitType.FT_NUMTYPES];			// How many of each fruit got
		static public GameTimer []fruitTime = new GameTimer[(int)FruitType.FT_NUMTYPES];		// Time left on each fruit action
		static public int levelFruitTime;											// How long a fruit action lasts for a level
		static public int currFruit;												// Which fruit is currently on (0 = None)
		static public GameTimer currFruitTime = new GameTimer();					// How long before the fruit disappears.
		static public int left_hyper;												// Vertical position on left hand side of hyper-jump
		static public int right_hyper;												// Vertical position on right hand side of hyper-jump
		static public int top_hyper;												// Horizontal position on top side of hyper-jump
		static public int bottom_hyper;												// Horizontal position on bottom side of hyper-jump
		static public int [,]map = new int[Defs.MAZE_WIDTH, Defs.MAZE_HEIGHT];		// Game map during play
		static public int [,]permMap = new int[Defs.MAZE_WIDTH, Defs.MAZE_HEIGHT];	// Level game map
	};

	/// <summary>
	/// Common definitions
	/// </summary>
	public class Common
	{
		public Common()
		{
			//
			// TODO: Add constructor logic here
			//
			for (int i = 0; i < (int)FruitType.FT_NUMTYPES; i++)
			{
				GameData.fruitTime[i] = new GameTimer();
			}
		}
	}
}
