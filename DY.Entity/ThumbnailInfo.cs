using System;
using System.Collections.Generic;
using System.Text;

namespace DY.Entity
{
    public class ThumbnailInfo
    {
        public ThumbnailInfo(string id, byte[] data,int gallery_id)
        {
            this.ID = id;
            this.Data = data;
            this.GalleryId = gallery_id;
        }


        private string id;
        public string ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        private byte[] thumbnail_data;
        public byte[] Data
        {
            get
            {
                return this.thumbnail_data;
            }
            set
            {
                this.thumbnail_data = value;
            }
        }

        private int gallery_id;
        public int GalleryId
        {
            get { return this.gallery_id; }
            set { this.gallery_id = value; }
        }
    }
}
