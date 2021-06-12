using System.Threading.Tasks;
using ServiceStack;
using MyApp.ServiceModel;
using MyApp.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace MyApp.ServiceInterface
{
    public class LogClickService: Service
    {
        public async Task DeleteAsync(ClearAllLogClick _)
        {
            await Db.DeleteAllAsync<LogClick>();
        }
    }
}
