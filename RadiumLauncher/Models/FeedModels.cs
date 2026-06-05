using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Threading;
using RadiumLauncher.Views;

namespace RadiumLauncher.Models;

public class FeedResponse
{
    [JsonPropertyName("Results")]
    public List<FeedItem> Results { get; init; } = [];
}

public class AccountResponse
{
    [JsonPropertyName("accountId")]
    [JsonInclude]
    public int AccountId { get; init; }

    [JsonPropertyName("profileImage")]
    [JsonInclude]
    public string? ProfileImage { get; init; }

    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;

    [JsonIgnore]
    public string ProfileImageUrl => !string.IsNullOrEmpty(ProfileImage)
        ? $"https://img.radie.app/{ProfileImage}"
        : "https://launcher.radie.app/api/photos/v1/default-avatar";
}

public class RoomResponse
{
    [JsonPropertyName("RoomId")]
    [JsonInclude]
    public int RoomId { get; init; }

    [JsonPropertyName("Name")]
    public readonly string Name = string.Empty;
}

public partial class FeedItem : ObservableObject
{
    private static readonly HttpClient HttpClient = new();

    [JsonPropertyName("Id")]
    public int Id { get; init; }

    [JsonPropertyName("PlayerId")]
    [JsonInclude]
    public int PlayerId { get; init; }

    [JsonPropertyName("RoomId")]
    [JsonInclude]
    public int? RoomId { get; init; }

    [JsonPropertyName("ImageName")]
    [JsonInclude]
    public string ImageName { get; init; } = string.Empty;

    [JsonIgnore]
    private string ImageUrl => $"https://img.radie.app/{ImageName}";

    [ObservableProperty]
    private Bitmap? _bitmap;

    [ObservableProperty]
    private Bitmap? _largeBitmap;

    [ObservableProperty]
    private Bitmap? _profileBitmap;

    [ObservableProperty]
    private string _username = "Loading...";

    [ObservableProperty]
    private string _roomName = string.Empty;

    [ObservableProperty]
    private bool _isImageLoading;

    [ObservableProperty]
    private bool _isLargeLoading;

    public async Task LoadImage()
    {
        if (Bitmap != null || IsImageLoading) return;
        IsImageLoading = true;

        try
        {
            var data = await HttpClient.GetByteArrayAsync(ImageUrl);
            var bitmap = await Task.Run(() =>
            {
                using var stream = new MemoryStream(data);
                return Bitmap.DecodeToHeight(stream, 300);
            });
            Dispatcher.UIThread.Post(() => Bitmap = bitmap);
        }
        catch
        {
            Console.WriteLine("Failed to load image");
        }
        finally
        {
            IsImageLoading = false;
        }
    }

    public async Task LoadLargeImage()
    {
        if (LargeBitmap != null || IsLargeLoading) return;
        IsLargeLoading = true;
        try
        {
            var data = await HttpClient.GetByteArrayAsync(ImageUrl);
            var bitmap = await Task.Run(() =>
            {
                using var stream = new MemoryStream(data);
                return new Bitmap(stream);
            });
            Dispatcher.UIThread.Post(() => LargeBitmap = bitmap);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load large image");
            _ = new MessageBoxWindow("Image Loading Failed", ex.Message, null);
        }
        finally
        {
            IsLargeLoading = false;
        }
    }

    public async Task LoadProfileImage(string? url)
    {
        if (ProfileBitmap != null || string.IsNullOrEmpty(url)) return;
        try
        {
            var data = await HttpClient.GetByteArrayAsync(url);
            var bitmap = await Task.Run(() =>
            {
                using var stream = new MemoryStream(data);
                return Bitmap.DecodeToHeight(stream, 64);
            });
            Dispatcher.UIThread.Post(() => ProfileBitmap = bitmap);
        }
        catch
        {
            Console.WriteLine("Failed to load profile image");
        }
    }

    public void UnloadImage()
    {
        if (Bitmap != null)
        {
            Bitmap.Dispose();
            Bitmap = null;
        }

        if (LargeBitmap != null)
        {
            LargeBitmap.Dispose();
            LargeBitmap = null;
        }
    }

    public void FullCleanup()
    {
        UnloadImage();
        if (ProfileBitmap != null)
        {
            ProfileBitmap.Dispose();
            ProfileBitmap = null;
        }
    }
}