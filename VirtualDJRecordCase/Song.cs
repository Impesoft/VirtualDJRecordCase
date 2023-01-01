using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDJRecordCase
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Song
    {
        public string? Url { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public int? PlayCount { get; set; }
        public int? PlayCountFromLogs { get; set; }
        public decimal? Time { get; set; }
        public long? FileSize { get; set; }
        public int? LastPlayTime { get; set; }
        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>


        private long? filesizeField;

        private int? lastplaytimeField;

        private string? artistField;

        private string? titleField;

        private string? remixField;

        private decimal? songlengthField;

        /// <remarks/>
        public long? filesize
        {
            get
            {
                return this.filesizeField;
            }
            set
            {
                this.filesizeField = value;
            }
        }

        /// <remarks/>
        public int? lastplaytime
        {
            get
            {
                return this.lastplaytimeField;
            }
            set
            {
                this.lastplaytimeField = value;
            }
        }

        /// <remarks/>
        public string? artist
        {
            get
            {
                return this.artistField;
            }
            set
            {
                this.artistField = value;
            }
        }

        /// <remarks/>
        public string? title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string? remix
        {
            get
            {
                return this.remixField;
            }
            set
            {
                this.remixField = value;
            }
        }

        /// <remarks/>
        public decimal? songlength
        {
            get
            {
                return this.songlengthField;
            }
            set
            {
                this.songlengthField = value;
            }
        }


        // You can add other properties as needed, such as the genre, length, etc.
    }

}
