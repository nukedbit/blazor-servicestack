using ServiceStack;
using MyApp.ServiceModel;
using ServiceStack.IO;
using System.Linq;

namespace MyApp.ServiceInterface
{
    public class FileUploadService : Service
    {
        public void PostAsync(FileUpload _)
        { 
            foreach (var file in this.Request.Files)
            {
                var fileName = file.FileName;
                var filePath = $"/uploads/{fileName}";
                VirtualFiles.WriteFile(filePath, file.InputStream);                
            }
        }

        public object GetAsync(ListAllFiles _)
        {
            var dir = VirtualFiles.GetDirectory("/uploads");
            var files = dir.GetAllFiles();

            return new ListAllFilesResponse()
            {
                Files = files.Select(f => f.Name).ToList()
            };
        }
    }
}
