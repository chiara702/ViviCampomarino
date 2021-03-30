using Plugin.Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ViviCampomarino {
    public class Database<T> {
        private IFirebaseFirestore current = CrossFirebaseFirestore.Current;
        public Database() { }
        public async void WriteDocument(String Patch, T oggetto) {
            await current.GetDocument(Patch).SetDataAsync(oggetto);
        }
        public async Task<T> ReadDocument(String Patch) {
            var ritorno = await current.GetDocument(Patch).GetDocumentSnapshotAsync<T>();
            T risultato = ritorno.Data;
            return risultato;
        }
        public ICollectionReference GetCollection(String Path) {
            return current.GetCollection(Path);
        }
    }

    class FirebaseStorage {
        public static async Task DownloadFromStorage(String storagePath, String localPath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            await child.DownloadFile(localPath).AwaitAsync();
        }
        public static async Task<Stream> DownloadStreamFromStorage(String storagePath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            return await child.GetStreamAsync(10000);
        }
        public static Task<String> DownloadUrlFromStorage(String storagePath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            return child.GetDownloadUrlAsync();
        }
        public static async Task UploadToStorage(String storagePath, String localPath) {
            var storage = Plugin.Firebase.Storage.CrossFirebaseStorage.Current;
            var child = storage.GetRootReference().GetChild(storagePath);
            await child.PutFile(localPath).AwaitAsync();
        }

    }

    public class Libro {
        [FirestoreProperty("Titolo")] public String Titolo { get; set; }
        [FirestoreProperty("Autori")] public String Autori { get; set; }
        [FirestoreProperty("Editore")] public String Editore { get; set; }
        [FirestoreProperty("ISBN")] public String ISBN { get; set; }
        [FirestoreProperty("Sommario")] public String Sommario { get; set; }
        [FirestoreProperty("DataPubblicazione")] public String DataPubblicazione { get; set; }
        [FirestoreProperty("Pagine")] public String Pagine { get; set; }
        [FirestoreProperty("Generi")] public String Generi { get; set; }
        [FirestoreProperty("Image")] public String Image { get; set; }
        [FirestoreProperty("Descrizione")] public String Descrizione { get; set; }
    }
}
