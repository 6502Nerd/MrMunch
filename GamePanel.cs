using System;
using System.Drawing;

namespace MrMunch
{
    /// <summary>
    /// Summary description for GamePanel.
    /// </summary>
    public class GamePanel
	{
		public GamePanel()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public static void ShowLives(int l)
		{
			int i;

			if (l > 3)
			{
				l = 3;
			}
			Rectangle destRect = new Rectangle(Defs.LIVES_WND_X, Defs.LIVES_WND_Y, 3*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, destRect);
			Rectangle srcRect = new Rectangle(3*Defs.SPRITE_WIDTH, 1*Defs.SPRITE_HEIGHT, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
			for(i = 0; i < l; i++)
			{
				destRect = new Rectangle(Defs.LIVES_WND_X+i*Defs.SPRITE_WIDTH, Defs.LIVES_WND_Y, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
				DDGraphics.surfShadow.g.DrawImage(DDGraphics.surfSprite.b, destRect, srcRect, GraphicsUnit.Pixel);
			}
			destRect = new Rectangle(Defs.LIVES_WND_X, Defs.LIVES_WND_Y, 3*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);
		}


		public static void ShowScore(int sc)
		{
			Rectangle destRect;
			string output;

			output = String.Format("{0, 6:d}", sc);
			destRect = new Rectangle(Defs.SCORE_WND_X, Defs.SCORE_WND_Y, 6*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, destRect);
			DDGraphics.ShowString(DDGraphics.surfShadow, Defs.SCORE_WND_X, Defs.SCORE_WND_Y, output);
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);
		}


		public static void ShowHiScore(int sc)
		{
			Rectangle destRect;
			string output;

			output = String.Format("{0, 6:d}", sc);
			destRect = new Rectangle(Defs.HISCORE_WND_X, Defs.HISCORE_WND_Y, 6*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, destRect);
			DDGraphics.ShowString(DDGraphics.surfShadow, Defs.HISCORE_WND_X, Defs.HISCORE_WND_Y, output);
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);
		}


		public static void ShowLevel(int l)
		{
			Rectangle destRect;
			string output;

			output = String.Format("{0, 2:d}", l);
			destRect = new Rectangle(Defs.LEVEL_WND_X, Defs.LEVEL_WND_Y, 2*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
			DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, destRect);
			DDGraphics.ShowString(DDGraphics.surfShadow, Defs.LEVEL_WND_X, Defs.LEVEL_WND_Y, output);
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, destRect);
		}
	}
}
