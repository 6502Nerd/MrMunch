namespace MrMunch
{
    /// <summary>
    /// Summary description for ScoreSprite.
    /// </summary>
    public class ScoreSprite
	{
		// Movement pattern for score sprites wafting up the screen
        // a simple sinusoidal movement is shown below which can 
        // be repeated.
		static int[] scoreSpriteDXMove = 
		{
				-2, -2, -1, -1, -1,  0, -1,  0,
                 0,  1,  0,  1,  1,  1,  2,  2,
                 2,  2,  1,  1,  1,  0,  1,  0,
                 0, -1,  0, -1, -1, -1, -2, -2
		};

		public static Sprite[] scoreSprite = new Sprite[Defs.NUM_SCORESPRITES];

		public ScoreSprite()
		{
			//
			// TODO: Add constructor logic here
			//
			for(int i = 0; i < Defs.NUM_SCORESPRITES; i++)
			{
				scoreSprite[i] = new Sprite();
			}
		}

		public static void StartScoreSprite(int x, int y, int idx)
		{
			int i;

			for(i = 0; i < Defs.NUM_SCORESPRITES; i++)
			{
				if(scoreSprite[i].s != (int)StatType.ST_NORMAL)
				{
					scoreSprite[i].Init(x, y, (int)StatType.ST_NORMAL, 48, DDGraphics.surfSprite, Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
					scoreSprite[i].SetFrame(idx*Defs.SPRITE_WIDTH, 11*Defs.SPRITE_HEIGHT);
					scoreSprite[i].AddAtBottom();

					return;
				}
			}
		}

		public static void ProcessScoreSprite()
		{
			int i;

			for(i = 0; i < Defs.NUM_SCORESPRITES; i++)
			{
				if(scoreSprite[i].s == (int)StatType.ST_NORMAL)
				{
					if(scoreSprite[i].d-- > 0)
					{
						if(scoreSprite[i].y > 0)
						{
							scoreSprite[i].y--;
							scoreSprite[i].x += scoreSpriteDXMove[(scoreSprite[i].d) % scoreSpriteDXMove.Length];
						}
						else
						{
							scoreSprite[i].d = 0;
						}
					}
					else
					{
						scoreSprite[i].s = (int)StatType.ST_DEAD;
						scoreSprite[i].Remove();
						scoreSprite[i].EraseNow(DDGraphics.surfScreen, DDGraphics.surfShadow, DDGraphics.surfStatic);
					}
				}
			}
		}

	}
}
