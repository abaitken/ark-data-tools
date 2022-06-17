using Renci.SshNet;

namespace ArkDataProcessor
{
    class SecureFileUploadPipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget>
    {
        internal override Task Execute(IEnumerable<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            var connectionInfo = new PasswordConnectionInfo(uploadTarget.Host, uploadTarget.Username, uploadTarget.Password);
            using (var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();

                foreach (var item in uploadItems)
                {
                    using (var stream = File.OpenRead(item.LocalPath))
                        sftp.UploadFile(stream, item.RemotePath, true);
                }

                sftp.Disconnect();
            }
            return Task.CompletedTask;
        }
    }
}
