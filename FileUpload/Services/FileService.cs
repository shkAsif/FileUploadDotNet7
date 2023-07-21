
using FileUpload.Data;
using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly DbContextClass dbContextClass;

        public FileService(DbContextClass dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task PostFileAsync(IFormFile fileData, FileType fileType, CancellationToken cancellationToken = default)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    ID = 0,
                    FileName = fileData.FileName,
                    FileType = fileType,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = dbContextClass.FileDetails.Add(fileDetails);
                await dbContextClass.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> fileData, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach(FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = 0,
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = dbContextClass.FileDetails.Add(fileDetails);
                }             
                await dbContextClass.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var file =  dbContextClass.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync(cancellationToken);

                var content = new System.IO.MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }        
        }
 
        public async Task CopyStream(Stream stream, string downloadPath, CancellationToken cancellationToken = default)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
               await stream.CopyToAsync(fileStream,cancellationToken);
            }
        }
    }
}
