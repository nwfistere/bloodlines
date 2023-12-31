﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bloodlines
{
    public class SpriteImporter : MonoBehaviour
    {
        public static Texture2D LoadTexture(string FilePath)
        {
            Texture2D texture;

            if (!File.Exists(FilePath))
            {
                throw new ArgumentException($"FilePath does not exist. <{FilePath}>");
            }

            byte[] imageBytes = File.ReadAllBytes(FilePath);
            texture = new Texture2D(2, 2);

            if (!ImageConversion.LoadImage(texture, imageBytes))
            {
                throw new Exception("ImageConversion.LoadImage failed");
            }

            // Point makes the pixels come out much clearer.
            texture.filterMode = FilterMode.Point;
            texture.name = Path.GetFileNameWithoutExtension(FilePath);

            return texture;
        }

        // Not sure which one of these is actually ran ^v Interesting that C# allows this...
        public static Texture2D LoadTexture(string FilePath, TextureFormat format = TextureFormat.RGBA32, bool mipChain = true)
        {
            Texture2D texture;

            if (!File.Exists(FilePath))
            {
                throw new ArgumentException($"FilePath does not exist. <{FilePath}>");
            }

            byte[] imageBytes = File.ReadAllBytes(FilePath);
            texture = new Texture2D(2, 2, format, mipChain);

            if (!ImageConversion.LoadImage(texture, imageBytes))
            {
                throw new Exception("ImageConversion.LoadImage failed");
            }

            texture.filterMode = FilterMode.Point;
            texture.name = Path.GetFileNameWithoutExtension(FilePath);

            return texture;
        }

        static readonly Dictionary<Uri, TextureDownloader> downloadCache = new();

        public static Texture2D LoadTexture(Uri textureUri)
        {
            Texture2D texture;
            byte[]? imageBytes;
            TextureDownloader textureDownloader;

            if (downloadCache.ContainsKey(textureUri))
            {
                textureDownloader = downloadCache[textureUri];
            }
            else
            {
                textureDownloader = new(textureUri);
            }

            imageBytes = textureDownloader.GetBytes();
            texture = new Texture2D(2, 2);

            if (!ImageConversion.LoadImage(texture, imageBytes))
            {
                throw new Exception("ImageConversion.LoadImage failed");
            }

            downloadCache[textureUri] = textureDownloader;
            texture.filterMode = FilterMode.Point;
            texture.name = Path.GetFileNameWithoutExtension(textureUri.LocalPath);

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
                    {
                        return texture;
                    }
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
            Sprite sprite = Sprite.Create(texture, rect, pivot);
            sprite.name = texture.name;
            return sprite;
        }

        public static Sprite LoadCharacterSprite(string FilePath)
        {
            Texture2D texture = LoadTexture(FilePath);
            // Stadard characters have large values (400?) in the x y parameters of their Rect. I wonder what that's for.
            return LoadSprite(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0f));
        }

        public static Sprite? TryLoadSprite(string FilePath)
        {
            try
            {
                Texture2D? texture = TryLoadTexture(FilePath);

                if (texture == null)
                {
                    return null;
                }

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

                if (texture == null)
                {
                    return null;
                }

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