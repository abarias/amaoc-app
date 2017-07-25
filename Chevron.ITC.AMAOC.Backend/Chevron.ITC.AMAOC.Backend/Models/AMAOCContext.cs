using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Chevron.ITC.AMAOC.DataObjects;
using System.Collections.Generic;


namespace Chevron.ITC.AMAOC.Backend.Models
{
    public class AMAOCContext : DbContext
    {
		private const string connectionStringName = "Name=MS_TableConnectionString";

		public AMAOCContext() : base(connectionStringName)
        {            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Add(
				new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
					"ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
		}

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventAttendee> EventAttendees { get; set; }

        public DbSet<FeedbackAnswer> FeedbackAnswers { get; set; }

        public DbSet<FeedbackAnswerFreeText> FeedbackAnswerFreeTexts { get; set; }

        public DbSet<FeedbackQuestion> FeedbackQuestions { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<EventRatingComment> EventRatingComments { get; set; }
    }
}
