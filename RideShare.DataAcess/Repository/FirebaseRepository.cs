using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Options;
using RideShare.DataAcess.Data;
using RideShare.DataAcess.Repository.IRepository;
using RideShare.Utilities.Helpers.FirebaseHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RideShare.DataAcess.Repository
{

    public class FirebaseRepository:IFirebaseRepository
    {
        private readonly ApplicationDbContext _db;
        private  FirebaseSettings _firebaseOptions { get; set; }



        public FirebaseRepository(
            ApplicationDbContext db,
            IOptions<FirebaseSettings> firebaseOptions

            )
        {
            _db = db;
            _firebaseOptions = firebaseOptions.Value;

        }







        public async Task<string> Upload(Stream stream, string fileName, string DestinationFolder)
        {


            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseOptions.FirebaseApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseOptions.AuthEmail,_firebaseOptions.AuthPassword);

             // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                _firebaseOptions.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })

                .Child(DestinationFolder)
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);
        
            string link;
          try
            {
                link = await task;
                return link;
            }
            catch
            {
                link = null;
                return link;
           }



        }

        public async Task Delete(string fileName, string DestinationFolder)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseOptions.FirebaseApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseOptions.AuthEmail, _firebaseOptions.AuthPassword);

            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                _firebaseOptions.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })

                .Child(DestinationFolder)
                .Child(fileName)
                .DeleteAsync();
            //.PutAsync(stream, cancellation.Token);
            //.PutAsync(stream, cancellation.Token);

            //   task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // cancel the upload
            // cancellation.Cancel();

            try
            {
                //string link = await task;

            }
            catch
            {

            }

        }

        public async Task<string> GetLink(string fileName, string DestinationFolder)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebaseOptions.FirebaseApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebaseOptions.AuthEmail, _firebaseOptions.AuthPassword);

            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();

            var task = await new FirebaseStorage(
               _firebaseOptions.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })

                .Child(DestinationFolder)
                .Child(fileName)
                .GetDownloadUrlAsync();
            //.PutAsync(stream, cancellation.Token);
            //.PutAsync(stream, cancellation.Token);

            //   task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // cancel the upload
            // cancellation.Cancel();
            string link;
            try
            {
                if (task == null)
                {
                    return null;
                }
                link = task;

                return link;


            }
            catch
            {
                return null;
            }

        }










    }
}
