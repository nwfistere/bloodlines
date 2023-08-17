using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Bloodlines
{
    public class SpriteImporter : MonoBehaviour
    {
        public static Texture2D LoadTexture(string FilePath)
        {
            Texture2D texture;

            if (!File.Exists(FilePath))
            {
                throw new ArgumentException("FilePath does not exist.");
            }

            byte[] imageBytes = File.ReadAllBytes(FilePath);
            texture = new Texture2D(2, 2);
            if (!ImageConversion.LoadImage(texture, imageBytes))
            {
                throw new Exception("ImageConversion.LoadImage failed");
            }

            return texture;
        }

        public static Texture2D LoadTexture(string FilePath, GraphicsFormat format = GraphicsFormat.None, TextureCreationFlags flags = TextureCreationFlags.None) //Texture2D(int width, int height, GraphicsFormat format, TextureCreationFlags flags)
        {
            Texture2D texture;

            if (!File.Exists(FilePath))
            {
                throw new ArgumentException("FilePath does not exist.");
            }

            byte[] imageBytes = File.ReadAllBytes(FilePath);
            texture = new Texture2D(2, 2, format, flags);
            if (!ImageConversion.LoadImage(texture, imageBytes))
            {
                throw new Exception("ImageConversion.LoadImage failed");
            }

            return texture;
        }

        private static readonly Dictionary<Uri, TextureDownloader> downloadCache = new();

        public static Texture2D LoadTexture(Uri textureUri)
        {
            Texture2D texture;
            byte[]? imageBytes;
            TextureDownloader textureDownloader;

            if (downloadCache.ContainsKey(textureUri))
                textureDownloader = downloadCache[textureUri];
            else
            {
                textureDownloader = new(textureUri);
            }

            imageBytes = textureDownloader.GetBytes();
            texture = new Texture2D(2, 2);

            if (!ImageConversion.LoadImage(texture, imageBytes))
                throw new Exception("ImageConversion.LoadImage failed");
            downloadCache[textureUri] = textureDownloader;

            return texture;
        }

        public static Texture2D? TryLoadTexture(Uri textureUri)
        {
            try
            {
                return LoadTexture(textureUri);
            }
            catch { }
            return null;
        }

        public static Texture2D? TryLoadTexture(string FilePath)
        {
            try
            {
                Texture2D texture;

                if (File.Exists(FilePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(FilePath);
                    texture = new Texture2D(2, 2);
                    if (ImageConversion.LoadImage(texture, imageBytes))
                        return texture;
                }
            }
            catch { }
            return null;
        }

        public static Sprite LoadSprite(string FilePath)
        {
            Texture2D texture = LoadTexture(FilePath);
            return LoadSprite(texture);
        }

        public static Sprite LoadSprite(Uri textureUri)
        {
            Texture2D texture = LoadTexture(textureUri);
            return LoadSprite(texture);
        }

        public static Sprite LoadSprite(string FilePath, Rect rect, Vector2 pivot)
        {
            Texture2D texture = LoadTexture(FilePath);
            return LoadSprite(texture, rect, pivot);
        }

        public static Sprite LoadSprite(Texture2D texture)
        {
            return LoadSprite(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        public static Sprite LoadSprite(Texture2D texture, Rect rect, Vector2 pivot)
        {
            return Sprite.Create(texture, rect, pivot);
        }

        public static Sprite? TryLoadSprite(string FilePath)
        {
            try
            {
                Texture2D? texture = TryLoadTexture(FilePath);
                if (texture == null) return null;
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            catch { }
            return null;
        }

        public static Sprite? TryLoadSprite(string FilePath, Rect rect, Vector2 pivot)
        {
            try
            {
                Texture2D? texture = TryLoadTexture(FilePath);
                if (texture == null) return null;
                return Sprite.Create(texture, rect, pivot);
            }
            catch { }
            return null;
        }

        public static Sprite? TryLoadSprite(Texture2D texture, Rect rect, Vector2 pivot)
        {
            try
            {
                return LoadSprite(texture, rect, pivot);
            }
            catch { }
            return null;
        }

        public static Sprite? TryLoadSprite(Texture2D texture)
        {
            try
            {
                return LoadSprite(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            catch { }
            return null;
        }
    }

}
