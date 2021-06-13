using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.ServiceModel
{
    [ValidateIsAuthenticated]
    public class FileUpload: IReturnVoid
    {

    }


    public class ListAllFilesResponse
    {
        public List<string> Files { get; set; }
    }

    [ValidateIsAuthenticated]
    public class ListAllFiles : IReturn<ListAllFilesResponse>
    {

    }
}
