using System.Drawing;

namespace MrMunch
{
    public class DDSurface
    {
        public Graphics g = null;
        public Bitmap b = null;

        public DDSurface(Bitmap bmnew)
        {
            b = bmnew;
            if (b != null)
            {
                g = Graphics.FromImage(b);
            }
        }

        ~DDSurface()
        {
            g.Dispose();
        }
    }
}
