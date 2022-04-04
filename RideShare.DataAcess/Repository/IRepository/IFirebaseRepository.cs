using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.DataAcess.Repository.IRepository
{
   public  interface IFirebaseRepository
    {
        public  Task<string> Upload(Stream stream, string fileName, string DestinationFolder);
        public Task Delete(string fileName, string DestinationFolder);

        public Task<string> GetLink(string fileName, string DestinationFolder);



    }
}
