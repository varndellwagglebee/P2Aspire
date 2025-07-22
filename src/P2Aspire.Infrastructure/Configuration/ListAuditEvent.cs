using System.Collections;
using Audit.Core;

namespace P2Aspire.Infrastructure;
public class ListAuditEvent : AuditEvent
{
    public ListAuditEvent( IList list ) => List = list;

    public IList List { get; set; }

}
