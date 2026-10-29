using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_project_WebBrowser
{
    public partial class Form1 : Form
    {
        // page opened by the Home button and at startup
        private const string HomePage = "https://www.google.com";

        // SQLite helper (Lab 4), keywords.db is created next to the .exe
        private SQLiteHandler db = new SQLiteHandler("Data Source=keywords.db");

        // blocked keywords, loaded from the database (used by IsBlocked and the ComboBox)
        private List<string> keywords = new List<string>();

        public Form1()
        {
            InitializeComponent();
            Trace.Listeners.Add(new TextWriterTraceListener("log.txt"));
            Trace.AutoFlush = true; //write to the file right away
            webBrowser1.ScriptErrorsSuppressed = true; //suppress script errors
        }

        // at startup: connect to the db in the background (no Connect button for the user),
        // load the keywords into the ComboBox, open the home page
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                db.ConnectToDB();
                LoadKeywords();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The list of blocked words could not be opened.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Log("App started");
            webBrowser1.Navigate(HomePage);
        }

        // when the app closes: close the db connection
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.DisconnectFromDB();
            // written directly (not with Log), the app may exit before a background task runs
            Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: App closed");
        }


        // write time (with ms) + message to log.txt, caller does not wait
        private void Log(string message)
        {
            Task.Run(() =>
            {
                Trace.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {message}");
            });
        }

        // read the keywords from the database and refresh the ComboBox
        private void LoadKeywords()
        {
            keywords = db.GetAllKeywords();
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.AddRange(keywords.ToArray());
            if (keywords.Count > 0)
            {
                toolStripComboBox1.SelectedIndex = 0;
            }
        }


        // LINQ: true if any keyword is inside the url
        private bool IsBlocked(string url)
        {
            string lowerUrl = url.ToLower();
            return keywords.Any(keyword => lowerUrl.Contains(keyword.ToLower()));
        }

        // same LINQ check, run on another thread
        private Task<bool> IsBlockedAsync(string url)
        {
            return Task.Run(() => IsBlocked(url));
        }

        // turns what the user typed into an address:
        // "google.com" -> opened as a site, "cat pictures" -> searched on Google
        private string MakeUrl(string text)
        {
            if (text.Contains(" ") || !text.Contains("."))
            {
                return "https://www.google.com/search?q=" + Uri.EscapeDataString(text);
            }
            return text;
        }

        //back
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
            Log("Back button clicked");
        }

        //home
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(HomePage);
            Log("Home button clicked");
        }


        //forward
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
            Log("Forward button clicked");
        }


        //go
        private async void toolStripButton4_Click(object sender, EventArgs e)
        {
            string url = toolStripTextBox1.Text.Trim();
            if (url == "") return; // nothing typed

            if (await IsBlockedAsync(url))
            {
                MessageBox.Show("This page is blocked.", "Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Log("Blocked URL: " + url);
            }
            else
            {
                webBrowser1.Navigate(MakeUrl(url));
                Log("Navigated to: " + url);
            }
        }

        // Enter in the address box works like Go
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                toolStripButton4_Click(sender, e); // same as clicking Go
                e.SuppressKeyPress = true; // no "ding" sound
            }
        }

        // runs before every page loads (also clicked links), must cancel right away so it stays sync
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string url = e.Url.ToString();
            if (IsBlocked(url))
            {
                e.Cancel = true; // stop the page from loading
                MessageBox.Show("This page is blocked.", "Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Log("Blocked URL: " + url);
            }
        }

        // after a page opens, show its address in the textbox
        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            toolStripTextBox1.Text = e.Url.ToString();
        }

        // when the page finished loading, show its title in the window title
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Text = webBrowser1.DocumentTitle + " - Browser Yes";
        }

        // Back / Forward are greyed out when there is no page to go to
        private void webBrowser1_CanGoBackChanged(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = webBrowser1.CanGoBack;
        }

        private void webBrowser1_CanGoForwardChanged(object sender, EventArgs e)
        {
            toolStripButton3.Enabled = webBrowser1.CanGoForward;
        }


        // add a new keyword through the separate form (victorform)
        private void AddKeyword()
        {
            if (!db.IsConnected())
            {
                MessageBox.Show("The list of blocked words is not available.");
                return;
            }

            // using: the form is disposed after it is closed
            using (victorform form = new victorform())
            {
                if (form.ShowDialog(this) != DialogResult.OK) return; // Cancel pressed

                string word = form.GetKeyword().ToLower();
                if (word == "")
                {
                    MessageBox.Show("No word entered.");
                    return;
                }

                if (keywords.Any(k => k.ToLower() == word)) // LINQ duplicate check
                {
                    MessageBox.Show("This word is already blocked.");
                    return;
                }

                db.InsertKeyword(word);
                LoadKeywords(); // the list refreshes automatically after a new word
                toolStripComboBox1.SelectedItem = word;
                Log("Keyword added: " + word);
            }
        }

        // delete the keyword selected in the ComboBox (asks first)
        private void DeleteSelectedKeyword()
        {
            if (!db.IsConnected())
            {
                MessageBox.Show("The list of blocked words is not available.");
                return;
            }

            if (toolStripComboBox1.SelectedItem == null)
            {
                MessageBox.Show("Choose a word from the list first.");
                return;
            }

            string word = toolStripComboBox1.SelectedItem.ToString();
            DialogResult answer = MessageBox.Show(
                $"Remove '{word}' from the blocked words?\nPages containing it will open again.",
                "Delete blocked word", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes) return;

            db.DeleteKeyword(word);
            LoadKeywords();
            Log("Keyword deleted: " + word);
        }

        // toolbar Add button
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            AddKeyword();
        }

        // toolbar Delete button
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DeleteSelectedKeyword();
        }

        // menu: Blocked words > Add
        private void addKeywordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddKeyword();
        }

        // menu: Blocked words > Delete
        private void deleteKeywordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedKeyword();
        }

        // menu: Help > About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Browser Yes - a simple web browser.\n\n" +
                "Type an address or a search in the box and press Enter.\n" +
                "Pages that contain a blocked word will not open.\n" +
                "Add or remove blocked words with the buttons on the right.",
                "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
