using System.Drawing;

namespace MrMunch
{
    /// <summary>
    /// Summary description for Sprite.
    /// </summary>
    public class Sprite
	{
		public int s, oS;				// Status of sprite (0 = inactive)
		public int x, y;				// Sprite coordinates on screen
		public int oX, oY;				// Old sprite coordinates on screen
		public int w, h;				// Sprite width and height
		public int srcX, srcY;			// Source data coordinates of sprite
		public int d;					// Direct of sprite travel
		DDSurface sourceImage;			// Source surface for sprite image
		Sprite nextSprite;				// Next sprite in list
		Sprite prevSprite;				// Previous sprite in list

		public Sprite()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static Sprite	firstSprite = null;					// Anchor sprite
		static int		inUpdate = 0;						// Update status

		public void Draw(DDSurface ddsDest)
		{
			Rectangle srcRect = new Rectangle(srcX, srcY, w, h);
			Rectangle destRect = new Rectangle(x, y, w, h);

            ddsDest.g.DrawImage(sourceImage.b, destRect, srcRect, GraphicsUnit.Pixel);
		}

		public void Erase(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Rectangle rect = new Rectangle(x, y, w, h);

			DDGraphics.UpdateSurface(ddsDest, ddsSrc, rect);
		}

		public void Update(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Rectangle oldRect = new Rectangle(oX, oY, w, h);
			Rectangle newRect = new Rectangle(x, y, w, h);

			// If rectangles overlap then do update with one call
			if(oldRect.IntersectsWith(newRect))
			{
				Rectangle dirtyRect = Rectangle.Union(oldRect, newRect);
				DDGraphics.UpdateSurface(ddsDest, ddsSrc, dirtyRect);
			}
			else // otherwise do it with two
			{
				DDGraphics.UpdateSurface(ddsDest, ddsSrc, oldRect);
				DDGraphics.UpdateSurface(ddsDest, ddsSrc, newRect);
			}
		}

		public static void DrawAll(DDSurface ddsDest)
		{
			Sprite sprite;

			sprite = firstSprite;
			while(sprite != null)
			{
				if(sprite.s != 0)
				{
					sprite.Draw(ddsDest);
				}
				sprite = sprite.nextSprite;
			}
		}

		public static void EraseAll(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Sprite sprite;

			sprite = firstSprite;
			while(sprite != null)
			{
				if(sprite.s != 0)
				{
					sprite.Draw(ddsDest);
				}
				sprite = sprite.nextSprite;
			}
		}

		public static void UpdateAll(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Sprite sprite;

			sprite = firstSprite;
			while(sprite != null)
			{
				if(sprite.s != 0)
				{
					sprite.Update(ddsDest, ddsSrc);
				}
				sprite = sprite.nextSprite;
			}
		}

		public static void BeginUpdate(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Sprite sprite;

			if(inUpdate == 0)
			{
				inUpdate = 1;
				sprite = firstSprite;
				while(sprite != null)
				{
					if((sprite.s != 0) || (sprite.oS != 0))
					{
						sprite.oX = sprite.x;
						sprite.oY = sprite.y;
						sprite.oS = sprite.s;
						sprite.Erase(ddsDest, ddsSrc);
					}
					sprite = sprite.nextSprite;
				}
			}
		}

		public static void EndUpdate(DDSurface ddsDest, DDSurface ddsSrc)
		{
			Sprite sprite;

			if(inUpdate != 0)
			{
				inUpdate = 0;
				DrawAll(ddsSrc);
				sprite = firstSprite;
				while(sprite != null)
				{
					if(sprite.oS != 0)
					{
						sprite.Update(ddsDest, ddsSrc);
					}
					sprite = sprite.nextSprite;
				}
			}
		}

		public void AddAtBottom()
		{
			// Add this sprite object to the beginning of the linked list of sprites
			prevSprite = null;
			nextSprite = firstSprite;
			firstSprite.prevSprite = this;
			firstSprite = this;
		}

		public void AddAtTop()
		{
			nextSprite = null;
			prevSprite = null;

			// Add tihs sprite object to the end of the linked list of sprites
			if(firstSprite != null)
			{
				Sprite listSprite = firstSprite;
				while(listSprite.nextSprite != null)
				{
					listSprite = listSprite.nextSprite;
				}
				listSprite.nextSprite = this;
				prevSprite = listSprite;
			}
			else
			{
				firstSprite = this;
			}
		}

		public void Remove()
		{
			if(nextSprite != null)
			{
				nextSprite.prevSprite = prevSprite;
			}
			if(prevSprite != null)
			{
				prevSprite.nextSprite = nextSprite;
			}
			if(firstSprite == this)
			{
				firstSprite = nextSprite;
			}
		}

		public static void ClearList()
		{
			firstSprite = null;
			inUpdate = 0;
		}


		public void Init(int ix, int iy, int ist, int id, DDSurface iddsSpriteSrc, int iw, int ih)
		{
			x = oX = ix;
			y = oY = iy;
			d = id;
			s = oS = ist;
			w = iw;
			h = ih;
			nextSprite = null;
			prevSprite = null;
			sourceImage = iddsSpriteSrc;
		}

		public void SetPos(int px, int py)
		{
			x = px;
			y = py;
		}

		public void SetStat(int st)
		{
			s = st;
		}

		public void SetAllStat(int st)
		{
			Sprite sprite;

			sprite = firstSprite;
			while(sprite != null)
			{
				sprite.SetStat(st);
				sprite = sprite.nextSprite;
			}
		}

		public void SetFrame(int xSrc, int ySrc)
		{
			srcX = xSrc;
			srcY = ySrc;
		}

		public void DrawNow(DDSurface ddsDest, DDSurface ddsShadow)
		{
		}

		public void EraseNow(DDSurface ddsScreen, DDSurface ddsShadow, DDSurface ddsStatic)
		{
			Rectangle destRect = new Rectangle(x, y, w, h);
			DDGraphics.UpdateSurface(ddsShadow, ddsStatic, destRect);
			DDGraphics.UpdateSurface(ddsScreen, ddsShadow, destRect);
		}

	}
}
