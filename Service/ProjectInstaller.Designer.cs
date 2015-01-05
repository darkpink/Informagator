namespace Acadian.Informagator.Service
{
    partial class ProjectInstaller
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
            this.InformagatorServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.InformagatorServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // InformagatorServiceProcessInstaller
            // 
            this.InformagatorServiceProcessInstaller.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.InformagatorServiceInstaller});
            this.InformagatorServiceProcessInstaller.Password = null;
            this.InformagatorServiceProcessInstaller.Username = null;
            // 
            // InformagatorServiceInstaller
            // 
            this.InformagatorServiceInstaller.Description = "Informagator Service";
            this.InformagatorServiceInstaller.DisplayName = "Informagator";
            this.InformagatorServiceInstaller.ServiceName = "InformagatorService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.InformagatorServiceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller InformagatorServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller InformagatorServiceInstaller;
    }
}