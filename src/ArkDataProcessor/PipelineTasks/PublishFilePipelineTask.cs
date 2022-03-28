using Renci.SshNet;

namespace ArkDataProcessor
{
    class PublishFilePipelineTask : DataProcessingPipelineTaskNoResult<string, UploadTarget>
    {
        internal override void Execute(string arg1, UploadTarget arg2)
        {
            switch (arg2.Scheme)
            {
                case "sftp":
                    ExecuteSFTP(arg1, arg2);
                    break;
                case "copy":
                    ExecuteCopy(arg1, arg2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg2.Scheme));
            }
        }

        private void ExecuteCopy(string sourceFile, UploadTarget uploadTarget)
        {
            File.Copy(sourceFile, uploadTarget.RemoteTarget, true);
        }

        private void ExecuteSFTP(string sourceFile, UploadTarget uploadTarget)
        {
            var connectionInfo = new PasswordConnectionInfo(uploadTarget.Host, uploadTarget.Username, uploadTarget.Password);
            using(var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();

                using (var stream = File.OpenRead(sourceFile))
                    sftp.UploadFile(stream, uploadTarget.RemoteTarget, true);

                sftp.Disconnect();
            }
        }
    }
}
