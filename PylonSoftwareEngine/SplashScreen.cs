using PylonSoftwareEngine.Mathematics;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PylonSoftwareEngine
{
    public class SplashScreen
    {
        private INTERNALSplashScreen INTERNALSplashScreen;
        private bool HasImage = false;

        public SplashScreen(Image Image, bool UseProgressbar = false)
        {
            INTERNALSplashScreen = new INTERNALSplashScreen(Image, UseProgressbar);
            if (Image != null)
                HasImage = true;
        }

        public void SetProgress(float Progress)
        {
            if (HasImage)
                INTERNALSplashScreen.SetProgress(Progress);
        }

        public void ShowAsync()
        {
            if (HasImage)
                INTERNALSplashScreen.ShowAsync();
        }

        public void Close()
        {
            if (HasImage)
                INTERNALSplashScreen.Close();
        }
    }

    internal class INTERNALSplashScreen : Form
    {
        private ProgressBar ProgressBar;
        public INTERNALSplashScreen(Image Image, bool UseProgressbar = false)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (Image == null)
                return;
            Size = Image.Size;

            if (UseProgressbar)
            {

                int height = 20;
                ProgressBar = new ProgressBar();
                ProgressBar.Size = new Size(this.Size.Width, height);
                ProgressBar.Location = new Point(0, this.Size.Height - height);
                ProgressBar.Style = ProgressBarStyle.Continuous;
                ProgressBar.BackColor = Color.Black;
                ProgressBar.ForeColor = Color.Red;
                ProgressBar.Maximum = this.Size.Width;
                this.Controls.Add(ProgressBar);
            }

            TopMost = true;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImage = Image;
            BackColor = Color.FromArgb(1, 1, 1);
            TransparencyKey = Color.FromArgb(1, 1, 1);
        }

        public void SetProgress(float Progress)
        {
            if (ProgressBar == null)
                return;
            float val = Mathf.Clamp(Progress, 0f, 1f);
            this.Invoke(new Action(() => { ProgressBar.Value = (int)(val * this.Size.Width); }));
        }

        public void ShowAsync()
        {
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
            Thread t = new Thread(() =>
            {
                //this.Show();
                Application.Run(this);
            });

            t.Start();
        }

        new public void Close()
        {
            base.Hide();
            base.Close();
            base.Dispose();
        }
    }
}