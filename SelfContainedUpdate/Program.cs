using Onova;
using Onova.Models;
using Onova.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfContainedUpdate
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CheckForUpdates();

            Application.Run(new Form1());
        }

        static void CheckForUpdates()
        {
            // Configure to look for packages in specified directory and treat them as zips
            using (var manager = new UpdateManager(
                AssemblyMetadata.FromAssembly(
                Assembly.GetEntryAssembly(),
                Process.GetCurrentProcess().MainModule.FileName),
                new LocalPackageResolver("C:\\UpdateTest\\Packages", "*.zip"),
                new ZipPackageExtractor()))
            {
                // Check for updates
                var result = manager.CheckForUpdatesAsync().Result;
                if (result.CanUpdate)
                {
                    string message = $"v{result.LastVersion} is available. Would you like to update now?";
                    string caption = "Update Available";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult dialogResult;
                    dialogResult = MessageBox.Show(message, caption, buttons);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var form = new UpdateForm(manager, result);
                        Application.Run(form);
                        form.StartUpdate();
                    }
                }
            }
        }
    }
}
