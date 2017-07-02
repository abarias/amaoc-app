using System;
using System.Data.Entity;

namespace Chevron.ITC.AMAOC.Backend.Models
{
    public class AMAOCContextInitializer : DropCreateDatabaseIfModelChanges<AMAOCContext>
    {
		protected override void Seed(AMAOCContext context)
		{
			//Seed Data Here
		}
    }
}
