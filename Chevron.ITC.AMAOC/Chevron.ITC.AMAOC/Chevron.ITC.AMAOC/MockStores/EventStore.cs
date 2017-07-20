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
        bool initialized = false;
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
                Event.EventStatus eventStatus = Event.EventStatus.Completed;
                var isAttended = await eventAttendees.IsAttended(ev.Id);
                if (!isAttended && ev.IsCompleted)
                {
                    statusImage = "minus.png";
                    eventStatus = Event.EventStatus.Missed;
                }
                else if (!isAttended && !ev.IsCompleted)
                {
                    statusImage = "detail.png";
                    eventStatus = Event.EventStatus.NotStarted;
                }

                ev.StatusImage = statusImage;
                ev.OCEventStatus = eventStatus;
            }
            
            return events;
        }

        public async override Task<Event> GetItemAsync(string id)
        {
            if (!initialized)
                await InitializeStore();

            return Events.FirstOrDefault(s => s.Id == id);
        }

        public async Task<IEnumerable<Event>> GetNextEvents()
        {
            if (!initialized)
                await InitializeStore();

            var date = DateTime.UtcNow.AddMinutes(-30);

            var events = await GetItemsAsync();

            var results = (from ocEvent in events
                           where (ocEvent.StartTime.HasValue && ocEvent.StartTime.Value > date
                           && !ocEvent.IsCompleted)
                           orderby ocEvent.StartTime.Value
                           select ocEvent).Take(2);


            var enumerable = results as Event[] ?? results.ToArray();
            return !enumerable.Any() ? null : enumerable;

        }

        public override async Task InitializeStore()
        {
            if (initialized)
                return;

            initialized = true;
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

        public Task<Event> GetAppIndexEvent(string id)
        {
            return GetItemAsync(id);
        }
    }
}
