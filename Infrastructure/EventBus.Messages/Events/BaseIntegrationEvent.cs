using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BaseIntegrationEvent
    {
        //Co- relation id
        public Guid Id { get; private set; }
        public DateTime CreationDate { get;private set; }
        public BaseIntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        public BaseIntegrationEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
