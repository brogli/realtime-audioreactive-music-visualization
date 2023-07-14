using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class ImageSequenceLoader
{
    public async Task<ImageSequence> LoadImageSequenceInFolder(string folderpath)
    {
        string[] filePaths = Directory.GetFiles(folderpath);

        Texture2D[] imageSequence = new Texture2D[filePaths.Length];


        for (int i = 0; i < filePaths.Length; i++)
        {
            imageSequence[i] = await LoadFileAsync(filePaths[i]);
        }

        return new ImageSequence(imageSequence);
    }
    private async Task<Texture2D> LoadFileAsync(string filePath)
    {
        byte[] result;
        Texture2D texture = null;

        using (FileStream SourceStream = File.Open(filePath, FileMode.Open))
        {
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
        }
        texture = new Texture2D(2, 2);
        texture.LoadImage(result);
        return texture;
    }
}
