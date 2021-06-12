using MyApp.ServiceModel.Types;
using ServiceStack;

namespace MyApp.ServiceModel
{
    [ValidateIsAuthenticated]
    [AutoApply(Behavior.AuditQuery)]
    public class QueryLogClicks : QueryDb<LogClick>
    {

    }


    public class CreateLogClickResponse : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }
        public LogClick Result { get; set; }
    }


    [AutoApply(Behavior.AuditCreate)]
    [ValidateIsAuthenticated] 
    public class CreateLogClick: ICreateDb<LogClick>, IReturn<CreateLogClickResponse> { }

    [AutoApply(Behavior.AuditCreate)]
    [ValidateIsAuthenticated]
    [ValidateHasRole("fake")]
    public class CreateLogClickRequireFakeRole : ICreateDb<LogClick>, IReturn<CreateLogClickResponse> { }

    [ValidateIsAuthenticated]
    public class ClearAllLogClick : IReturnVoid { }
}