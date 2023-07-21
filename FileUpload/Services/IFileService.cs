using FileUpload.Entities;

namespace FileUpload.Services
{
    public interface IFileService
    {
        public Task PostFileAsync(IFormFile fileData, FileType fileType, CancellationToken cancellationToken = default);
        public Task PostMultiFileAsync(List<FileUploadModel> fileData, CancellationToken cancellationToken = default);

        public Task DownloadFileById(int fileName, CancellationToken cancellationToken = default);
    }
}
