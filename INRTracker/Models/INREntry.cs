using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace INRTracker.Models
{
    public class INREntry : ValidatableBindableBase
    {
        #region Private Fields

        private DateTime _date;
        private float _inr;
        private float _doseMg;
        private float _doseMgAlternating;

        #endregion

        #region Properties

        public long INREntryID { get; set; }

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        [Range(0, float.MaxValue)]
        public float INR
        {
            get { return _inr; }
            set
            {
                SetProperty(ref _inr, value);
            }
        }

        [Range(0, float.MaxValue)]
        public float DoseMg
        {
            get { return _doseMg; }
            set
            {
                SetProperty(ref _doseMg, value);
            }
        }

        [Range(0, float.MaxValue)]
        public float DoseMgAlternating
        {
            get { return _doseMg; }
            set
            {
                SetProperty(ref _doseMgAlternating, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a random, mock INREntry object. Used as mock data for
        /// design mode.
        /// </summary>
        /// <returns></returns>
        public static INREntry GenerateRandomEntry()
        {
            return new INREntry()
            {
                INREntryID = -1,
                Date = Common.RandomDateTime(),
                DoseMg = Common.RandomFloat(0f, 10f),
                DoseMgAlternating = Common.RandomFloat(0, 10f),
                INR = Common.RandomFloat(1f, 4f)
            };
        }

        #endregion
    }
}
