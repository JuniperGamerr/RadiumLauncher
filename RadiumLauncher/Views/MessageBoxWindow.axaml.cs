using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace RadiumLauncher.Views;

public partial class MessageBoxWindow : Window
{
    private readonly Action? _callback;

    public MessageBoxWindow(string title, string content, Action? callback)
    {
        InitializeComponent();
        this.GetControl<TextBlock>("Title").Text = title;
        this.GetControl<TextBlock>("Content").Text = content;
        _callback = callback;
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    private void OkButton_Click(object? sender, RoutedEventArgs e)
    {
        _callback?.Invoke();
        Close();
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}