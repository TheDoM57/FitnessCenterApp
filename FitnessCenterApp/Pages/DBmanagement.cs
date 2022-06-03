using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using FitnessCenterApp.Model;

namespace FitnessCenterApp.Pages
{
    static class DBmanagement
    {
        static public readonly Core db = new Core();
        static public int StorePhoto(BitmapSource photo, Photo dbStruct)
        {
            byte[] data;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(photo));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            if (dbStruct == null)
            {
                dbStruct = new Photo();
                dbStruct.ImageData = data;
                db.context.Photo.Add(dbStruct);
            }
            else
            {
                dbStruct.ImageData = data;
            }
            if (db.context.SaveChanges() == 0)
            {
                return -1;
            }
            return dbStruct.Id;
        }
        static public (BitmapSource, Photo) LoadPhoto(int? Id)
        {
            if (Id == null || Id < 0)
                return (null, null);
            Photo photo;
            try
            {
                photo = db.context.Photo.Find(Id);
            }
            catch (InvalidOperationException)
            {
                return (null, null);
            }
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(photo.ImageData);
            bitmap.EndInit();
            return (bitmap, photo);
        }
    }
}
