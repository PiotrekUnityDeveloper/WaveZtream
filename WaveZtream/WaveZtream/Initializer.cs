﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    public partial class Initializer : Form
    {
        public static Initializer activeInitializer;

        public Initializer()
        {
            InitializeComponent();
            activeInitializer = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = Color.Black;
            Initialize();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Initialize()
        {
            waveLogo.BringToFront();
            LoadingMessage.BringToFront();
            LoadingMessage.Text = "Initializing";
            CheckForFirstTimeInstallation();
        }

        public void CheckForFirstTimeInstallation()
        {
            string configPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //MessageBox.Show(configPath);

            if (!Directory.Exists(configPath))
            {
                MessageBox.Show("Your system appears to have no appdata folder. Store configurations in source folder? This means everything you configure is lost after uninstallation unless you backup it!");
            }
            else
            {
                if (!Directory.Exists(configPath + "\\WaveZtream\\"))
                {
                    RunFirstTimeInstallation();
                }
                else
                {
                    //directory exists, welcome back!
                }
            }
        }

        public async void RunFirstTimeInstallation()
        {
            LoadingMessage.Text = "Welcome to WaveZtream!";
            waveLogo.Image = WaveZtream.Properties.Resources.waveztream_legacylogo;
            await Task.Delay(2400);
            LoadingMessage.Hide();
            extendOperation = true;
            Initializer_Welcome init_welcome = new Initializer_Welcome();
            init_welcome.SendToBack();
            waveLogo.BringToFront();
            LoadingMessage.BringToFront();
            init_welcome.Location = new Point(0, 0);
            this.Controls.Add(init_welcome);
            init_welcome.Show();
            WindowsExtender.Start();
        }

        private bool extendOperation = true;

        private void WindowsExtender_Tick(object sender, EventArgs e)
        {
            //original size: 672; 403
            //extended size: 672; 833

            if (extendOperation)
            {
                //expand

                if(this.Size.Height < 833)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height + 5);
                    this.CenterToScreen();
                }
                else
                {
                    this.Size = new Size(this.Size.Width, 833);
                    this.CenterToScreen();
                    WindowsExtender.Stop();
                }
            }
            else
            {
                //shrink

                if (this.Size.Height > 403)
                {
                    this.Size = new Size(this.Size.Width, this.Size.Height + 5);
                    this.CenterToScreen();
                }
                else
                {
                    this.Size = new Size(this.Size.Width, 403);
                    this.CenterToScreen();
                    WindowsExtender.Stop();
                }
            }
        }
    }
}
