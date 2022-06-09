using Renci.SshNet;

namespace ArkDataProcessor
{
    class PublishFilePipelineTask : DataProcessingPipelineTaskNoResult<IEnumerable<UploadItem>, UploadTarget>
    {
        internal override void Execute(IEnumerable<UploadItem> arg1, UploadTarget arg2)
        {
            var uploadItems = arg1.ToList();

            if (uploadItems.Count == 0)
                return;

            switch (arg2.Scheme)
            {
                case "sftp":
                    ExecuteSFTP(uploadItems, arg2);
                    break;
                case "copy":
                    ExecuteCopy(uploadItems);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(arg2.Scheme));
            }
        }

        private void ExecuteCopy(List<UploadItem> uploadItems)
        {
            foreach (var item in uploadItems)
                File.Copy(item.LocalPath, item.RemotePath, true);
        }

        private void ExecuteSFTP(List<UploadItem> uploadItems, UploadTarget uploadTarget)
        {
            var connectionInfo = new PasswordConnectionInfo(uploadTarget.Host, uploadTarget.Username, uploadTarget.Password);
            using(var sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();

                foreach (var item in uploadItems)
                {
                    using (var stream = File.OpenRead(item.LocalPath))
                        sftp.UploadFile(stream, item.RemotePath, true);
                }

                sftp.Disconnect();
            }
        }
    }
}
