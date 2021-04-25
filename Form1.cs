using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.IO;

namespace MrMunch
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainClass : System.Windows.Forms.Form
    {
        private IContainer components;

		private DDGraphics ddGraphics = null;
		private DDSound ddSound = null;
		private GameControl gameControl = null;
		private Common common = null;
		private ScoreSprite scoreSprite = null;
		private System.Windows.Forms.MenuItem menuItemGame;
		private System.Windows.Forms.MenuItem menuItemStart;
		private System.Windows.Forms.MenuItem menuItemPaused;
		private System.Windows.Forms.MenuItem menuItemQuit;
		private System.Windows.Forms.MenuItem menuItemSound;
		private System.Windows.Forms.MenuItem menuItemMusic;
		private System.Windows.Forms.MenuItem menuItemEffects;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItemInstructions;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.MainMenu mainMenu;
		private GhostControl ghostControl = null;

		public MainClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.ClientSize = new System.Drawing.Size(Defs.WINDOW_WIDTH, Defs.WINDOW_HEIGHT);

			common = new Common();
			scoreSprite = new ScoreSprite();
			ddGraphics = new DDGraphics(this);
			ddSound = new DDSound(this);
			gameControl = new GameControl();
			ghostControl = new GhostControl();

			this.Show();

			DDGraphics.UpdateSurface(DDGraphics.surfShadow, DDGraphics.surfStatic, new Rectangle(0, 0, Defs.WINDOW_WIDTH, Defs.WINDOW_HEIGHT));
		}

		private void StartLoop()
		{
			while(Created)
			{
				if(!GameControl.gamePaused)
				{
					GameControl.ProcessLoop();
				}
				Application.DoEvents();
			}
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainClass));
            this.menuItemGame = new System.Windows.Forms.MenuItem();
            this.menuItemStart = new System.Windows.Forms.MenuItem();
            this.menuItemPaused = new System.Windows.Forms.MenuItem();
            this.menuItemQuit = new System.Windows.Forms.MenuItem();
            this.menuItemSound = new System.Windows.Forms.MenuItem();
            this.menuItemMusic = new System.Windows.Forms.MenuItem();
            this.menuItemEffects = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemInstructions = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.SuspendLayout();
            // 
            // menuItemGame
            // 
            this.menuItemGame.Index = 0;
            this.menuItemGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemStart,
            this.menuItemPaused,
            this.menuItemQuit});
            this.menuItemGame.Text = "Game";
            // 
            // menuItemStart
            // 
            this.menuItemStart.Index = 0;
            this.menuItemStart.Text = "Start";
            this.menuItemStart.Click += new System.EventHandler(this.menuItemStart_Click);
            // 
            // menuItemPaused
            // 
            this.menuItemPaused.Index = 1;
            this.menuItemPaused.Text = "Paused";
            this.menuItemPaused.Click += new System.EventHandler(this.menuItemPaused_Click);
            // 
            // menuItemQuit
            // 
            this.menuItemQuit.Index = 2;
            this.menuItemQuit.Text = "Quit";
            this.menuItemQuit.Click += new System.EventHandler(this.menuItemQuit_Click);
            // 
            // menuItemSound
            // 
            this.menuItemSound.Index = 1;
            this.menuItemSound.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemMusic,
            this.menuItemEffects});
            this.menuItemSound.Text = "Sound";
            // 
            // menuItemMusic
            // 
            this.menuItemMusic.Index = 0;
            this.menuItemMusic.Text = "Music";
            this.menuItemMusic.Click += new System.EventHandler(this.menuItemMusic_Click);
            // 
            // menuItemEffects
            // 
            this.menuItemEffects.Checked = true;
            this.menuItemEffects.Index = 1;
            this.menuItemEffects.Text = "Effects";
            this.menuItemEffects.Click += new System.EventHandler(this.menuItemEffects_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 2;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemInstructions,
            this.menuItemAbout});
            this.menuItemHelp.Text = "About";
            // 
            // menuItemInstructions
            // 
            this.menuItemInstructions.Index = 0;
            this.menuItemInstructions.Text = "Instructions";
            this.menuItemInstructions.Click += new System.EventHandler(this.menuItemInstructions_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 1;
            this.menuItemAbout.Text = "About";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGame,
            this.menuItemSound,
            this.menuItemHelp});
            // 
            // MainClass
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(504, 350);
            this.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainClass";
            this.Text = "Mr Munch!";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{


            MainClass m = new MainClass();

			m.StartLoop();
		}

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			DDGraphics.UpdateSurface(DDGraphics.surfScreen, DDGraphics.surfShadow, new Rectangle(0, 0, Defs.WINDOW_WIDTH, Defs.WINDOW_HEIGHT));
		}

		private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Left)
				MunchControl.nextDir = (int)DirType.DIR_LEFT;
			else if(e.KeyCode == Keys.Right)
				MunchControl.nextDir = (int)DirType.DIR_RIGHT;
			else if(e.KeyCode == Keys.Up)
				MunchControl.nextDir = (int)DirType.DIR_UP;
			else if(e.KeyCode == Keys.Down)
				MunchControl.nextDir = (int)DirType.DIR_DOWN;
		}

		private void menuItemStart_Click(object sender, System.EventArgs e)
		{
			GameControl.gameFunc = GameFunctions.GAME_INITGAME;
			menuItemPaused.Checked = GameControl.gamePaused = false;
		}

		private void menuItemQuit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void menuItemMusic_Click(object sender, System.EventArgs e)
		{
			DDSound.music = !DDSound.music;
			menuItemMusic.Checked = DDSound.music;
			if(DDSound.music && (GameControl.gameFunc != GameFunctions.GAME_NOTHING) && !GameControl.gamePaused)
			{
				DDSound.PlayLoop(DDSound.dsbMainTune);
			}
			else
			{
				DDSound.StopAllLoopSounds();
			}
		}

		private void menuItemEffects_Click(object sender, System.EventArgs e)
		{
			DDSound.effects = !DDSound.effects;
			menuItemEffects.Checked = DDSound.effects;
			if(!DDSound.music)
			{
				DDSound.StopAllFxSounds();
			}
		}

		private void menuItemPaused_Click(object sender, System.EventArgs e)
		{
			if(GameControl.gameFunc != GameFunctions.GAME_NOTHING)
			{
				GameControl.gamePaused = !GameControl.gamePaused;
				if(GameControl.gamePaused)
				{
					DDSound.StopAllSounds();
				}
				else
				{
					DDSound.PlayLoop(DDSound.dsbMainTune);
				}
			}
			else
			{
				GameControl.gamePaused = false;
			}
			menuItemPaused.Checked = GameControl.gamePaused;
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			GameControl.gamePaused = false;
			menuItemPaused_Click(sender, e);
			MessageBox.Show(this,   "Written by Dolo Miah\r" +
                                    "Sound by John Coleman\r" +
                                    "First written in C++ for\r" +
                                    "Windows 95 and DirectDraw!\r" +
                                    "(c) 1998-2018", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
			menuItemPaused_Click(sender, e);
		}

        private void menuItemInstructions_Click(object sender, EventArgs e)
        {
            GameControl.gamePaused = false;
            menuItemPaused_Click(sender, e);
            MessageBox.Show(this,   "Use the cursor keys to control the Pac-Man.\r\r" +
                                    "Eat all the dots whilst avoiding the ghosts\r" +
                                    "to complete a level.\r\r" +
                                    "Eating a power pill allows the ghosts to be\r" +
                                    "eaten.\r\r" +
                                    "Collecting 3 of the same fruit wins a bonus\r" +
                                    "to help complete the level!", "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);
            menuItemPaused_Click(sender, e);
        }
    }
}
