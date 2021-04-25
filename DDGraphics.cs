using System.Windows.Forms;
using System.Drawing;
using MrMunch;

public class DDGraphics
{
	private static Control owner = null;

	private const int spriteSize = 50;
	private static Control localDevice = null;
	public static DDSurface surfScreen = null;
	public static DDSurface surfShadow = null;
	public static DDSurface surfStatic = null;
	public static DDSurface surfSprite = null;
    public static DDSurface surfStaticSprite = null;

	public DDGraphics(Control owner)
	{
		DDGraphics.owner = owner;

        localDevice = owner;

		CreateSurfaces();
        
	}

	private void CreateSurfaces()
	{
        surfStaticSprite = new DDSurface(MrMunch.Properties.Resources.sprites);

        surfSprite = new DDSurface(MrMunch.Properties.Resources.sprites);
        surfSprite.b.MakeTransparent(Color.FromArgb(0));

        surfShadow = new DDSurface(new Bitmap(Defs.WINDOW_WIDTH, Defs.WINDOW_HEIGHT));

        surfStatic = new DDSurface(MrMunch.Properties.Resources.background);

        surfScreen = new DDSurface(null);
        surfScreen.g = owner.CreateGraphics();
	}

	public static void UpdateSurface(DDSurface surfDest, DDSurface surfSrc, Rectangle rect)
	{
		Rectangle screenRect = new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);

        // Do the blit!
        surfDest.g.DrawImage(surfSrc.b, rect, rect, GraphicsUnit.Pixel);
	}

	public static void RefreshScreen(DDSurface ddsDest, DDSurface ddsSrc)
	{
		Rectangle rect = new Rectangle(0, 0, Defs.WINDOW_WIDTH, Defs.WINDOW_HEIGHT);
		UpdateSurface(ddsDest, ddsSrc, rect);
	}

	public static void ShowString(DDSurface ddsDest, int x, int y, string s)
	{
		Rectangle srcRect, destRect;
		int sx, sy, i;
		char c;

		for(i = 0; i < s.Length; i++)
		{
			c = s[i];
			if((c >= '0') && (c <= '9'))
			{
				sx = (c-'0')*Defs.SPRITE_WIDTH;
				sy = 12*Defs.SPRITE_WIDTH;
			}
			else if((c >= 'A') && (c <= 'Z'))
			{
				sx = ((c-'A')%13)*Defs.SPRITE_WIDTH;
				sy = (13+((c-'A')/13))*Defs.SPRITE_HEIGHT;
			}
			else
			{
				sx = 11*Defs.SPRITE_WIDTH;
				sy = 12*Defs.SPRITE_WIDTH;
			}

			destRect = new Rectangle(x, y, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
			srcRect = new Rectangle(sx, sy, Defs.SPRITE_HEIGHT, Defs.SPRITE_WIDTH);
			ddsDest.g.DrawImage(surfSprite.b, destRect, srcRect, GraphicsUnit.Pixel);

			x += Defs.SPRITE_WIDTH;
		}
	}

	public static void ShowMsgNow(int x, int y, string s, int del)
	{
		Rectangle dest;

		ShowString(DDGraphics.surfScreen, x, y, s);

		System.Threading.Thread.Sleep(del);

		dest = new Rectangle (x, y, s.Length*Defs.SPRITE_WIDTH, Defs.SPRITE_HEIGHT);
		UpdateSurface(surfScreen, surfShadow, dest);
	}

}