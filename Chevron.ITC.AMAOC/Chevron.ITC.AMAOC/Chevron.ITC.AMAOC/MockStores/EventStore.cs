using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class EventStore : BaseStore<Event>, IEventStore
    {
        List<Event> Events { get; }
        IEventAttendeeStore eventAttendees;

        public EventStore()
        {
            Events = new List<Event>();
            eventAttendees = DependencyService.Get<IEventAttendeeStore>();
        }

        public override async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore();

            var events = Events.OrderBy(s => s.StartTime).ToList();

            foreach (var ev in events)
            {
                string statusImage = "check.png";
                var isAttended = await eventAttendees.IsAttended(ev.Id);
                if (!isAttended && ev.IsCompleted)
                    statusImage = "minus.png";
                else if (!isAttended && !ev.IsCompleted)
                    statusImage = "detail.png";

                ev.StatusImage = statusImage;
            }
            
            return events;
        }

        public override async Task InitializeStore()
        {
            if (Events.Count != 0)
                return;

            var eventAttendeeList = await eventAttendees.GetItemsAsync();

            Events.Add(new Event
            {
                Id = "001",
                Name = "AMA OC Yammer Membership",
                Abstract = "Abstract of AMA OC Yammer Membership",
                StartTime = new DateTimeOffset(DateTime.Now),
                EndTime = new DateTimeOffset(DateTime.Now.AddHours(3)),
                Location = "PHRC - 34F L&D Room",
                Points = 20,
                IsCompleted = false
            });

            Events.Add(new Event
            {
                Id = "002",
                Name = "AIM Road Show",
                Abstract = "Abstract of AIM Road Show",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(2)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(2).AddHours(3)),
                Location = "PHRC - 34F L&D Room",
                Points = 30,
                IsCompleted = false
            });

            Events.Add(new Event
            {
                Id = "003",
                Name = "SAP Road Show",
                Abstract = "Abstract of SAP Road Show",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(3)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(3).AddHours(3)),
                Location = "PHRC - 34F L&D Room",
                Points = 20,
                IsCompleted = false
            });

            Events.Add(new Event
            {
                Id = "004",
                Name = "JDE Road Show",
                Abstract = "Abstract of JDE Road Show",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(4)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(4).AddHours(3)),
                Location = "PHRC - 34F L&D Room",
                Points = 30,
                IsCompleted = false
            });

            Events.Add(new Event
            {
                Id = "005",
                Name = "Agile Methodology",
                Abstract = "Abstract of Agile Methodology",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(-2)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(-2).AddHours(3)),
                Location = "PHRC - 34F L&D Room",
                Points = 10,
                IsCompleted = true
            });

            Events.Add(new Event
            {
                Id = "006",
                Name = "Intenet of Things",
                Abstract = "Abstract of Internet of Things",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(-1)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(-1).AddHours(2)),
                Location = "PHRC - 34F L&D Room",
                Points = 10,
                IsCompleted = true
            });

            Events.Add(new Event
            {
                Id = "007",
                Name = "Cloud Computing",
                Abstract = "Abstract of Cloud Computing",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(-3)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(-3).AddHours(2)),
                Location = "PHRC - 34F L&D Room",
                Points = 20,
                IsCompleted = true
            });

            Events.Add(new Event
            {
                Id = "008",
                Name = "Leap - Business Process",
                Abstract = "Abstract of Leap - Business Process",
                StartTime = new DateTimeOffset(DateTime.Now.AddMonths(1)),
                EndTime = new DateTimeOffset(DateTime.Now.AddMonths(1).AddHours(2)),
                Location = "PHRC - 34F L&D Room",
                Points = 30,
                IsCompleted = true
            });
        }
    }
}
