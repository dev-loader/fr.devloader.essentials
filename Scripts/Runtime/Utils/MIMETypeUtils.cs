/// Copyright 2024, Antonin Boureau, All rights reserved.
/// Version 20240501

using System.IO;

namespace Devloader.Utils
{
    public class MIMETypeUtils
    {
        public struct MIMETypeCollection
        {
            private string application;
            private string audio;
            private string image;
            private string model;
            private string text;
            private string video;

            public bool Has(string mimetype) => mimetype == application || mimetype == audio || mimetype == image || mimetype == model || mimetype == text || mimetype == video;

            public override string ToString()
            {
                if (application.Length > 0) return application;
                else if(audio.Length > 0) return audio;
                else if(image.Length > 0) return image;
                else if(model.Length > 0) return model;
                else if (text.Length > 0) return text;
                else return video;
            }

            public static MIMETypeCollection Make(string application = "", string audio = "", string image = "", string model = "", string text = "", string video = "")
            {
                MIMETypeCollection collection = new MIMETypeCollection();
                collection.application = application;
                collection.audio = audio;
                collection.image = image;
                collection.model = model;
                collection.text = text;
                collection.video = video;

                return collection;
            }

            public string Application { get => "application/" + application; }
            public string Audio { get => "audio/" + audio; }
            public string Image { get => "image/" + image; }
            public string Model { get => "model/" + model; }
            public string Text { get => "text/" + text; }
            public string Video { get => "video/" + video; }
        }
        
        public class MIMEExtensionCouple
        {
            public string extension;
            public MIMETypeCollection mimetypes;

            public MIMEExtensionCouple(string extension, MIMETypeCollection mimetypes)
            {
                this.extension = extension;
                this.mimetypes = mimetypes;
            }

            public static MIMEExtensionCouple Make(string extension, MIMETypeCollection mimetypes) => new MIMEExtensionCouple(extension, mimetypes);
        }

        public static MIMEExtensionCouple[] mimeExtensions = {
            MIMEExtensionCouple.Make("bin", MIMETypeCollection.Make(application: "octet-stream")),
            MIMEExtensionCouple.Make("css", MIMETypeCollection.Make(text: "css")),
            MIMEExtensionCouple.Make("gif", MIMETypeCollection.Make(image: "gif")),
            MIMEExtensionCouple.Make("html", MIMETypeCollection.Make(text: "html")),
            MIMEExtensionCouple.Make("js", MIMETypeCollection.Make(text: "javascript")),
            MIMEExtensionCouple.Make("jpeg", MIMETypeCollection.Make(image: "jpeg")),
            MIMEExtensionCouple.Make("json", MIMETypeCollection.Make(application: "json")),
            MIMEExtensionCouple.Make("mp3", MIMETypeCollection.Make(audio: "mp3")),
            MIMEExtensionCouple.Make("ogg", MIMETypeCollection.Make(application: "ogg", audio: "ogg", video: "ogg")),
            MIMEExtensionCouple.Make("png", MIMETypeCollection.Make(image: "png")),
            MIMEExtensionCouple.Make("svg", MIMETypeCollection.Make(image: "svg+xml")),
            MIMEExtensionCouple.Make("txt", MIMETypeCollection.Make(text: "plain")),
            MIMEExtensionCouple.Make("wav", MIMETypeCollection.Make(audio: "wav")),
            MIMEExtensionCouple.Make("webm", MIMETypeCollection.Make(audio: "webm", video: "webm")),
            MIMEExtensionCouple.Make("webp", MIMETypeCollection.Make(image: "webp")),
        };

        public static string GetExtension(string mimetype, bool returnWithADot = false)
        {
            string extension = returnWithADot ? "." : "";

            foreach (MIMEExtensionCouple couple in mimeExtensions)
                if (couple.mimetypes.Has(mimetype))
                    return extension + couple.extension;

            return extension + "bin";
        }

        public static MIMETypeCollection GetFromExtension(string fileExtension)
        {
            foreach (MIMEExtensionCouple couple in mimeExtensions)
                if (couple.extension == fileExtension)
                    return couple.mimetypes;

            return MIMETypeCollection.Make("application/octet-stream", "audio/wav", "image/jpeg", "model/obj", "text/plain", "video/mp4");
        }

        public static MIMETypeCollection GetFromFile(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            foreach (MIMEExtensionCouple couple in mimeExtensions)
                if (couple.extension == fileExtension)
                    return couple.mimetypes;

            return MIMETypeCollection.Make("application/octet-stream", "audio/wav", "image/jpeg", "model/obj", "text/plain", "video/mp4");
        }
    }
}
