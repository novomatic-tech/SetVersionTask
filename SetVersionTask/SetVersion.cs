namespace SetVersionTask
{
    using System;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class SetVersion : Task
    {
        [Required]
        public string FileName { get; set; }

        public string AssemblyVersion { get; set; }
        public string AssemblyFileVersion { get; set; }
        public string AssemblyInformationalVersion { get; set; }

        public override bool Execute()
        {
            try
            {
                if (this.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    var updater = new CSharpUpdater(AssemblyVersion, AssemblyFileVersion, AssemblyInformationalVersion);
                    updater.UpdateFile(FileName);
                }
                else if (this.FileName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase))
                {
                    UpdateNuSpec();
                }
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
                return false;
            }
            return true;
        }

        private void UpdateNuSpec()
        {
        }
    }
}
