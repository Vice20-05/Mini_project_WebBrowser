using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_project_WebBrowser
{
    // separate Form for adding a new keyword (Lab 4 C4)
    // OK / Cancel set the DialogResult, so ShowDialog() in Form1 knows which one was pressed
    public partial class victorform : Form
    {
        public victorform()
        {
            InitializeComponent();
        }

        // the text the user typed, without spaces at the ends
        public string GetKeyword()
        {
            return textBox1.Text.Trim();
        }
    }
}
