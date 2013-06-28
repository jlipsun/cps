namespace FtpToSharePoint
{
    partial class FtpToSpService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._ftpWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this._ftpWatcher)).BeginInit();
            // 
            // _ftpWatcher
            // 
            this._ftpWatcher.EnableRaisingEvents = true;
            this._ftpWatcher.Created += new System.IO.FileSystemEventHandler(this._ftpWatcher_Created);
            // 
            // FtpToSpService
            // 
            this.CanPauseAndContinue = true;
            this.ServiceName = "FtpWatcher";
            ((System.ComponentModel.ISupportInitialize)(this._ftpWatcher)).EndInit();

        }

        #endregion

        private System.IO.FileSystemWatcher _ftpWatcher;

    }
}
