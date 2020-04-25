using Onova;
using Onova.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfContainedUpdate
{
    public partial class UpdateForm : Form
    {
        private UpdateManager _updateManager;
        private CheckForUpdatesResult _result;

        public UpdateForm(UpdateManager updateManager, CheckForUpdatesResult result)
        {
            _updateManager = updateManager;
            _result = result;
            InitializeComponent();
            StartUpdate();
        }

        public void StartUpdate()
        {
            var progress = new Progress<double>(percent =>
            {
                UpdateProgress(Convert.ToInt32(percent * 100));
            });

            // Prepare an update by downloading and extracting the package
            // (supports progress reporting and cancellation)
            _updateManager.PrepareUpdateAsync(_result.LastVersion, progress).Wait();

            // Launch an executable that will apply the update
            // (can be instructed to restart the application afterwards)
            _updateManager.LaunchUpdater(_result.LastVersion);

            // Terminate the running application so that the updater can overwrite files
            Environment.Exit(0);
        }

        private void UpdateProgress(int progress)
        {
            progressBar.Invoke((Action)(() => progressBar.Value = progress));
        }
    }
}
