/// <summary>
/// Game-wide definitions
/// </summary>
public class Defs
{
	public static int NUM_GHOSTS	=	4;								// The number of ghosts in the game

	public static int MAZE_WIDTH	=	23;								// Horizontal blocks in the maze
	public static int MAZE_HEIGHT	=	23;								// Vertical blocks in the maze
	
	public static int SPRITE_WIDTH	=	16;								// Sprite width in pixels
	public static int SPRITE_HEIGHT	=	16;								// Sprite height in pixels
	
	public static int GAME_WIDTH	=	(MAZE_WIDTH * SPRITE_WIDTH);	// Game area width in pixels
	public static int GAME_HEIGHT	=	(MAZE_HEIGHT * SPRITE_HEIGHT);	// Game area height in pixels
	
	public static int GAME_OFFX		=	8;								// Offset in to maze
	public static int GAME_OFFY		=	8;								// Offset in to maze
	
	public static int WINDOW_WIDTH	=	512;							// Windows client area width
	public static int WINDOW_HEIGHT	=	384;							// Windows client area height
	
	public static int GHOST_HOMEX	=	11;								// Map x position of ghost home
	public static int GHOST_HOMEY	=	9;								// Map y position of ghost home
	
	public static int MUNCH_HOMEX	=	11;								// Map x position of munch start point
	public static int MUNCH_HOMEY	=	11;								// Map y position of munch start point
	
	public static int FRUIT_HOMEX	=	11;								// Map x position of fruit
	public static int FRUIT_HOMEY	=	11;								// Map x position of fruit

	public static int STEP_SIZE		=	4;								// Step size for movements
    public static int HIT_SIZE      =   4;								// Hit box size;

	public static int SCORE_WND_X	=	396;							// Pixel x position of score window
	public static int SCORE_WND_Y	=	56;								// Pixel y position of score window

	public static int HISCORE_WND_X	=	395;							// Pixel x position of hi-score window
	public static int HISCORE_WND_Y	=	186;							// Pixel y position of hi-score window

	public static int LIVES_WND_X	=	419;							// Pixel x position of lives window
	public static int LIVES_WND_Y	=	143;							// Pixel y position of lives window

	public static int LEVEL_WND_X	=	427;							// Pixel x position of level window
	public static int LEVEL_WND_Y	=	99;								// Pixel y position of level window

	public static int BONUS_WND_X	=	408;							// Pixel x position of bonus window
	public static int BONUS_WND_Y	=	232;							// Pixel y position of bonus window

	public static int NORMAL_MUNCH_SPEED = 20;							// Mr Munch's default speed
	public static int NORMAL_GHOST_SPEED = 26;							// Ghost's default speed
	public static int GHOST_FLEE_SPEED = 30;							// Ghost's speed when fleeing
	public static int GHOST_GOHOME_SPEED = 8;							// Speed when going home (after being eaten)

	public static int SCORE_SPRITE_SPEED = 26;

	public static int NUM_SCORESPRITES = 5;								// Number of score sprites

	public static int DOT_SCORE = 1;									// Points gained for eating a dot (not pill)
}

