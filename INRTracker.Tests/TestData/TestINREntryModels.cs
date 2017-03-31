using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INRTracker.Models;

namespace INRTracker.Tests.TestData
{
    /// <summary>
    /// A static class containing test INREntry objects for testing classes implementing
    /// IINREntryRepository. Note that First, Second and Third Entry should match
    /// whatever data populates the test DB in the accompanying SQL script! Annoying 
    /// coupling, but such is SQL!
    /// </summary>
    public static class TestINREntryModels
    {
        public static INREntry FirstEntry = new INREntry()
        {
            INREntryID = 1,
            Date = DateTime.Parse("2015-02-11T16:00:00.000Z"),
            INR = 1.0f,
            DoseMg = 10f,
            DoseMgAlternating = 0f,
        };

        public static INREntry SecondEntry = new INREntry()
        {
            INREntryID = 2,
            Date = DateTime.Parse("2015-02-18T16:00:00.000Z"),
            INR = 3.1f,
            DoseMg = 5f,
            DoseMgAlternating = 0f
        };

        public static INREntry ThirdEntry = new INREntry()
        {
            INREntryID = 3,
            Date = DateTime.Parse("2015-02-25T16:00:00.000Z"),
            INR = 2.5f,
            DoseMg = 8.0f,
            DoseMgAlternating = 7.5f
        };

        public static INREntry NewEntry = new INREntry()
        {
            Date = DateTime.Parse("2015-03-03T16:00:00.000Z"),
            INR = 2.2f,
            DoseMg = 8.0f,
            DoseMgAlternating = 7.5f
        };

        public static INREntry NewEntryInvalid = new INREntry()
        {
            Date = DateTime.Parse("2015-03-03T16:00:00.000Z"),
            INR = -1.2f, // can't have negative INR!
            DoseMg = 8.0f,
            DoseMgAlternating = 7.5f
        };

        public static INREntry EntryInvalidID = new INREntry()
        {
            INREntryID = 10,
            Date = DateTime.Parse("2015-03-03T16:00:00.000Z"),
            INR = 1f, 
            DoseMg = 8.0f,
            DoseMgAlternating = 7.5f
        };

        public static INREntry UpdatedEntryValid = new INREntry()
        {
            INREntryID = 2,
            Date = DateTime.Parse("2015-02-18T16:00:00.000Z"),
            INR = 3.1f,
            DoseMg = 5f, // update the original dosing from 5mg to 5mg/4mg
            DoseMgAlternating = 4f
        };

        public static INREntry UpdatedEntryInvalid = new INREntry()
        {
            INREntryID = 2,
            Date = DateTime.Parse("2015-02-18T16:00:00.000Z"),
            INR = 3.1f,
            DoseMg = -1f, // tried to set the dose to a negative value, whoops
            DoseMgAlternating = 0f
        };
    }
}
