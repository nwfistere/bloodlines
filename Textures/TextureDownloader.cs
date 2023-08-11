using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace EasyAddCharacter.Textures
{
    public class TextureDownloader
    {
        private readonly Uri textureUri;
        private byte[]? DownloadedBytes;

        public TextureDownloader(Uri textureUri)
        {
            this.textureUri = textureUri;
        }

        private void DownloadTexture()
        {
            UnityWebRequest? uwr = null;
            byte[] bytes;
            try
            {
                uwr = UnityWebRequest.Get(textureUri.AbsoluteUri);
                var request = uwr.SendWebRequest();

                while (!request.isDone)
                    System.Threading.Thread.Sleep(50);

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception(uwr.error);
                }

                bytes = uwr.downloadHandler.data;
            }
            catch
            {
                uwr?.Dispose();
                throw;
            }

            uwr.Dispose();
            DownloadedBytes = bytes;
        }

        public byte[]? GetBytes()
        {
            if (DownloadedBytes == null || DownloadedBytes?.Length == 0)
            {
                DownloadTexture();
            }

            return DownloadedBytes;
        }
    }

}
