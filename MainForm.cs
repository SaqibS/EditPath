namespace EditPath
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class MainForm : Form
    {
        private List<string> paths;
        private BindingSource pathsSource;

        private ListBox pathList = new ListBox();
        private Button moveUpButton = new Button();
        private Button moveDownButton = new Button();
        private Button addButton = new Button();
        private Button removeButton = new Button();
        private Button cleanUpButton = new Button();

        public MainForm()
        {
            this.GetPaths();
            this.InitializeUI();
        }

        private void GetPaths()
        {
            this.paths = new List<string>(Environment.GetEnvironmentVariable("path").Split(';'));
            this.pathsSource = new BindingSource { DataSource = this.paths };
        }

        private void InitializeUI()
        {
            this.Text = "Edit Path";
            this.Size = new Size(320, 240);
            this.FormClosing += delegate(object sender, FormClosingEventArgs e)
            {
                SavePath();
            };

            this.pathList.Dock = DockStyle.Fill;
            this.pathList.DataSource = this.pathsSource;
            this.Controls.Add(this.pathList);
            this.pathList.Focus();

            InitializeButton(this.moveUpButton, "Move &Up");
            InitializeButton(this.moveDownButton, "Move &Down");
            InitializeButton(this.addButton, "&Add…");
            InitializeButton(this.removeButton, "&Remove");
            InitializeButton(this.cleanUpButton, "&Clean up…");
        }

        private void InitializeButton(Button b, string text)
        {
            b.Text = text;
            b.Dock = DockStyle.Bottom;
            b.Click += ButtonClicked;
            this.Controls.Add(b);
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            int index = this.pathList.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            if (sender == this.moveUpButton && index > 0)
            {
                Swap(index, index - 1);
            }
            else if (sender == this.moveDownButton && index < this.pathsSource.Count - 1)
            {
                Swap(index, index + 1);
            }
            else if (sender == this.addButton)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Select folder to add to path";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        this.pathsSource.Add(dlg.SelectedPath);
                    }
            }
            else if (sender == this.removeButton)
            {
                this.pathsSource.RemoveAt(index);
            }
            else if (sender == this.cleanUpButton)
            {
                CleanUpPaths();
            }
        }

        private void Swap(int i, int j)
        {
            string temp = (string)this.pathsSource[i];
            this.pathsSource[i] = this.pathsSource[j];
            this.pathsSource[j] = temp;
        }

        private void CleanUpPaths()
        {
            Dictionary<string, bool> seenBefore = new Dictionary<string, bool>();
            for (int i = 0; i < this.pathsSource.Count; i++)
            {
                string path = (string)this.pathsSource[i];
                if (!Directory.Exists(path) || seenBefore.ContainsKey(path))
                {
                    this.pathsSource.RemoveAt(i);
                    i--;
                }

                seenBefore[path] = true;
            }
}

        private void SavePath()
        {
            string newPath = string.Join(";", this.paths.ToArray());
            Environment.SetEnvironmentVariable("path", newPath, EnvironmentVariableTarget.Machine);
        }
    }
}
