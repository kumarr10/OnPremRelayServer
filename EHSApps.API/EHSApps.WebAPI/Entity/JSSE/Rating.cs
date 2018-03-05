using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class JSSERating
    {
        private bool _ratingChecked = false;
        [DataMember]
        public int Rating_ID { get; set; }
        [DataMember]
        public string Rating { get; set; }
        [DataMember]
        public bool RatingChecked
        {
            get
            {
                return _ratingChecked;
            }
            set
            {
                _ratingChecked = value;
            }
        }

        [DataMember]
        public bool Selected { get; set; }
    }
}
